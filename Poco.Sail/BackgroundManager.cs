using System;
using System.Drawing;
using System.Linq;

namespace Poco.Sail
{
    public class BackgroundManager
    {
        public BackgroundManager(Backgrounds backgrounds) {
            _Backgrounds = backgrounds;
            _VideoRamManager = new VideoRamManager[_Backgrounds.Count];
            for (int i = 0; i < _VideoRamManager.Length; ++i) {
                _VideoRamManager[i] = new VideoRamManager(_Backgrounds[i].VideoRam);
            }
        }

        public void Load(int layer, string path) {
            using (var image = Image.FromFile(path)) {
                var width = image.Width / 8;
                var height = image.Height / 8;
                var size = width * height;
                var index = _VideoRamManager[layer].GetFreeIndex(size);
                _VideoRamManager[layer].Reserve(index, size);
                _Backgrounds[layer].Load(index, image);
            }
        }

        readonly Backgrounds _Backgrounds;
        readonly VideoRamManager[] _VideoRamManager;
    }
}
