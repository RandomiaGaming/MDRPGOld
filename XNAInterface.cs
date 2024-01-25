using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MDRPG
{
    public delegate void UpdateCallback();
    public sealed class XNAInterface : Game
    {
        private static Assembly assembly = typeof(XNAInterface).Assembly;

        public readonly int RenderWidth = 512;
        public readonly int RenderHeight = 288;

        public int CameraX = 0;
        public int CameraY = 0;

        public bool ProfilerEnabled = false;

        public GraphicsDeviceManager XNAGraphicsDeviceManager = null;
        public SpriteBatch XNASpriteBatch = null;
        public RenderTarget2D XNARenderTarget = null;

        private UpdateCallback _updateCallback;
        private Stopwatch _gameTimer;
        private long _ticksLastFrame;
        public XNAInterface(UpdateCallback updateCallback)
        {
            _updateCallback = updateCallback;

            //this.InactiveSleepTime = default;
            this.TargetElapsedTime = new System.TimeSpan(10000000 / 120);
            this.MaxElapsedTime = new System.TimeSpan(10000000 / 120);
            this.IsFixedTimeStep = true;
            this.IsMouseVisible = true;

            XNAGraphicsDeviceManager = new GraphicsDeviceManager(this);
            XNAGraphicsDeviceManager.GraphicsProfile = GraphicsProfile.Reach;
            XNAGraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            XNAGraphicsDeviceManager.HardwareModeSwitch = false;
            XNAGraphicsDeviceManager.IsFullScreen = false;
            XNAGraphicsDeviceManager.PreferHalfPixelOffset = false;
            XNAGraphicsDeviceManager.PreferredBackBufferFormat = SurfaceFormat.Color;
            XNAGraphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight | DisplayOrientation.Portrait | DisplayOrientation.PortraitDown | DisplayOrientation.Unknown | DisplayOrientation.Default;
            //GraphicsDeviceManager.PreferredBackBufferWidth = default;
            //GraphicsDeviceManager.PreferredBackBufferHeight = default;
            XNAGraphicsDeviceManager.ApplyChanges();

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.BlendFactor = Color.White;
            GraphicsDevice.Indices = default;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.ResourcesLost = false;
            //GraphicsDevice.GraphicsDebug = default;
            //GraphicsDevice.Metrics = default;
            //GraphicsDevice.ScissorRectangle = default;
            //GraphicsDevice.Viewport = default;

            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            Window.IsBorderless = false;
            Window.Position = new Point(GraphicsDevice.Adapter.CurrentDisplayMode.Width / 4, GraphicsDevice.Adapter.CurrentDisplayMode.Height / 4);
            Window.Title = "MDRPG - 0.1";

            XNASpriteBatch = new SpriteBatch(GraphicsDevice /*, default*/);
            XNASpriteBatch.Name = "MDRPG Main Sprite Batch";
            XNASpriteBatch.Tag = null;

            XNARenderTarget = new RenderTarget2D(GraphicsDevice, RenderWidth, RenderHeight, false, SurfaceFormat.Color, DepthFormat.None);
            XNARenderTarget.Name = "MDRPG Main Render Target";
            XNARenderTarget.Tag = null;

            _gameTimer = new Stopwatch();
            _gameTimer.Start();
        }

        private bool _F11LastFrame; //Fullscreen
        private bool _F1LastFrame; //Save Screenshot
        private bool _F2LastFrame; //Toggle Profiler
        protected sealed override void Draw(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            bool F11 = keyboardState.IsKeyDown(Keys.F11);
            bool F2 = keyboardState.IsKeyDown(Keys.F2);
            bool F1 = keyboardState.IsKeyDown(Keys.F1);
            if (F11 && !_F11LastFrame)
            {
                XNAGraphicsDeviceManager.ToggleFullScreen();
            }
            if (F2 && !_F2LastFrame)
            {
                ProfilerEnabled = !ProfilerEnabled;
            }

            long ticksNow = _gameTimer.ElapsedTicks;
            long TPF = ticksNow - _ticksLastFrame;
            double FPS = 10000000.0 / TPF;
            if (ProfilerEnabled)
            {
                Console.WriteLine($"TPF {TPF} FPS {FPS}");
            }
            _ticksLastFrame = ticksNow;

            GraphicsDevice.SetRenderTarget(XNARenderTarget);

            XNASpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);
            _updateCallback.Invoke();
            XNASpriteBatch.End();

            if (F1 && !_F1LastFrame)
            {
                string screenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Screenshot.png";
                SavePNG(XNARenderTarget, screenshotPath);
                Console.WriteLine($"Screenshot saved to \"{screenshotPath}\".");
            }

            GraphicsDevice.SetRenderTarget(null);

            XNASpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);
            XNASpriteBatch.Draw(XNARenderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Rectangle(0, 0, RenderWidth, RenderHeight), Color.White);
            XNASpriteBatch.End();

            _F11LastFrame = F11;
            _F2LastFrame = F2;
            _F1LastFrame = F1;
        }
        public void SavePNG(RenderTarget2D target, string path)
        {
            FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            target.SaveAsPng(fileStream, target.Width, target.Height);
            fileStream.Close();
            fileStream.Dispose();
        }
        public void SavePNG(Texture2D target, string path)
        {
            FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            target.SaveAsPng(fileStream, target.Width, target.Height);
            fileStream.Close();
            fileStream.Dispose();
        }
        public void DrawSprite(Texture2D sprite, int xWorld, int yWorld)
        {
            int xScreen = xWorld - CameraX;
            int yScreen = yWorld - CameraY;
            if (xScreen < RenderWidth && yScreen < RenderHeight && xScreen + sprite.Width >= 0 && yScreen + sprite.Height >= 0)
            {
                int yScreenInverted = RenderHeight - (yScreen + sprite.Height);
                XNASpriteBatch.Draw(sprite, new Vector2(xScreen, yScreenInverted), Color.White);
            }
        }
        public Texture2D LoadTexture(string embeddedResourceName)
        {
            Stream embeddedResourceStream = assembly.GetManifestResourceStream(embeddedResourceName);
            Texture2D output = Texture2D.FromStream(GraphicsDevice, embeddedResourceStream);
            embeddedResourceStream.Dispose();
            return output;
        }
    }
}