using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Poco.Shaders;

namespace Poco
{
    public class Rasterizer
    {
        public Rasterizer(int width, int height, float scale, Background[] background, Sprite sprite) {
            _Width = width;
            _Height = height;
            _Scale = scale;
            GL.Viewport(0, 0, _Width, _Height);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.ClearColor(Color4.Black);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Equal, 1f);
            _BackgroundShader = new BackgroundShader[background.Length];
            for (var i = 0; i < _BackgroundShader.Length; ++i) {
                _BackgroundShader[i] = new BackgroundShader(background[i]);
            }
            _SpriteShader = new SpriteShader(sprite);
        }

        public void Rasterize() {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _Width, _Height, 0, -1, 1);
            GL.Scale(_Scale, _Scale, 1f);
            GL.GetFloat(GetPName.ProjectionMatrix, out Matrix4 projection);

            foreach (var shader in _BackgroundShader) {
                shader.Render(projection);
            }
            _SpriteShader.Render(projection);
        }

        readonly int _Width;
        readonly int _Height;
        readonly float _Scale;
        readonly BackgroundShader[] _BackgroundShader;
        readonly SpriteShader _SpriteShader;
    }
}
