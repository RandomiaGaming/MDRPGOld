using System.IO;

namespace MDRPG
{
    public delegate void UpdateCallback();
    public sealed class XNAInterface
    {
        #region Public Variables
        public readonly int RenderWidth;
        public readonly int RenderHeight;

        public bool ShowDebugInfo = false;

        public int CameraX = 0;
        public int CameraY = 0;

        public readonly Microsoft.Xna.Framework.Game XNAGame = null;
        public readonly Microsoft.Xna.Framework.GraphicsDeviceManager XNAGraphicsDeviceManager = null;
        public readonly Microsoft.Xna.Framework.Graphics.GraphicsDevice XNAGraphicsDevice = null;
        public readonly Microsoft.Xna.Framework.GameWindow XNAWindow = null;
        public readonly Microsoft.Xna.Framework.Graphics.SpriteBatch XNASpriteBatch = null;
        public readonly Microsoft.Xna.Framework.Graphics.RenderTarget2D XNARenderTarget = null;
        #endregion
        #region Private Variables
        private UpdateCallback _updateCallback;

        //Profiler
        private System.Diagnostics.Stopwatch _gameTimer;
        private long _frameCount;
        private long _ticksLastLog;

        //Default Actions
        private bool _F2LastFrame; //Take Screenshot
        private bool _F3LastFrame; //Toggle Debug Info
        private bool _F11LastFrame; //Toggle Fullscreen
        #endregion
        #region Public Constructors
        public XNAInterface(UpdateCallback updateCallback, int renderWidth = 512, int renderHeight = 288)
        {
            if (updateCallback is null)
            {
                throw new System.Exception("updateCallback cannot be null.");
            }
            _updateCallback = updateCallback;
            if (renderHeight <= 0)
            {
                throw new System.Exception("renderWidth must be greater than 0.");
            }
            RenderHeight = renderHeight;
            if (renderWidth <= 0)
            {
                throw new System.Exception("renderHeight must be greater than 0.");
            }
            RenderWidth = renderWidth;

            XNAGame = new GameWithCallback() { _callback = Update };
            //XNAGame.InactiveSleepTime = default;
            XNAGame.TargetElapsedTime = new System.TimeSpan(10000000 / 120);
            XNAGame.MaxElapsedTime = new System.TimeSpan(10000000 / 120);
            XNAGame.IsFixedTimeStep = true;
            XNAGame.IsMouseVisible = true;

            XNAGraphicsDeviceManager = new Microsoft.Xna.Framework.GraphicsDeviceManager(XNAGame);
            XNAGraphicsDeviceManager.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach;
            XNAGraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            XNAGraphicsDeviceManager.HardwareModeSwitch = false;
            XNAGraphicsDeviceManager.IsFullScreen = false;
            XNAGraphicsDeviceManager.PreferHalfPixelOffset = false;
            XNAGraphicsDeviceManager.PreferredBackBufferFormat = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color;
            XNAGraphicsDeviceManager.SupportedOrientations = Microsoft.Xna.Framework.DisplayOrientation.LandscapeLeft | Microsoft.Xna.Framework.DisplayOrientation.LandscapeRight | Microsoft.Xna.Framework.DisplayOrientation.Portrait | Microsoft.Xna.Framework.DisplayOrientation.PortraitDown | Microsoft.Xna.Framework.DisplayOrientation.Unknown | Microsoft.Xna.Framework.DisplayOrientation.Default;
            //XNAGraphicsDeviceManager.PreferredBackBufferWidth = default;
            //XNAGraphicsDeviceManager.PreferredBackBufferHeight = default;
            XNAGraphicsDeviceManager.ApplyChanges();

            XNAGraphicsDevice = XNAGame.GraphicsDevice;
            XNAGraphicsDevice.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend;
            XNAGraphicsDevice.DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState.None;
            XNAGraphicsDevice.RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone;
            XNAGraphicsDevice.BlendFactor = Microsoft.Xna.Framework.Color.White;
            //XNAGraphicsDevice.Indices = default;
            XNAGraphicsDevice.RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone;
            XNAGraphicsDevice.ResourcesLost = false;
            //GraphicsDevice.GraphicsDebug = default;
            //GraphicsDevice.Metrics = default;
            //GraphicsDevice.ScissorRectangle = default;
            //GraphicsDevice.Viewport = default;

            XNAWindow = XNAGame.Window;
            XNAWindow.AllowAltF4 = true;
            XNAWindow.AllowUserResizing = true;
            XNAWindow.IsBorderless = false;
            XNAWindow.Position = new Microsoft.Xna.Framework.Point(XNAGraphicsDevice.Adapter.CurrentDisplayMode.Width / 4, XNAGraphicsDevice.Adapter.CurrentDisplayMode.Height / 4);
            XNAWindow.Title = "Unamed Game";

            XNASpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(XNAGraphicsDevice /*, default*/);
            XNASpriteBatch.Name = "Main Sprite Batch";
            XNASpriteBatch.Tag = null;

            XNARenderTarget = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(XNAGraphicsDevice, RenderWidth, RenderHeight, false, Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color, Microsoft.Xna.Framework.Graphics.DepthFormat.None);
            XNARenderTarget.Name = "Main Render Target";
            XNARenderTarget.Tag = null;

            _gameTimer = new System.Diagnostics.Stopwatch();
            _gameTimer.Start();
        }
        #endregion
        #region Private Methods
        private void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Microsoft.Xna.Framework.Input.KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            bool F2 = keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2);
            bool F3 = keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F3);
            bool F11 = keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F11);
            if (F11 && !_F11LastFrame)
            {
                XNAGraphicsDeviceManager.ToggleFullScreen();
            }
            if (F3 && !_F3LastFrame)
            {
                ShowDebugInfo = !ShowDebugInfo;
            }

            long ticksNow = _gameTimer.ElapsedTicks;
            long deltaTicks = ticksNow - _ticksLastLog;
            if (ShowDebugInfo && deltaTicks > 10000000)
            {
                long TPF = deltaTicks / _frameCount;
                long FPS = _frameCount * 10000000 / deltaTicks;

                System.Console.Clear();
                System.Console.WriteLine($"Debug Info - {XNAWindow.Title}");
                System.Console.WriteLine($"Preformance: {TPF}tpf {FPS}fps");
                System.Console.WriteLine();

                _ticksLastLog = ticksNow;
                _frameCount = 0;
            }

            XNAGraphicsDevice.SetRenderTarget(XNARenderTarget);

            XNASpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate, Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, Microsoft.Xna.Framework.Graphics.DepthStencilState.None, Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone, null, null);
            _updateCallback.Invoke();
            XNASpriteBatch.End();

            if (F2 && !_F2LastFrame)
            {
                SaveScreenshot();
            }

            XNAGraphicsDevice.SetRenderTarget(null);

            XNASpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate, Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, Microsoft.Xna.Framework.Graphics.DepthStencilState.None, Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone, null, null);
            XNASpriteBatch.Draw(XNARenderTarget, new Microsoft.Xna.Framework.Rectangle(0, 0, XNAGraphicsDevice.Viewport.Width, XNAGraphicsDevice.Viewport.Height), new Microsoft.Xna.Framework.Rectangle(0, 0, RenderWidth, RenderHeight), Microsoft.Xna.Framework.Color.White);
            XNASpriteBatch.End();

            _F2LastFrame = F2;
            _F3LastFrame = F3;
            _F11LastFrame = F11;

            _frameCount++;
        }
        #endregion
        #region Public Methods
        public void Run()
        {
            XNAGame.Run();
        }
        public void SavePNG(Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget, string filePath)
        {
            if (renderTarget is null)
            {
                throw new System.Exception("renderTarget cannot be null.");
            }
            System.IO.FileStream fileStream = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
            renderTarget.SaveAsPng(fileStream, renderTarget.Width, renderTarget.Height);
            fileStream.Close();
            fileStream.Dispose();
        }
        public void SavePNG(Microsoft.Xna.Framework.Graphics.Texture2D texture, string filePath)
        {
            if (texture is null)
            {
                throw new System.Exception("texture cannot be null.");
            }
            System.IO.FileStream fileStream = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
            texture.SaveAsPng(fileStream, texture.Width, texture.Height);
            fileStream.Close();
            fileStream.Dispose();
        }
        public void SaveJPG(Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget, string filePath)
        {
            if (renderTarget is null)
            {
                throw new System.Exception("renderTarget cannot be null.");
            }
            System.IO.FileStream fileStream = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
            renderTarget.SaveAsJpeg(fileStream, renderTarget.Width, renderTarget.Height);
            fileStream.Close();
            fileStream.Dispose();
        }
        public void SaveJPG(Microsoft.Xna.Framework.Graphics.Texture2D texture, string filePath)
        {
            if (texture is null)
            {
                throw new System.Exception("texture cannot be null.");
            }
            System.IO.FileStream fileStream = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
            texture.SaveAsJpeg(fileStream, texture.Width, texture.Height);
            fileStream.Close();
            fileStream.Dispose();
        }
        public void DrawTexture(Microsoft.Xna.Framework.Graphics.Texture2D texture, int xWorld, int yWorld)
        {
            int xScreen = xWorld - CameraX;
            int yScreen = yWorld - CameraY;
            if (xScreen < RenderWidth && yScreen < RenderHeight && xScreen + texture.Width >= 0 && yScreen + texture.Height >= 0)
            {
                int yScreenInverted = RenderHeight - (yScreen + texture.Height);
                XNASpriteBatch.Draw(texture, new Microsoft.Xna.Framework.Vector2(xScreen, yScreenInverted), Microsoft.Xna.Framework.Color.White);
            }
        }
        public void DrawTexture(Microsoft.Xna.Framework.Graphics.Texture2D texture, int xWorld, int yWorld, int xAtlas, int yAtlas, int Width, int Height)
        {
            int xScreen = xWorld - CameraX;
            int yScreen = yWorld - CameraY;
            if (xScreen < RenderWidth && yScreen < RenderHeight && xScreen + Width >= 0 && yScreen + Height >= 0)
            {
                int yScreenInverted = RenderHeight - (yScreen + texture.Height);
                XNASpriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(xScreen, yScreenInverted, Width, Height), new Microsoft.Xna.Framework.Rectangle(xAtlas, yAtlas, Width, Height), Microsoft.Xna.Framework.Color.White);
            }
        }
        public void SaveScreenshot()
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            int index = 0;
            string screenshotPath = $"{desktopPath}\\Screenshot {index}.png";
            while (File.Exists(screenshotPath))
            {
                index++;
                screenshotPath = $"{desktopPath}\\Screenshot {index}.png";
            }
            SavePNG(XNARenderTarget, screenshotPath);
            System.Console.WriteLine($"Screenshot saved to \"{screenshotPath}\".");
        }
        public Microsoft.Xna.Framework.Graphics.Texture2D LoadTexture(string assetName)
        {
            System.Type type = typeof(XNAInterface);
            System.Reflection.Assembly assembly = type.Assembly;
            string[] resourceNames = assembly.GetManifestResourceNames();
            foreach (string resourceName in resourceNames)
            {
                if (resourceName.EndsWith(assetName))
                {
                    System.IO.Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
                    Microsoft.Xna.Framework.Graphics.Texture2D output = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(XNAGraphicsDevice, resourceStream);
                    resourceStream.Dispose();
                    return output;
                }
            }
            throw new System.Exception($"No embedded resource could be found with name \"{assetName}\".");
        }
        #endregion
        #region Private Sub Classes
        private sealed class GameWithCallback : Microsoft.Xna.Framework.Game
        {
            public delegate void GameCallback(Microsoft.Xna.Framework.GameTime gameTime);
            public GameCallback _callback;
            protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
            {
                _callback.Invoke(gameTime);
            }
            protected override void OnExiting(object sender, System.EventArgs args)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        #endregion
    }
}