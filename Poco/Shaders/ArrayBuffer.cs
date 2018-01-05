using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    class ArrayBuffer
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
    }

    class ArrayBuffer<T> : ArrayBuffer where T : struct
    {
        public T[] Buffer => _Buffer;

        public ref T this[int index] {
            get { return ref _Buffer[index]; }
        }

        public ArrayBuffer(int program, string name, T[] buffer) : base(program, name) {
            _Buffer = buffer;
        }

        readonly T[] _Buffer;
    }

    static class ArrayBufferExtensions
    {
        public static void GenerateStatic(this ArrayBuffer<Vector2> buffer) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, buffer.Buffer.Length * Vector2.SizeInBytes, buffer.Buffer, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(buffer.Handle);
            GL.VertexAttribPointer(buffer.Handle, 2, VertexAttribPointerType.Float, false, 0, 0);
        }

        public static void GenerateDynamic(this ArrayBuffer<Vector2> buffer) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, buffer.Buffer.Length * Vector2.SizeInBytes, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(buffer.Handle);
            GL.VertexAttribPointer(buffer.Handle, 2, VertexAttribPointerType.Float, false, 0, 0);
        }

        public static void Update(this ArrayBuffer<Vector2> buffer) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.BufferObject);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, buffer.Buffer.Length * Vector2.SizeInBytes, buffer.Buffer);
            GL.VertexAttribPointer(buffer.Handle, 2, VertexAttribPointerType.Float, false, 0, 0);
        }
    }
}
