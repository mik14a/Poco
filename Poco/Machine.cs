using System;
using System.Drawing;
using System.Linq;
using Poco.Internals;

namespace Poco
{
    /// <summary>
    /// Poco Machine. Poco machine have input device, background and sprite memory.
    /// </summary>
    public class Machine : OpenTK.NativeWindow
    {
        /// <summary>
        /// Create Poco machine with window title.
        /// </summary>
        /// <param name="title">Title text</param>
        /// <returns>New Poco machine instance use default settings</returns>
        public static Machine Create(string title) {
            return new Machine(title, Settings.Default);
        }

        /// <summary>
        /// Create Poco machine with window title and machine settings.
        /// </summary>
        /// <param name="title">Title text</param>
        /// <param name="settings">Machine settings</param>
        /// <returns>New Poco machine instance</returns>
        public static Machine Create(string title, Settings settings) {
            return new Machine(title, settings);
        }

        /// <summary>
        /// Input device.
        /// </summary>
        public Input Inputs { get; }

        /// <summary>
        /// Background memory.
        /// </summary>
        public Background Backgrounds { get; }

        /// <summary>
        /// Sprite memory.
        /// </summary>
        public Sprite Sprites { get; }

        /// <summary>
        /// Construct Poco machine.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="settings">Machine settings</param>
        protected Machine(string title, Settings settings)
            : base((int)(settings.Screen.Width * settings.Screen.Scale),
                   (int)(settings.Screen.Height * settings.Screen.Scale),
                   title,
                   OpenTK.GameWindowFlags.FixedWindow,
                   OpenTK.Graphics.GraphicsMode.Default,
                   OpenTK.DisplayDevice.Default) {
            var scaleFactor = 1f;
            using (var graphics = Graphics.FromHwnd(WindowInfo.Handle)) {
                scaleFactor = settings.Screen.Scale * graphics.DpiX / 96f;
            }

            // Create and initialize OpenTK context.
            _Context = new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, WindowInfo, 3, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
            _Context.SwapInterval = 0;
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            // Create virtual device and memory instance.
            Inputs = Input.Create();
            Backgrounds = Background.Create(settings.Background);
            Sprites = Sprite.Create(settings.Sprite);

            // Create rasterizer.
            _Rasterizer = new Rasterizer(ClientSize.Width, ClientSize.Height, scaleFactor, Backgrounds, Sprites);
        }

        /// <summary>
        /// Update display.
        /// </summary>
        public void Update() {
            EnsureUndisposed();
            if (Exists) {
                Inputs.Populate();
                _Rasterizer.Rasterize();
                _Context.SwapBuffers();
            }
        }

        /// <summary>
        /// Dispose Poco machine.
        /// </summary>
        public override void Dispose() {
            _Context.Dispose();
            Backgrounds.Dispose();
            Sprites.Dispose();
            _Rasterizer.Dispose();
            base.Dispose();
        }

        /// <summary>OpenTK graphics context.</summary>
        readonly OpenTK.Graphics.GraphicsContext _Context;
        /// <summary>Video memory rasterizer</summary>
        readonly Rasterizer _Rasterizer;
    }
}
