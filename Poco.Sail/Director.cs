using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Poco.Sail.Controllers;
using Poco.Sail.Managers;

namespace Poco.Sail
{
    public class Director
    {
        public Machine Poco => _Machine;
        public InputController Input => _InputController;
        public BackgroundController Background => _BackgroundController;
        public SpriteController Sprite => _SpriteController;

        public long Elapsed { get; private set; }

        public Director(Machine machine) {
            _Machine = machine;
            _InputController = new InputController(_Machine.Input);
            _BackgroundController = new BackgroundController(_Machine.Background);
            _SpriteController = new SpriteController(_Machine.Sprite);
            _Scene = new Stack<Scene>();
        }

        public void RunWithScene<T>() where T : Scene {
            Enter<T>();
            Run();
        }

        public void Run() {
            _Machine.Visible = true;
            while (_Machine.Exists) {
                _Machine.ProcessEvents();
                Elapsed = _Stopwatch.ElapsedTicks;
                if (_FrequenciesParFrame < Elapsed) {
                    _Stopwatch.Restart();
                    _InputController.Update();
                    var scene = _Scene.Peek();
                    scene.Update();
                    _BackgroundController.Update();
                    _SpriteController.Update();
                    _Machine.Update();
                }
            }
        }

        public T Enter<T>() where T : Scene {
            var scene = Activator<T>.CreateInstance();
            ((IScene)scene).Enter(this);
            _Scene.Push(scene);
            return scene;
        }

        public void Exit() {
            var scene = _Scene.Pop();
            ((IScene)scene).Exit();
        }

        readonly Machine _Machine;
        readonly InputController _InputController;
        readonly BackgroundController _BackgroundController;
        readonly SpriteController _SpriteController;
        readonly Stopwatch _Stopwatch = Stopwatch.StartNew();
        readonly Stack<Scene> _Scene;
        readonly long _FrequenciesParFrame = Stopwatch.Frequency / 60;

        internal interface IScene
        {
            void Enter(Director director);
            void Exit();
        }
    }
}
