using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    class ElementArrayBuffer
    {
        public int BufferObject { get; }

        public ElementArrayBuffer() {
            BufferObject = GL.GenBuffer();
        }
    }

    class ElementArrayBuffer<T> : ElementArrayBuffer
        where T : struct
    {
        public T[] Buffer { get; }

        public ref T this[int index]
        {
            get { return ref Buffer[index]; }
        }

        public ElementArrayBuffer(T[] buffer) {
            Buffer = buffer;
        }
    }

    static class ElementArrayBufferExtensions
    {
        public static void GenerateStatic(this ElementArrayBuffer<int> buffer) {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.BufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, buffer.Buffer.Length * sizeof(int), buffer.Buffer, BufferUsageHint.StaticDraw);
        }

        public static void GenerateDynamic(this ElementArrayBuffer<int> buffer) {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.BufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, buffer.Buffer.Length * sizeof(int), buffer.Buffer, BufferUsageHint.DynamicDraw);
        }

        public static void Update(this ElementArrayBuffer<int> buffer) {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, buffer.Buffer.Length * sizeof(int), buffer.Buffer);
        }
    }
}
