using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    public sealed class Sprite : IEnumerable<Object>, IDisposable
    {
        public int Size { get; }

        public Object[] Attribute { get; }

        public VideoRam VideoRam { get; }

        public ref Object this[int index]
        {
            get { return ref Attribute[index]; }
        }

        public Sprite(Settings.Sprites sprites) {
            Size = sprites.AttributeSize;
            Attribute = new Object[sprites.AttributeSize];
            VideoRam = new VideoRam(sprites.VideoRamSize);
        }

        public void Load(int index, Image image) {
            VideoRam.Load(index, image);
        }

        public void Reset() {
            Array.Clear(Attribute, 0, Attribute.Length);
        }

        public IEnumerator<Object> GetEnumerator() {
            return ((IEnumerable<Object>)Attribute).GetEnumerator();
        }

        public void Dispose() {
            VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Attribute.GetEnumerator();
        }
    }
}
