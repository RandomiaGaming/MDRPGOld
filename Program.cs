using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MDRPG
{
    public static class Program
    {
        public static Texture2D playerTexture;
        public static XNAInterface xnaInterface;
        public static Player player;
        public static World world;
        [STAThread]
        public static int Main(string[] args)
        {
            xnaInterface = new XNAInterface(Update);

            Initialize();

            xnaInterface.Run();

            return 0;
        }
        public static void Initialize()
        {
            player = new Player(xnaInterface);
            world = new World(xnaInterface);
            world.Generate();
        }
        public static void Update()
        {
            world.Update();
            player.Update();
        }
    }
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

            int mouseX = mouseState.X - (_xnaInterface.Window.ClientBounds.Width / 2);
            int mouseY = -(mouseState.Y - (_xnaInterface.Window.ClientBounds.Height / 2));

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
            _xnaInterface.DrawSprite(_boarder, X, Y);

            /* if (Rotation is 0)
             {
                 _xnaInterface.XNASpriteBatch.Draw(_sprite, new Rectangle((int)X, (int)Y, _boarderWidth, _boarderHeight), new Rectangle(0, 0, _boarderWidth, _boarderHeight), Color.White);
             }
             else if (Rotation >= 16)
             {
                 _xnaInterface.XNASpriteBatch.Draw(_sprite, new Rectangle((int)X, (int)Y, _boarderWidth, _boarderHeight), new Rectangle(_spriteWidth - Rotation, 0, _boarderWidth, _boarderHeight), Color.White);
             }
             else
             {
                 _xnaInterface.XNASpriteBatch.Draw(_sprite, new Rectangle((int)X + Rotation, (int)Y, _boarderWidth - Rotation, _boarderHeight), new Rectangle(0, 0, _boarderWidth - Rotation, _boarderHeight), Color.White);
                 _xnaInterface.XNASpriteBatch.Draw(_sprite, new Rectangle((int)X, (int)Y, Rotation, _boarderHeight), new Rectangle(_spriteWidth - Rotation, 0, Rotation, _boarderHeight), Color.White);
             }*/
        }
    }
    public class World
    {
        private Random RNG = new Random((int)DateTime.Now.Ticks);

        public const int Width = 64;
        public const int Height = 64;
        public Tile[][] TileData;
        public World()
        {
            TileData = new Tile[Width][];
            for (int column = 0; column < Width; column++)
            {
                TileData[column] = new Tile[Height];
            }
        }
        public Tile GroundTile;
        public Tile SandTile;
        private XNAInterface _xnaInterface;
        public World(XNAInterface xnaInterface)
        {
            _xnaInterface = xnaInterface;

            GroundTile = new Tile();
            GroundTile.Texture = _xnaInterface.LoadTexture("MDRPG.GroundTile.png");
            GroundTile.SeedChance = 0.05;
            GroundTile.SpreadChance = 1;

            SandTile = new Tile();
            SandTile.Texture = _xnaInterface.LoadTexture("MDRPG.SandTile.png");
            SandTile.SeedChance = 0.01;
            SandTile.SpreadChance = 1;
        }
        public void Generate()
        {
            int totalTileCount = Width * Height;
            int generatedCount = 0;


            TileData = new Tile[Width][];
            for (int x = 0; x < Width; x++)
            {
                TileData[x] = new Tile[Height];
                for (int y = 0; y < Height; y++)
                {
                    if (RNG.NextDouble() < GroundTile.SeedChance)
                    {
                        TileData[x][y] = GroundTile;
                    }
                    else if (RNG.NextDouble() < SandTile.SeedChance)
                    {
                        TileData[x][y] = SandTile;
                    }
                }
            }
        }
        public void Update()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile tile = TileData[x][y];
                    if (tile != null)
                    {
                        _xnaInterface.DrawSprite(tile.Texture, x * 16, y * 16);
                    }
                }
            }
        }
    }
    public struct TileData
    {
        public short Temperature;
        public Texture2D Texture;
    }
    public class Tile
    {
        public double SeedChance;
        public double SpreadChance;
        public Texture2D Texture;
    }
}