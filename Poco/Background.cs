using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poco
{
    public sealed partial class Background : IEnumerable<Background.Plane>, IDisposable
    {
        public int Length => _Planes.Length;

        public ref Plane this[int index] {
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
    }
}
