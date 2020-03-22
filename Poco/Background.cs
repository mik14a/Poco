using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    public sealed class Background : IEnumerable<Background.Plane>, IDisposable
    {
        public int Length => _Planes.Length;

        public ref Plane this[int index]
        {
            get { return ref _Planes[index]; }
        }

        public Background(Settings.Backgrounds background) {
            _Planes = new Plane[background.Plane];
            for (var i = 0; i < _Planes.Length; ++i) {
                _Planes[i] = new Plane(background);
            }
        }

        public IEnumerator<Plane> GetEnumerator() {
            return ((IEnumerable<Plane>)_Planes).GetEnumerator();
        }

        public void Dispose() {
            for (var i = 0; i < _Planes.Length; ++i) {
                _Planes[i].Dispose();
            }
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Planes.GetEnumerator();
        }

        readonly Plane[] _Planes;

        public sealed class Plane : IEnumerable<Background.Character>, IDisposable
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

            public Plane(Settings.Backgrounds backgrounds) {
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

        public struct Character
        {
            [Flags]
            public enum Rotates
            {
                RotateNone = 0,
                Rotate0 = 0x00,
                Rotate90 = 0x01,
                Rotate180 = 0x10,
                Rotate270 = 0x11
            }

            [Flags]
            public enum Flips
            {
                FlipNone = 0,
                FlipHorizontal = 0x01,
                FlipVertical = 0x10
            }

            public int No;
            public Rotates Rotate;
            public Flips Flip;
        }

    }
}
