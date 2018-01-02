using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco.Sail
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
            for (int i = 0; i < size; ++i) {
                _Usage[index + i] = true;
            }
        }

        public void Release(int index) {
            var size = _Map[index];
            for (int i = 0; i < size; ++i) {
                _Usage[index + i] = false;
            }
            _Map.Remove(index);
        }


        public int GetFreeIndex(int size) {
            var index = 0;
            while (index < _Usage.Length) {
                if (_Usage[index]) continue;

                var length = 0;
                while (length < size) {
                    if (_Usage[index + length]) {
                        break;
                    }
                    ++length;
                }
                if (length == size) {
                    return index;
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
