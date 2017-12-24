using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
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
            using (var graphics = Graphics.FromHwnd(WindowInfo.Handle)) {
                _Scale = scale * graphics.DpiX / 96f;
            }

            _Context = new GraphicsContext(GraphicsMode.Default, WindowInfo, 1, 0, GraphicsContextFlags.Default);
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);

            _Input = new Input();
            _Sprite = new Sprite(256);
            _Backgrounds = new Backgrounds(1, 32, 256);
        }

        public void Execute() {
            EnsureUndisposed();
            if (Exists) {
                _Input.Populate();
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, ClientSize.Width, ClientSize.Height, 0, -1, 1);
                GL.Scale(_Scale, _Scale, 1f);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                _Sprite.Draw();
                _Backgrounds.Draw();
                GL.PopMatrix();
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
    }
}
