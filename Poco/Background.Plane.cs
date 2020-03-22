using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Poco
{
    public sealed partial class Background
    {
        public sealed class Plane : IEnumerable<Background.Character>, IDisposable
        {
            public static Plane Create(int mapSize, int videoRamSize) {
                return new Plane(mapSize, videoRamSize);
            }
            public int Size { get; }

            public int X { get; set; }

            public int Y { get; set; }

            public int Priority { get; set; }

            public VideoRam VideoRam { get; }

            public ref Character this[int index] {
                get { return ref _Map[index]; }
            }

            public ref Character this[int x, int y] {
                get { return ref _Map[x + y * Size]; }
            }

            public Plane(int mapSize, int videoRamSize) {
                Size = mapSize;
                _Map = new Character[Size * Size];
                VideoRam = new VideoRam(videoRamSize);
            }

            public void Load(int index, Image image) {
                VideoRam.Load(index, image);
            }

            public void Reset() {
                Array.Clear(_Map, 0, _Map.Length);
            }

            public IEnumerator<Character> GetEnumerator() {
                return ((IEnumerable<Character>)_Map).GetEnumerator();
            }

            public void Dispose() {
                VideoRam.Dispose();
                GC.SuppressFinalize(this);
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return _Map.GetEnumerator();
            }

            readonly Character[] _Map;
        }
    }
}
