using System;
using System.Drawing;
using System.Linq;

namespace Poco.Managers
{
    public class SpriteController : Controller
    {
        public SpriteController(Sprite sprite) {
            _Sprite = sprite;
            _VideoRamManager = new VideoRamManager(_Sprite.VideoRam);
        }

        public override void Update() {
            _Sprite.Reset();
            _Component.Cast<ISpriteComponent>().Select(spr => spr.ToObject()).ForEach((obj, i) => _Sprite[i] = obj);
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

        public interface ISpriteComponent
        {
            Object ToObject();
        }
    }
}
