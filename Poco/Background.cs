using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    public sealed class Background : IEnumerable<Character>, IDisposable
    {
        public int Size { get; }

        public Character[] Map { get; }

        public VideoRam VideoRam { get; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Priority { get; set; }

        public ref Character this[int index]
        {
            get { return ref Map[index]; }
        }

        public ref Character this[int x, int y]
        {
            get { return ref Map[x + y * Size]; }
        }

        public Background(Settings.Backgrounds backgrounds) {
            Size = backgrounds.MapSize;
            Map = new Character[Size * Size];
            VideoRam = new VideoRam(backgrounds.VideoRamSize);
        }

        public void Load(int index, Image image) {
            VideoRam.Load(index, image);
        }

        public void Reset() {
            Array.Clear(Map, 0, Map.Length);
        }

        public IEnumerator<Character> GetEnumerator() {
            return ((IEnumerable<Character>)Map).GetEnumerator();
        }

        public void Dispose() {
            VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Map.GetEnumerator();
        }
    }
}
