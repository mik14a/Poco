using System;
using System.Linq;

namespace Poco.Sail.Manager
{
    public class SpriteManager
    {
        public SpriteManager(Sprite sprite) {
            _Sprite = sprite;
        }

        readonly Sprite _Sprite;
    }
}
