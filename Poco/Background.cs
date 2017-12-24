using System;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public partial class Background : IDisposable
    {

        public int Size => _Size;

        public VideoRam VideoRam => _VideoRam;

        public Character this[int x, int y] {
            get { return _Map[x, y]; }
            set { _Map[x, y] = value; }
        }

        public struct Character
        {
            [Flags] public enum Rotates { Rotate0 = 0x00, Rotate90 = 0x01, Rotate180 = 0x10, Rotate270 = 0x11, RotateNone = 0 }
            [Flags] public enum Flips { FlipHorizontal = 0x01, FlipVertical = 0x10, FlipNone = 0 }

            public int No;
            public Rotates Rotate;
            public Flips Flip;
        }

        public Background(int mapSize, int videoRamSize) {
            _Size = mapSize;
            _Map = new Character[_Size, _Size];
            _VideoRam = new VideoRam(videoRamSize);
        }

        public void Draw() {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _VideoRam.Texture);
            var size = _VideoRam.Size;
            for (var y = 0; y < _Size; ++y) {
                for (var x = 0; x < _Size; ++x) {
                    var c = _Map[x, y];
                    var uv = new RectangleF(
                        new PointF((c.No % (size / 8)) * 8f / size, (c.No / (size/ 8)) * 8f / size),
                        new SizeF(8f / size, 8f / size)
                    );
                    var xy = new RectangleF(
                        new PointF(x * 8f, y * 8f),
                        new SizeF(8f, 8f)
                    );
                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.TexCoord3(uv.Left, uv.Top, 0f); GL.Vertex3(xy.Left, xy.Top, 0f);
                    GL.TexCoord3(uv.Right, uv.Top, 0f); GL.Vertex3(xy.Right, xy.Top, 0f);
                    GL.TexCoord3(uv.Left, uv.Bottom, 0f); GL.Vertex3(xy.Left, xy.Bottom, 0f);
                    GL.TexCoord3(uv.Right, uv.Bottom, 0f); GL.Vertex3(xy.Right, xy.Bottom, 0f);
                    GL.End();
                }
            }
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        public void Dispose() {
            _VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }


        readonly int _Size;
        readonly Character[,] _Map;
        readonly VideoRam _VideoRam;
    }
}
