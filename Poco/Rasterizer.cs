using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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
            //_BackgroundShader = new BackgroundShader[backgrounds.Count];
            //for (int i = 0; i < _BackgroundShader.Length; ++i) {
            //    _BackgroundShader[i] = new BackgroundShader(backgrounds[i]);
            //}
            _Shader = new BackgroundShader(backgrounds[0]);
        }

        public void Rasterize(Backgrounds backgrounds, Sprite sprite) {

            _Shader.Update();

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _Width, _Height, 0, -1, 64);
            GL.Scale(_Scale, _Scale, 1f);
            GL.GetFloat(GetPName.ProjectionMatrix, out Matrix4 projection);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.PushMatrix();

            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, sprite.VideoRam.Texture);
            foreach (var a in sprite) {
                var index = a.Name;
                var size = sprite.VideoRam.Size;
                for (int y = 0; y < a.Size.Height; ++y) {
                    for (int x = 0; x < a.Size.Width; ++x) {
                        var uv = new RectangleF(
                            new PointF(index % (size / 8) * 8f / size, index / (size / 8) * 8f / size),
                            new SizeF(8f / size, 8f / size)
                        );
                        var xy = new RectangleF(
                            new PointF(a.X + x * 8f, a.Y + y * 8f),
                            new SizeF(8f, 8f)
                        );
                        var z = a.Priority;
                        GL.Begin(PrimitiveType.TriangleStrip);
                        GL.TexCoord3(uv.Left, uv.Top, 0f); GL.Vertex3(xy.Left, xy.Top, z);
                        GL.TexCoord3(uv.Right, uv.Top, 0f); GL.Vertex3(xy.Right, xy.Top, z);
                        GL.TexCoord3(uv.Left, uv.Bottom, 0f); GL.Vertex3(xy.Left, xy.Bottom, z);
                        GL.TexCoord3(uv.Right, uv.Bottom, 0f); GL.Vertex3(xy.Right, xy.Bottom, z);
                        GL.End();
                        ++index;
                    }
                }
            }

            //_BackgroundShader.ForEach(s => s.Render(projection));
            //_BackgroundShader[0].Render(projection);
            _Shader.Render(projection);

            /*foreach (var background in backgrounds)*/
            //{
            //    var background = backgrounds[0];
            //    GL.BindTexture(TextureTarget.Texture2D, background.VideoRam.Texture);
            //    var size = background.VideoRam.Size;
            //    for (var y = 0; y < background.Size; ++y) {
            //        for (var x = 0; x < background.Size; ++x) {
            //            var c = background[x, y];
            //            if (c.No == 0) continue;
            //            var uv = new RectangleF(
            //                new PointF((c.No % (size / 8)) * 8f / size, (c.No / (size / 8)) * 8f / size),
            //                new SizeF(8f / size, 8f / size)
            //            );
            //            var xy = new RectangleF(
            //                new PointF(x * 8f, y * 8f),
            //                new SizeF(8f, 8f)
            //            );
            //            var z = background.Priority;
            //            GL.Begin(PrimitiveType.TriangleStrip);
            //            GL.TexCoord3(uv.Left, uv.Top, 0f); GL.Vertex3(xy.Left, xy.Top, z);
            //            GL.TexCoord3(uv.Right, uv.Top, 0f); GL.Vertex3(xy.Right, xy.Top, z);
            //            GL.TexCoord3(uv.Left, uv.Bottom, 0f); GL.Vertex3(xy.Left, xy.Bottom, z);
            //            GL.TexCoord3(uv.Right, uv.Bottom, 0f); GL.Vertex3(xy.Right, xy.Bottom, z);
            //            GL.End();
            //        }
            //    }
            //}

            GL.Disable(EnableCap.Texture2D);

            GL.PopMatrix();
        }

        int _Width;
        int _Height;
        float _Scale;
        BackgroundShader _Shader;
        BackgroundShader[] _BackgroundShader;
    }
}
