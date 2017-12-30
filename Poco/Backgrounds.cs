using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poco
{
    public class Backgrounds : IEnumerable<Background>, IDisposable
    {
        public int Count => _Count;

        public Background this[int index] => _Background[index];

        public Backgrounds(MachineSettings.BackgroundsSettings backgroundsSettings) {
            _Count = backgroundsSettings.Layers;
            _Background = new Background[_Count];
            for (var i = 0; i < _Background.Length; ++i) {
                _Background[i] = new Background(backgroundsSettings.Background);
            }
        }


        public IEnumerator<Background> GetEnumerator() {
            return ((IEnumerable<Background>)_Background).GetEnumerator();
        }

        public void Dispose() {
            Array.ForEach(_Background, b => b.Dispose());
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Background.GetEnumerator();
        }

        readonly int _Count;
        readonly Background[] _Background;
    }
}
