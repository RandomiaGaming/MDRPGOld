using System;
using System.Diagnostics;
using System.Collections.Generic;
namespace MDRPG
{
    public sealed class MonoGameInterface : Microsoft.Xna.Framework.Game
    {
        public readonly MDRPG.MDRPGGame dmGame = null;

        private MDRPG.Texture epsilonFrameBuffer;
        private Microsoft.Xna.Framework.Graphics.Texture2D frameBuffer;
        private Microsoft.Xna.Framework.Color[] frameColorBuffer;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

        private int lastScrollWheelValue = 0;
        public MonoGameInterface()
        {
            dmGame = new MDRPG.MDRPGGame();

            _ = new Microsoft.Xna.Framework.GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };

            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Window.IsBorderless = false;
            Window.Title = "Don't Melt! - 1.0.0";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            TargetElapsedTime = new TimeSpan(10000000 / 60);
        }
        private MDRPG.InputPacket CreateInputPacket()
        {
            Microsoft.Xna.Framework.Input.KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            List<MDRPG.KeyboardButton> pressedKeyboardButtons = new List<MDRPG.KeyboardButton>();
            foreach (Microsoft.Xna.Framework.Input.Keys key in keyboardState.GetPressedKeys())
            {
                switch (key)
                {
                    case Microsoft.Xna.Framework.Input.Keys.A:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.A);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.B:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.B);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.C:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.C);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.D:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.D);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.E:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.E);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.G:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.G);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.H:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.H);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.I:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.I);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.J:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.J);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.K:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.K);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.L:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.L);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.M:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.M);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.N:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.N);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.O:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.O);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.P:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.P);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Q:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Q);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.R:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.R);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.S:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.S);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.T:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.T);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.U:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.U);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.V:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.V);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.W:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.W);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.X:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.X);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Y:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Y);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Z:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Z);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad0:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad0);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad1:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad1);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad2:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad2);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad3:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad3);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad4:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad4);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad5:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad5);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad6:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad6);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad7:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad7);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad8:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad8);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumPad9:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPad9);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.OemPlus:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPadPlus);
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Plus);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.OemMinus:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPadMinus);
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Minus);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.NumLock:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumLock);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.LeftShift:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.LeftShift);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.RightShift:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.RightShift);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.LeftControl:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.LeftControl);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.RightControl:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.RightControl);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.LeftAlt:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.LeftAlt);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.RightAlt:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.RightAlt);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.LeftWindows:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.LeftWindows);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.RightWindows:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.RightWindows);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F1:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F1);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F2:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F2);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F3:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F3);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F4:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F4);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F5:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F5);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F6:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F6);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F7:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F7);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F8:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F8);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F9:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F9);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F10:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F10);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F11:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F11);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.F12:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.F12);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Back:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Backspace);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Delete:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Delete);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Scroll:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.ScrollLock);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Escape:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Escape);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Tab:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Tab);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.OemTilde:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Tilde);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Space:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Space);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.PrintScreen:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.PrintScreen);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Insert:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Insert);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Home:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Home);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.PageUp:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.PageUp);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.PageDown:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.PageDown);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.End:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.End);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.OemBackslash:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Backslash);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Divide:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Slash);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.OemComma:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Comma);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.OemPeriod:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Period);
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.NumPadPoint);
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Help:
                        pressedKeyboardButtons.Add(MDRPG.KeyboardButton.Help);
                        break;
                }
            }

            MDRPG.KeyboardState dmKeyboardState = new MDRPG.KeyboardState(keyboardState.CapsLock, false, keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift | Microsoft.Xna.Framework.Input.Keys.RightShift), keyboardState.NumLock, pressedKeyboardButtons);



            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            MDRPG.Point position = new MDRPG.Point(mouseState.X, mouseState.Y);

            int scrollWheelValue = mouseState.ScrollWheelValue - lastScrollWheelValue;

            bool rightMouseButton = mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            bool leftMouseButton = mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            bool middleMouseButton = mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;

            List<MDRPG.MouseButton> pressedMouseButtons = new List<MDRPG.MouseButton>();

            if (mouseState.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                pressedMouseButtons.Add(MDRPG.MouseButton.Button0);
            }
            if (mouseState.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                pressedMouseButtons.Add(MDRPG.MouseButton.Button1);
            }

            MDRPG.MouseState dmMouseState = new MDRPG.MouseState(position, scrollWheelValue, rightMouseButton, leftMouseButton, middleMouseButton, pressedMouseButtons);

            MDRPG.InputPacket iPacket = new MDRPG.InputPacket(dmKeyboardState, dmMouseState);

            lastScrollWheelValue = scrollWheelValue;
            return iPacket;
        }
        protected override void Initialize()
        {
            spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            base.Initialize();
        }
        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            MDRPG.InputPacket iPacket = CreateInputPacket();

            MDRPG.TickReturnPacket rPacket = dmGame.Tick(new MDRPG.TickInputPacket(iPacket, new MDRPG.Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)));

            if (!(rPacket.exceptions == null) && rPacket.exceptions.Count > 0)
            {
                Process.GetCurrentProcess().Kill();
            }
            epsilonFrameBuffer = rPacket.frameBuffer;
            base.Update(gameTime);
            Console.WriteLine(gameTime.ElapsedGameTime.Ticks);
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!(epsilonFrameBuffer == null))
            {
                if (frameBuffer == null || frameBuffer.Width != epsilonFrameBuffer.width || frameBuffer.Height != epsilonFrameBuffer.height)
                {
                    frameBuffer = new Microsoft.Xna.Framework.Graphics.Texture2D(GraphicsDevice, epsilonFrameBuffer.width, epsilonFrameBuffer.height);
                }
                if (frameColorBuffer == null || frameColorBuffer.Length != epsilonFrameBuffer.width * epsilonFrameBuffer.height)
                {
                    frameColorBuffer = new Microsoft.Xna.Framework.Color[epsilonFrameBuffer.width * epsilonFrameBuffer.height];
                }

                int i = 0;
                for (int y = epsilonFrameBuffer.height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < epsilonFrameBuffer.width; x++)
                    {
                        MDRPG.Color pixelColor = epsilonFrameBuffer.GetPixelUnsafe(x, y);
                        frameColorBuffer[i] = new Microsoft.Xna.Framework.Color(pixelColor.r, pixelColor.g, pixelColor.b, byte.MaxValue);
                        i++;
                    }
                }
                frameBuffer.SetData(frameColorBuffer);
                spriteBatch.Begin(samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
                spriteBatch.Draw(frameBuffer, new Microsoft.Xna.Framework.Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Microsoft.Xna.Framework.Color.White);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            }
            base.Draw(gameTime);
        }
    }
}