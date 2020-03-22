using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Poco.Controllers;
using Poco.Managers;

namespace Poco
{
    public class Director
    {
        public Machine Poco { get; }

        public InputController Input { get; }

        public BackgroundController Background { get; }

        public SpriteController Sprite { get; }

        public long Elapsed { get; private set; }

        public Director(Machine machine) {
            Poco = machine;
            Input = new InputController(Poco.Input);
            Background = new BackgroundController(Poco.Background);
            Sprite = new SpriteController(Poco.Sprite);
            _Scene = new Stack<Scene>();
        }

        public void RunWithScene<T>()
            where T : Scene {
            Enter<T>();
            Run();
        }

        public void Run() {
            Poco.Visible = true;
            while (Poco.Exists) {
                Poco.ProcessEvents();
                Elapsed = _Stopwatch.ElapsedTicks;
                if (_FrequenciesParFrame < Elapsed) {
                    _Stopwatch.Restart();
                    Input.Update();
                    var scene = _Scene.Peek();
                    scene.Update();
                    Background.Update();
                    Sprite.Update();
                    Poco.Update();
                }
            }
        }

        public T Enter<T>()
            where T : Scene {
            var scene = Activator<T>.CreateInstance();
            ((IScene)scene).Enter(this);
            _Scene.Push(scene);
            return scene;
        }

        public void Exit() {
            var scene = _Scene.Pop();
            ((IScene)scene).Exit();
        }

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
