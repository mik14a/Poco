using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    class SpriteShader : Shader
    {
        public SpriteShader(Sprite sprite)
            : base("Poco.Shaders.Sprite.vert", "Poco.Shaders.Sprite.frag") {
            _Sprite = sprite;
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
            GL.UseProgram(Handle);
            GL.BindVertexArray(_VertexArray);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _Sprite.VideoRam.Texture);
            GL.BindSampler(0, _Sprite.VideoRam.Sampler);
            GL.UniformMatrix4(_Projection, false, ref projection);
            GL.Uniform1(_Texture, 0);
            foreach (var o in _Sprite) {
                if (!o.Enable) continue;
                UpdateCoord(o);
                UpdateIndex(o);
                var modelView = Matrix4.CreateTranslation(o.X, o.Y, o.Priority);
                GL.UniformMatrix4(_ModelView, false, ref modelView);
                var count = o.Size.Width * o.Size.Height * 6;
                GL.DrawElements(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
            GL.UseProgram(0);
            GL.BindVertexArray(0);
        }

        void GenerateVertex() {
            var buffer = new Vector2[Size * Size * 4];
            for (var y = 0; y < Size; ++y) {
                for (var x = 0; x < Size; x++) {
                    var index = (x + y * Size) * 4;
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
            var buffer = new Vector2[Size * Size * 4];
            _Coord = new ArrayBuffer<Vector2>(Handle, "coord", buffer);
            _Coord.GenerateDynamic();
        }

        void UpdateCoord(Object @object) {
            var image = _Sprite.VideoRam.Size;
            var size = new SizeF(8f / image, 8f / image);
            for (var y = 0; y < @object.Size.Height; ++y) {
                for (var x = 0; x < @object.Size.Width; ++x) {
                    var name = @object.Name + x + y * @object.Size.Width;
                    var u = name % (image / 8);
                    var v = name / (image / 8);
                    var location = new PointF(u * 8f / image, v * 8f / image);
                    var uv = new RectangleF(location, size);
                    var index = (x + y * Size) * 4;
                    _Coord[index + 0] = new Vector2(uv.Left, uv.Top);
                    _Coord[index + 1] = new Vector2(uv.Right, uv.Top);
                    _Coord[index + 2] = new Vector2(uv.Left, uv.Bottom);
                    _Coord[index + 3] = new Vector2(uv.Right, uv.Bottom);
                }
            }
            _Coord.Update();
        }

        void GenerateIndex() {
            var buffer = new int[Size * Size * 6];
            _Index = new ElementArrayBuffer<int>(buffer);
            _Index.GenerateDynamic();
        }

        void UpdateIndex(Object @object) {
            for (var y = 0; y < @object.Size.Height; ++y) {
                for (var x = 0; x < @object.Size.Width; x++) {
                    var vertex = (x + y * Size) * 4;
                    var index = (x + y * @object.Size.Width) * 6;
                    _Index[index + 0] = 0 + vertex;
                    _Index[index + 1] = 1 + vertex;
                    _Index[index + 2] = 2 + vertex;
                    _Index[index + 3] = 1 + vertex;
                    _Index[index + 4] = 2 + vertex;
                    _Index[index + 5] = 3 + vertex;
                }
            }
            _Index.Update();
        }

        const int Size = 8;
        readonly Sprite _Sprite;
        readonly int _Projection;
        readonly int _ModelView;
        readonly int _Texture;
        readonly int _VertexArray;
        ArrayBuffer<Vector2> _Vertex;
        ArrayBuffer<Vector2> _Coord;
        ElementArrayBuffer<int> _Index;
    }
}
