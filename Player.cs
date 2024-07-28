using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDRPG
{
    public class Player
    {
        public int X = 0;
        public int Y = 0;
        public double SubPixelX = 0.0;
        public double SubPixelY = 0.0;
        public double VelocityX = 0.0;
        public double VelocityY = 0.0;
        public double AccelerationX = 0.0;
        public double AccelerationY = 0.0;
        public int Rotation = 0; //In range from 0 to _sprite.Width;

        public const double MoveSpeed = 2.5;

        private Texture2D _boarder;
        private int _boarderWidth;
        private int _boarderHeight;
        private Texture2D _sprite;
        private int _spriteWidth;
        private XNAInterface _xnaInterface;
        private int _cameraOffsetX;
        private int _cameraOffsetY;
        public Player(XNAInterface xnaInterface)
        {
            _xnaInterface = xnaInterface;

            _boarder = xnaInterface.LoadTexture("MDRPG.PlayerBoarder.png");
            _boarderWidth = _boarder.Width;
            _boarderHeight = _boarder.Height;

            _sprite = xnaInterface.LoadTexture("MDRPG.PlayerSprite.png");
            if (_sprite.Height != _boarderHeight)
            {
                throw new Exception("Da fuck is this shit.");
            }
            _spriteWidth = _sprite.Width;

            _cameraOffsetX = ((-1 * _xnaInterface.RenderWidth) / 2) + (_boarderWidth / 2);
            _cameraOffsetY = ((-1 * _xnaInterface.RenderHeight) / 2) + (_boarderWidth / 2);
        }
        private const double Tao = 6.2831853071795862;
        public void Update()
        {
            InputUpdate();
            PhysicsUpdate();
            CameraUpdate();
            Render();
        }
        public void InputUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.D))
            {
                VelocityX = MoveSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                VelocityX = -MoveSpeed;
            }
            else
            {
                VelocityX = 0.0;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                VelocityY = MoveSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                VelocityY = -MoveSpeed;
            }
            else
            {
                VelocityY = 0.0;
            }

            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X - (_xnaInterface.XNAWindow.ClientBounds.Width / 2);
            int mouseY = -(mouseState.Y - (_xnaInterface.XNAWindow.ClientBounds.Height / 2));

            double mouseRotation = Math.Atan2(mouseY, mouseX);
            if (mouseRotation < 0)
            {
                mouseRotation = Tao + mouseRotation;
            }

            Rotation = (int)((mouseRotation / Tao) * _spriteWidth);
        }
        public void PhysicsUpdate()
        {
            VelocityX += AccelerationX;
            VelocityY += AccelerationY;
            SubPixelX += VelocityX;
            SubPixelY += VelocityY;
            X = (int)SubPixelX;
            Y = (int)SubPixelY;
        }
        public void CameraUpdate()
        {
            _xnaInterface.CameraX = X + _cameraOffsetX;
            _xnaInterface.CameraY = Y + _cameraOffsetY;
        }
        public void Render()
        {
            _xnaInterface.DrawTexture(_boarder, X, Y);

            if (Rotation is 0)
            {
                _xnaInterface.DrawTexture(_sprite, X, Y, 0, 0, _boarderWidth, _boarderHeight);
            }
            else if (Rotation >= 16)
            {
                _xnaInterface.DrawTexture(_sprite, X, Y, _spriteWidth - Rotation, 0, _boarderWidth, _boarderHeight);
            }
            else
            {
                _xnaInterface.DrawTexture(_sprite, X + Rotation, Y, 0, 0, _boarderWidth - Rotation, _boarderHeight);
                _xnaInterface.DrawTexture(_sprite, X, Y, _spriteWidth - Rotation, 0, Rotation, _boarderHeight);
            }
        }
    }
}
