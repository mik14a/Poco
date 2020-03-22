using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Poco.Managers;

namespace Poco.Controllers
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
                .Where(bg => bg.IsDirty)
                .ForEach(bg => {
                    var layer = bg.Layer;
                    var location = bg.Rectangle.Location;
                    var size = bg.Rectangle.Size;
                    for (var y = 0; y < size.Height; ++y) {
                        for (var x = 0; x < size.Width; ++x) {
                            _Background[layer][location.X + x, location.Y + y] = bg[x, y];
                        }
                    }
                    bg.IsDirty = false;
                });
        }

        public void Load(int layer, string path) {
            var videoRamManager = _VideoRamManager[layer];
            var background = _Background[layer];
            using (var image = Image.FromFile(path)) {
                var width = image.Width / 8;
                var height = image.Height / 8;
                var size = width * height;
                var index = videoRamManager.GetFreeIndex(size);
                videoRamManager.Reserve(index, size);
                background.Load(index, image);
            }
        }

        public void FromGZippedBase64String(int layer, int width, int height, string value) {
            var background = _Background[layer];
            using (var memory = new MemoryStream(Convert.FromBase64String(value)))
                using (var stream = new GZipStream(memory, CompressionMode.Decompress)) {
                    var buffer = new byte[width * height * 4];
                    stream.Read(buffer, 0, buffer.Length);
                    var map = buffer.Buffer(4).Select(b => BitConverter.ToInt32(b.ToArray(), 0)).ToArray();
                    for (var y = 0; y < 20; ++y) {
                        for (var x = 0; x < 30; ++x) {
                            var c = map[x + y * 30];
                            if (c == 0)
                                continue;
                            background[x, y].No = c - 1;
                        }
                    }
                }
        }

        public void Reset(int layer) {
            _VideoRamManager[layer].Reset();
        }

        readonly Background[] _Background;
        readonly VideoRamManager[] _VideoRamManager;

        public interface IBackgroundComponent
        {
            bool IsDirty { get; set; }

            int Layer { get; }

            Rectangle Rectangle { get; }

            ref Character this[int x, int y] { get; }

            //void Updated();
        }
    }
}
