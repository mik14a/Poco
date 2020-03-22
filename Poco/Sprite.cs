using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    /// <summary>
    /// Sprite memory.
    /// </summary>
    public sealed class Sprite : IEnumerable<Sprite.Attribute>, IDisposable
    {
        public int Size { get; }

        public Attribute[] Attributes { get; }

        public VideoRam VideoRam { get; }

        public ref Attribute this[int index]
        {
            get { return ref Attributes[index]; }
        }

        public Sprite(Settings.Sprites sprites) {
            Size = sprites.AttributeSize;
            Attributes = new Attribute[sprites.AttributeSize];
            VideoRam = new VideoRam(sprites.VideoRamSize);
        }

        public void Load(int index, Image image) {
            VideoRam.Load(index, image);
        }

        public void Reset() {
            Array.Clear(Attributes, 0, Attributes.Length);
        }

        public IEnumerator<Attribute> GetEnumerator() {
            return ((IEnumerable<Attribute>)Attributes).GetEnumerator();
        }

        public void Dispose() {
            VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Attributes.GetEnumerator();
        }

        public struct Attribute
        {
            public bool Enable;
            public int Name;
            public int Priority;
            public int X;
            public int Y;
            public Size Size;
        }
    }
}
