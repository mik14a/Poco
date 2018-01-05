using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    abstract class Shader : IDisposable
    {
        public int Handle => _Program;

        public Shader(string vertexShaderPath, string fragmentShaderPath) {
            _Program = GL.CreateProgram();
            _VertexShader = CreateShader(ShaderType.VertexShader, vertexShaderPath);
            _FragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderPath);
            GL.AttachShader(_Program, _VertexShader);
            GL.AttachShader(_Program, _FragmentShader);
            GL.LinkProgram(_Program);
            Debug.WriteLine(GL.GetProgramInfoLog(_Program));
        }

        public int CreateShader(ShaderType shaderType, string path) {
            var shader = GL.CreateShader(shaderType);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream)) {
                GL.ShaderSource(shader, reader.ReadToEnd());
            }
            GL.CompileShader(shader);
            Debug.WriteLine(GL.GetShaderInfoLog(shader));
            return shader;
        }

        public void Update() {
            UpdateImpl();
        }

        public void Render(Matrix4 projection) {
            RenderImpl(projection);
        }

        protected abstract void UpdateImpl();

        protected abstract void RenderImpl(Matrix4 projection);

        public void Dispose() {
            GL.DeleteProgram(_Program);
            GL.DeleteShader(_VertexShader);
            GL.DeleteShader(_FragmentShader);
        }

        private readonly int _Program;
        protected readonly int _VertexShader;
        protected readonly int _FragmentShader;


        protected class ArrayBuffer
        {
            public int Program => _Program;
            public string Name => _Name;
            public int Handle => _Handle;
            public int BufferObject => _BufferObject;

            public ArrayBuffer(int program, string name) {
                _Program = program;
                _Name = name;
                _Handle = GL.GetAttribLocation(_Program, _Name);
                _BufferObject = GL.GenBuffer();
            }

            readonly int _Program;
            readonly string _Name;
            readonly int _Handle;
            readonly int _BufferObject;

            public static void GenerateStatic(ArrayBuffer<Vector2> buffer) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, buffer.Buffer.Length * Vector2.SizeInBytes, buffer.Buffer, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(buffer.Handle, 2, VertexAttribPointerType.Float, false, 0, 0);
            }

            public static void GenerateDynamic(ArrayBuffer<Vector2> buffer) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, buffer.Buffer.Length * Vector2.SizeInBytes, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(buffer.Handle, 2, VertexAttribPointerType.Float, false, 0, 0);
            }

            public static void Update(ArrayBuffer<Vector2> buffer) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, buffer.Buffer.Length * Vector2.SizeInBytes, buffer.Buffer);
            }
        }

        protected class ElementArrayBuffer
        {
            public int BufferObject => _BufferObject;
            public ElementArrayBuffer() {
                _BufferObject = GL.GenBuffer();
            }
            readonly int _BufferObject;

            public static void GenerateStatic(ElementArrayBuffer<int> buffer) {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.BufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, buffer.Buffer.Length * sizeof(int), buffer.Buffer, BufferUsageHint.StaticDraw);
            }
        }

        protected class ArrayBuffer<T> : ArrayBuffer where T : struct
        {
            public T[] Buffer => _Buffer;

            public ref T this[int index] {
                get { return ref _Buffer[index]; }
            }

            public ArrayBuffer(int program, string name, T[] buffer) : base(program, name) {
                _Buffer = buffer;
            }

            T[] _Buffer;
        }

        protected class ElementArrayBuffer<T> : ElementArrayBuffer where T : struct
        {
            public T[] Buffer => _Buffer;

            public ref T this[int index] {
                get { return ref _Buffer[index]; }
            }

            public ElementArrayBuffer(T[] buffer) {
                _Buffer = buffer;
            }

            T[] _Buffer;
        }
    }
}
