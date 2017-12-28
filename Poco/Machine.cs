using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public class Machine : NativeWindow
    {
        public static Machine Create(int width, int height, int scale, string title) {
            return new Machine(width, height, scale, title);
        }

        public Input Input => _Input;
        public Sprite Sprite => _Sprite;
        public Backgrounds Backgrounds => _Backgrounds;


        public Machine(int width, int height, int scale, string title)
            : base(width * scale, height * scale, title, GameWindowFlags.FixedWindow, GraphicsMode.Default, DisplayDevice.Default) {
            float scaleFactor = 1f;
            using (var graphics = Graphics.FromHwnd(WindowInfo.Handle)) {
                scaleFactor = scale * graphics.DpiX / 96f;
            }

            _Context = new GraphicsContext(GraphicsMode.Default, WindowInfo, 1, 0, GraphicsContextFlags.Default);
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            _Input = new Input();
            _Sprite = new Sprite(256, 256);
            _Backgrounds = new Backgrounds(1, 32, 256);
            _Rasterizer = new Rasterizer(ClientSize.Width, ClientSize.Height, scaleFactor);
        }

        public void Execute() {
            EnsureUndisposed();
            if (Exists) {
                _Input.Populate();
                _Rasterizer.Rasterize(_Sprite, _Backgrounds);
                _Context.SwapBuffers();
            }
        }

        public override void Dispose() {
            _Context.Dispose();
            _Sprite.Dispose();
            _Backgrounds.Dispose();
            base.Dispose();
        }

        readonly float _Scale = 1;
        readonly GraphicsContext _Context;
        readonly Input _Input;
        readonly Sprite _Sprite;
        readonly Backgrounds _Backgrounds;
        readonly Rasterizer _Rasterizer;
    }
}
