using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    public class Sprite : IEnumerable<Object>, IDisposable
    {
        public int Size => _Size;
        public VideoRam VideoRam => _VideoRam;

        public ref Object this[int index] {
            get { return ref _Attribute[index]; }
        }

        public Sprite(MachineSettings.SpriteSettings spriteSettings) {
            _Size = spriteSettings.AttributeSize;
            _Attribute = new Object[spriteSettings.AttributeSize];
            _VideoRam = new VideoRam(spriteSettings.VideoRamSize);
        }

        public void Load(int index, Image image) {
            _VideoRam.Load(index, image);
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
