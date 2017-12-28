using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poco
{
    public partial class Background : IEnumerable<Character>, IDisposable
    {
        public int Size => _Size;
        public VideoRam VideoRam => _VideoRam;

        public int Priority { get; set; }

        public ref Character this[int x, int y] {
            get { return ref _Map[x + y * _Size]; }
        }

        public Background(int mapSize, int videoRamSize) {
            _Size = mapSize;
            _Map = new Character[_Size * _Size];
            _VideoRam = new VideoRam(videoRamSize);
        }

        public IEnumerator<Character> GetEnumerator() {
            return ((IEnumerable<Character>)_Map).GetEnumerator();
        }

        public void Dispose() {
            _VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Map.GetEnumerator();
        }

        readonly int _Size;
        readonly Character[] _Map;
        readonly VideoRam _VideoRam;
    }
}
