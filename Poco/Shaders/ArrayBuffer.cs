using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    class ArrayBuffer
    {
        public int Program { get; }

        public string Name { get; }

        public int Handle { get; }

        public int BufferObject { get; }

        public ArrayBuffer(int program, string name) {
            Program = program;
            Name = name;
            Handle = GL.GetAttribLocation(Program, Name);
            BufferObject = GL.GenBuffer();
        }
    }

    class ArrayBuffer<T> : ArrayBuffer
        where T : struct
    {
        public T[] Buffer { get; }

        public ref T this[int index]
        {
            get { return ref Buffer[index]; }
        }

        public ArrayBuffer(int program, string name, T[] buffer)
            : base(program, name) {
            Buffer = buffer;
        }
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
