using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    public sealed class Background : IEnumerable<Character>, IDisposable
    {
        public int Size => _Size;
        public Character[] Map => _Map;
        public VideoRam VideoRam => _VideoRam;

        public int X { get; set; }
        public int Y { get; set; }
        public int Priority { get; set; }

        public ref Character this[int index] {
            get { return ref _Map[index]; }
        }

        public ref Character this[int x, int y] {
            get { return ref _Map[x + y * _Size]; }
        }

        public Background(Settings.Backgrounds backgrounds) {
            _Size = backgrounds.MapSize;
            _Map = new Character[_Size * _Size];
            _VideoRam = new VideoRam(backgrounds.VideoRamSize);
        }

        public void Load(int index, Image image) {
            _VideoRam.Load(index, image);
        }

        public void Reset() {
            Array.Clear(_Map, 0, _Map.Length);
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
