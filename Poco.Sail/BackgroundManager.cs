using System;
using System.Drawing;
using System.Linq;

namespace Poco.Sail
{
    public class BackgroundManager
    {
        public BackgroundManager(Background[] background) {
            _Background = background;
            _VideoRamManager = new VideoRamManager[_Background.Length];
            for (var i = 0; i < _VideoRamManager.Length; ++i) {
                _VideoRamManager[i] = new VideoRamManager(_Background[i].VideoRam);
            }
        }

        public void Load(int layer, string path) {
            using (var image = Image.FromFile(path)) {
                var width = image.Width / 8;
                var height = image.Height / 8;
                var size = width * height;
                var index = _VideoRamManager[layer].GetFreeIndex(size);
                _VideoRamManager[layer].Reserve(index, size);
                _Background[layer].Load(index, image);
            }
        }

        readonly Background[] _Background;
        readonly VideoRamManager[] _VideoRamManager;
    }
}
