using System;
using System.Drawing;
using System.Linq;
//using OpenTK;
//using OpenTK.Graphics;

namespace Poco
{
    public class Machine : OpenTK.NativeWindow
    {
        public static Machine Create(string title) {
            return new Machine(title, Settings.Default);
        }
        public static Machine Create(string title, Settings settings) {
            return new Machine(title, settings);
        }

        public Input Input { get; }

        public Background[] Background { get; }

        public Sprite Sprite { get; }

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

            _Context = new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, WindowInfo, 3, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
            _Context.SwapInterval = -1;
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            Input = new Input();
            Background = new Background[settings.Background.Layer];
            for (var i = 0; i < Background.Length; ++i) {
                Background[i] = new Background(settings.Background);
            }
            Sprite = new Sprite(settings.Sprite);
            _Rasterizer = new Rasterizer(ClientSize.Width, ClientSize.Height, scaleFactor, Background, Sprite);
        }

        public void Update() {
            EnsureUndisposed();
            if (Exists) {
                Input.Populate();
                _Rasterizer.Rasterize();
                _Context.SwapBuffers();
            }
        }

        public override void Dispose() {
            _Context.Dispose();
            Array.ForEach(Background, background => background.Dispose());
            Sprite.Dispose();
            _Rasterizer.Dispose();
            base.Dispose();
        }

        readonly OpenTK.Graphics.GraphicsContext _Context;
        readonly Rasterizer _Rasterizer;
    }
}
