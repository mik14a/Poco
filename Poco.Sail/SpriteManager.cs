using System;
using System.Drawing;
using System.Linq;

namespace Poco.Sail
{
    public class SpriteManager
    {
        public SpriteManager(Sprite sprite) {
            _Sprite = sprite;
            _VideoRamManager = new VideoRamManager(_Sprite.VideoRam);
        }

        public int Load(string path) {
            using (var image = Image.FromFile(path)) {
                var width = image.Width / 8;
                var height = image.Height / 8;
                var size = width * height;
                var index = _VideoRamManager.GetFreeIndex(size);
                _VideoRamManager.Reserve(index, size);
                _Sprite.Load(index, image);
                return index;
            }
        }

        readonly Sprite _Sprite;
        readonly VideoRamManager _VideoRamManager;

    }
}
