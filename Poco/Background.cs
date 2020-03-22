using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poco
{
    public sealed partial class Background : IEnumerable<Background.Plane>, IDisposable
    {
        public static Background Create(Settings.Backgrounds background) {
            return new Background(background.Plane, background.MapSize, background.VideoRamSize);
        }

        public int Size => _Planes.Length;

        public ref Plane this[int index] {
            get { return ref _Planes[index]; }
        }

        Background(int plane, int mapSize, int videoRamSize) {
            _Planes = new Plane[plane];
            for (var i = 0; i < _Planes.Length; ++i) {
                _Planes[i] = Plane.Create(mapSize, videoRamSize);
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
    }
}
