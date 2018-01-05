using System;
using System.Diagnostics;
using System.Linq;
using Poco;
using Poco.Sail;

namespace Shooting
{
    static class Game
    {
        public static Machine Machine => _Machine;
        public static BackgroundManager Background => _BackgroundManager;
        public static SpriteManager Sprite => _SpriteManager;
        public static InputManager Input => _InputManager;


        static Game() {
            _Machine = Machine.Create("Shooting");
            _BackgroundManager = new BackgroundManager(_Machine.Background);
            _SpriteManager = new SpriteManager(_Machine.Sprite);
            _InputManager = new InputManager(_Machine.Input);
            Task.Initialize(_Machine.Sprite);
        }

        static void Main(string[] args) {
            var stage = new Stage();
            _Machine.Visible = true;
            while (_Machine.Exists) {
                _Machine.ProcessEvents();
                var elapsed = _Stopwatch.ElapsedMilliseconds;
                if (_MillisecondsParSeconds < elapsed) {
                    _InputManager.Execute();
                    stage.Synchronize(elapsed);
                    stage.Invalidate();
                    _Machine.Execute();
                    _Stopwatch.Restart();
                }
            }
        }

        readonly static Machine _Machine;
        readonly static BackgroundManager _BackgroundManager;
        readonly static SpriteManager _SpriteManager;
        readonly static InputManager _InputManager;
        readonly static Stopwatch _Stopwatch = Stopwatch.StartNew();
        const int _MillisecondsParSeconds = 1000 / 60;
    }
}
