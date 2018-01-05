using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    class ElementArrayBuffer
    {
        public int BufferObject => _BufferObject;

        public ElementArrayBuffer() {
            _BufferObject = GL.GenBuffer();
        }

        readonly int _BufferObject;
    }

    class ElementArrayBuffer<T> : ElementArrayBuffer where T : struct
    {
        public T[] Buffer => _Buffer;

        public ref T this[int index] {
            get { return ref _Buffer[index]; }
        }

        public ElementArrayBuffer(T[] buffer) {
            _Buffer = buffer;
        }

        readonly T[] _Buffer;
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
