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
        public static InputManager Input => _InputManager;
        public static BackgroundManager Background => _BackgroundManager;
        public static SpriteManager Sprite => _SpriteManager;


        static Game() {
            _Machine = Machine.Create("Shooting");
            _InputManager = new InputManager(_Machine.Input);
            _BackgroundManager = new BackgroundManager(_Machine.Background);
            _SpriteManager = new SpriteManager(_Machine.Sprite);
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

        static readonly Machine _Machine;
        static readonly InputManager _InputManager;
        static readonly BackgroundManager _BackgroundManager;
        static readonly SpriteManager _SpriteManager;
        static readonly Stopwatch _Stopwatch = Stopwatch.StartNew();
        const int _MillisecondsParSeconds = 1000 / 60;
    }
}
