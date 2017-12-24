using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poco
{
    public class Backgrounds : IDisposable
    {
        public int Count => _Background.Count;

        public Background this[int index] => _Background[index];

        public Backgrounds(int numberOfPlane, int mapSize, int videoRamSize) {
            for (int i = 0; i < numberOfPlane; ++i) {
                _Background.Add(new Background(mapSize, videoRamSize));
            }
        }

        public void Draw() {
            _Background.ForEach(b => b.Draw());
        }

        public void Dispose() {
            _Background.ForEach(b => b.Dispose());
            _Background.Clear();
            GC.SuppressFinalize(this);
        }

        readonly List<Background> _Background = new List<Background>();
    }
}
