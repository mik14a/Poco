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
        public Rasterizer(int width, int height, float scale, Backgrounds backgrounds, Sprite sprite) {
            _Width = width;
            _Height = height;
            _Scale = scale;
            GL.Viewport(0, 0, _Width, _Height);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.ClearColor(Color4.Black);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Equal, 1f);
            _BackgroundShader = new BackgroundShader[backgrounds.Count];
            for (var i = 0; i < _BackgroundShader.Length; ++i) {
                _BackgroundShader[i] = new BackgroundShader(backgrounds[i]);
            }
            _SpriteShader = new SpriteShader(sprite);
        }

        public void Rasterize(Backgrounds backgrounds, Sprite sprite) {
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

        int _Width;
        int _Height;
        float _Scale;
        BackgroundShader[] _BackgroundShader;
        SpriteShader _SpriteShader;
    }
}
