using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;

namespace Poco
{
    public class Machine : NativeWindow
    {
        public static Machine Create(string title) {
            return new Machine(title, Settings.Default);
        }
        public static Machine Create(string title, Settings settings) {
            return new Machine(title, settings);
        }

        public Input Input => _Input;
        public Background[] Background => _Background;
        public Sprite Sprite => _Sprite;

        protected Machine(string title, Settings settings)
            : base((int)(settings.Screen.Width * settings.Screen.Scale),
                  (int)(settings.Screen.Height * settings.Screen.Scale),
                  title, GameWindowFlags.FixedWindow, GraphicsMode.Default, DisplayDevice.Default) {
            var scaleFactor = 1f;
            using (var graphics = Graphics.FromHwnd(WindowInfo.Handle)) {
                scaleFactor = settings.Screen.Scale * graphics.DpiX / 96f;
            }

            _Context = new GraphicsContext(GraphicsMode.Default, WindowInfo, 3, 0, GraphicsContextFlags.Default);
            _Context.SwapInterval = -1;
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            _Input = new Input();
            _Background = new Background[settings.Background.Layer];
            for (var i = 0; i < _Background.Length; ++i) {
                _Background[i] = new Background(settings.Background);
            }
            _Sprite = new Sprite(settings.Sprite);
            _Rasterizer = new Rasterizer(ClientSize.Width, ClientSize.Height, scaleFactor, _Background, _Sprite);
        }

        public void Update() {
            EnsureUndisposed();
            if (Exists) {
                _Input.Populate();
                _Rasterizer.Rasterize();
                _Context.SwapBuffers();
            }
        }

        public override void Dispose() {
            _Context.Dispose();
            Array.ForEach(_Background, background => background.Dispose());
            _Sprite.Dispose();
            _Rasterizer.Dispose();
            base.Dispose();
        }

        readonly GraphicsContext _Context;
        readonly Input _Input;
        readonly Background[] _Background;
        readonly Sprite _Sprite;
        readonly Rasterizer _Rasterizer;
    }
}
