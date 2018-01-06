using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poco.Sail.Managers
{
    public class VideoRamManager
    {
        public VideoRamManager(VideoRam videoRam) {
            _VideoRam = videoRam;
            _Map = new Dictionary<int, int>();
            _Usage = new BitArray(videoRam.Size * videoRam.Size);
        }

        public void Reserve(int index, int size) {
            _Map[index] = size;
            for (var i = 0; i < size; ++i) {
                _Usage[index + i] = true;
            }
        }

        public void Release(int index) {
            var size = _Map[index];
            for (var i = 0; i < size; ++i) {
                _Usage[index + i] = false;
            }
            _Map.Remove(index);
        }

        public void Reset() {
            _Map.Clear();
            _Usage.SetAll(false);
        }

        public int GetFreeIndex(int size) {
            var index = 0;
            while (index < _Usage.Length) {
                if (!_Usage[index]) {
                    var length = 0;
                    while (length < size && !_Usage[index + length]) {
                        ++length;
                    }
                    if (length == size) {
                        return index;
                    }
                }
                ++index;
            }
            return -1;
        }

        readonly VideoRam _VideoRam;
        readonly Dictionary<int, int> _Map;
        readonly BitArray _Usage;
    }
}
