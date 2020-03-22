using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    class BackgroundShader : Shader
    {
        public BackgroundShader(Background.Plane background)
            : base("Poco.Shaders.Background.vert", "Poco.Shaders.Background.frag") {
            _Plane = background;
            _Projection = GL.GetUniformLocation(Handle, "projection");
            _ModelView = GL.GetUniformLocation(Handle, "modelview");
            _Texture = GL.GetUniformLocation(Handle, "texture");
            _VertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_VertexArray);
            GenerateVertex();
            GenerateCoord();
            GenerateIndex();
            GL.BindVertexArray(0);
        }

        protected override void RenderImpl(Matrix4 projection) {
            UpdateCoord();
            GL.UseProgram(Handle);
            GL.BindVertexArray(_VertexArray);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _Plane.VideoRam.Texture);
            GL.BindSampler(0, _Plane.VideoRam.Sampler);
            GL.UniformMatrix4(_Projection, false, ref projection);
            GL.Uniform1(_Texture, 0);
            GL.DrawElements(PrimitiveType.Triangles, _Index.Buffer.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.UseProgram(0);
            GL.BindVertexArray(0);
        }

        void GenerateVertex() {
            var size = _Plane.Size;
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
            _Vertex.GenerateStatic();
        }

        void GenerateCoord() {
            var size = _Plane.Size;
            var buffer = new Vector2[size * size * 4];
            _Coord = new ArrayBuffer<Vector2>(Handle, "coord", buffer);
            _Coord.GenerateDynamic();
        }

        void UpdateCoord() {
            var stride = _Plane.Size;
            var image = _Plane.VideoRam.Size;
            var size = new SizeF(8f / image, 8f / image);
            for (var y = 0; y < stride; ++y) {
                for (var x = 0; x < stride; ++x) {
                    var c = _Plane[x, y];
                    var u = c.No % (image / 8);
                    var v = c.No / (image / 8);
                    var location = new PointF(u * 8f / image, v * 8f / image);
                    var uv = new RectangleF(location, size);
                    var index = (x + y * stride) * 4;
                    _Coord[index + 0] = new Vector2(uv.Left, uv.Top);
                    _Coord[index + 1] = new Vector2(uv.Right, uv.Top);
                    _Coord[index + 2] = new Vector2(uv.Left, uv.Bottom);
                    _Coord[index + 3] = new Vector2(uv.Right, uv.Bottom);
                }
            }
            _Coord.Update();
        }

        void GenerateIndex() {
            var size = _Plane.Size;
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
            _Index.GenerateStatic();
        }

        readonly Background.Plane _Plane;
        readonly int _Projection;
        readonly int _ModelView;
        readonly int _Texture;
        readonly int _VertexArray;
        ArrayBuffer<Vector2> _Vertex;
        ArrayBuffer<Vector2> _Coord;
        ElementArrayBuffer<int> _Index;
    }
}
