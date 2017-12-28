using System;
using System.Linq;

namespace Poco.Sail
{
    public class BackgroundManager
    {
        public void Initialize(Backgrounds backgrounds) {
            _Backgrounds = backgrounds;
        }
        Backgrounds _Backgrounds;
    }
}
