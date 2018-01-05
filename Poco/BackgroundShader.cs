using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    class BackgroundShader : Shader
    {
        public BackgroundShader(Background background)
            : base("Poco.Shaders.Background.vert", "Poco.Shaders.Background.frag") {
            _Background = background;
            _Projection = GL.GetUniformLocation(Handle, "projection");
            _Texture = GL.GetUniformLocation(Handle, "texture");
            GenerateVertex();
            GenerateCoord();
            GenerateIndex();
        }

        protected override void UpdateImpl() {
            UpdateCoord();
        }

        protected override void RenderImpl(Matrix4 projection) {
            GL.UseProgram(Handle);
            GL.BindTexture(TextureTarget.Texture2D, _Background.VideoRam.Texture);
            GL.UniformMatrix4(_Projection, false, ref projection);
            GL.Uniform1(_Texture, 0);
            GL.EnableVertexAttribArray(_Vertex.Handle);
            GL.EnableVertexAttribArray(_Coord.Handle);
            GL.DrawElements(PrimitiveType.Triangles, _Index.Buffer.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DisableVertexAttribArray(_Vertex.Handle);
            GL.DisableVertexAttribArray(_Coord.Handle);
            GL.UseProgram(0);
        }

        void GenerateVertex() {
            var size = _Background.Size;
            var buffer = new Vector2[size * size * 4];
            for (var y = 0; y < size; ++y) {
                for (var x = 0; x < size; x++) {
                    var index = (x + y * size) * 4;
                    buffer[index + 0] = new Vector2(x * 8, y * 8);
                    buffer[index + 1] = new Vector2(x * 8 + 8, y * 8);
                    buffer[index + 2] = new Vector2(x * 8, y * 8 + 8);
                    buffer[index + 3] = new Vector2(x * 8 + 8, y * 8 + 8);
                }
            }
            _Vertex = new ArrayBuffer<Vector2>(Handle, "vertex", buffer);
            ArrayBuffer.GenerateStatic(_Vertex);
        }

        void GenerateCoord() {
            var size = _Background.Size;
            var buffer = new Vector2[size * size * 4];
            _Coord = new ArrayBuffer<Vector2>(Handle, "coord", buffer);
            ArrayBuffer.GenerateDynamic(_Coord);
        }

        private void UpdateCoord() {
            var stride = _Background.Size;
            var image = _Background.VideoRam.Size;
            var size = new SizeF(8f / image, 8f / image);
            for (var y = 0; y < stride; ++y) {
                for (var x = 0; x < stride; ++x) {
                    var c = _Background[x, y];
                    var location = new PointF((c.No % (image / 8)) * 8f / image, (c.No / (image / 8)) * 8f / image);
                    var uv = new RectangleF(location, size);
                    var index = (x + y * stride) * 4;
                    _Coord[index + 0] = new Vector2(uv.Left, uv.Top);
                    _Coord[index + 1] = new Vector2(uv.Right, uv.Top);
                    _Coord[index + 2] = new Vector2(uv.Left, uv.Bottom);
                    _Coord[index + 3] = new Vector2(uv.Right, uv.Bottom);
                }
            }
            ArrayBuffer.Update(_Coord);
        }

        void GenerateIndex() {
            var size = _Background.Size;
            var buffer = new int[size * size * 6];
            for (var y = 0; y < size; ++y) {
                for (var x = 0; x < size; x++) {
                    var vertex = (x + y * size) * 4;
                    var index = (x + y * size) * 6;
                    buffer[index + 0] = 0 + vertex;
                    buffer[index + 1] = 1 + vertex;
                    buffer[index + 2] = 2 + vertex;
                    buffer[index + 3] = 1 + vertex;
                    buffer[index + 4] = 2 + vertex;
                    buffer[index + 5] = 3 + vertex;
                }
            }
            _Index = new ElementArrayBuffer<int>(buffer);
            ElementArrayBuffer.GenerateStatic(_Index);
        }

        readonly Background _Background;
        int _Projection;
        int _Texture;
        ArrayBuffer<Vector2> _Vertex;
        ArrayBuffer<Vector2> _Coord;
        ElementArrayBuffer<int> _Index;
    }
}
