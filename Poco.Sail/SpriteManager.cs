using System;
using System.Linq;

namespace Poco.Sail
{
    public class SpriteManager
    {
        public SpriteManager(Sprite sprite) {
            _Sprite = sprite;
        }

        readonly Sprite _Sprite;
    }
}
