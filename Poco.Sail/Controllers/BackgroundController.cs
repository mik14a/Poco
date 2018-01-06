using System;
using System.Drawing;
using System.Linq;
using Poco.Sail.Managers;

namespace Poco.Sail.Controllers
{
    public class BackgroundController : Controller
    {
        public BackgroundController(Background[] background) {
            _Background = background;
            _VideoRamManager = new VideoRamManager[_Background.Length];
            for (var i = 0; i < _VideoRamManager.Length; ++i) {
                _VideoRamManager[i] = new VideoRamManager(_Background[i].VideoRam);
            }
        }

        public override void Update() {
            _Component.Cast<IBackgroundComponent>()
                .Where(c => !c.IsDirty)
                .ForEach(c => {
                    var location = c.Rectangle.Location;
                    var size = c.Rectangle.Size;
                    for (var y = 0; y < size.Height; ++y) {
                        for (var x = 0; x < size.Width; ++x) {
                            var index = x + y * size.Width;
                            _Background[c.Layer][location.X + x, location.Y + y] = c.Map[index];
                        }
                    }
                });
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

        public interface IBackgroundComponent
        {
            bool IsDirty { get; }
            int Layer { get; }
            Rectangle Rectangle { get; }
            Character[] Map { get; }
        }
    }
}
