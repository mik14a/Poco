using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public class Rasterizer
    {
        public Rasterizer(int width, int height, float scale) {
            _Width = width;
            _Height = height;
            _Scale = scale;
            GL.Viewport(0, 0, _Width, _Height);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
        }

        public void Rasterize(Sprite sprite, Backgrounds backgrounds) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _Width, _Height, 0, -1, 1);
            GL.Scale(_Scale, _Scale, 1f);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            GL.Enable(EnableCap.Texture2D);

            foreach (var attribute in sprite) {
            }

            foreach(var background in backgrounds) {
                GL.BindTexture(TextureTarget.Texture2D, background.VideoRam.Texture);
                var size = background.VideoRam.Size;
                for (var y = 0; y < background.Size; ++y) {
                    for (var x = 0; x < background.Size; ++x) {
                        var c = background[x, y];
                        var uv = new RectangleF(
                            new PointF((c.No % (size / 8)) * 8f / size, (c.No / (size / 8)) * 8f / size),
                            new SizeF(8f / size, 8f / size)
                        );
                        var xy = new RectangleF(
                            new PointF(x * 8f, y * 8f),
                            new SizeF(8f, 8f)
                        );
                        var z = background.Priority;
                        GL.Begin(PrimitiveType.TriangleStrip);
                        GL.TexCoord3(uv.Left, uv.Top, 0f); GL.Vertex3(xy.Left, xy.Top, z);
                        GL.TexCoord3(uv.Right, uv.Top, 0f); GL.Vertex3(xy.Right, xy.Top, z);
                        GL.TexCoord3(uv.Left, uv.Bottom, 0f); GL.Vertex3(xy.Left, xy.Bottom, z);
                        GL.TexCoord3(uv.Right, uv.Bottom, 0f); GL.Vertex3(xy.Right, xy.Bottom, z);
                        GL.End();
                    }
                }
            }
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        int _Width;
        int _Height;
        float _Scale;
    }
}
