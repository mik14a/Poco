using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public class Sprite : IEnumerable<Object>, IDisposable
    {
        public int Size => _Size;

        public VideoRam VideoRam => _VideoRam;

        public ref Object this[int index] {
            get { return ref _Attribute[index]; }
        }

        public Sprite(int attributeSize, int videoRamSize) {
            _Size = attributeSize;
            _Attribute = new Object[attributeSize];
            _VideoRam = new VideoRam(videoRamSize);
        }

        public IEnumerator<Object> GetEnumerator() {
            return ((IEnumerable<Object>)_Attribute).GetEnumerator();
        }

        public void Dispose() {
            _VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Attribute.GetEnumerator();
        }

        readonly int _Size;
        readonly Object[] _Attribute;
        readonly VideoRam _VideoRam;
    }
}