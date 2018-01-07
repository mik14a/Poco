using System;
using System.IO;
using System.Linq;
using Poco;

namespace Shooting.Scenes
{
    class Title : Scene
    {
        protected override void OnEnter() {
            OnUpdate = Control;
            Director.Background.Load(0, @"Assets\Font.png");
            var map = File.ReadAllText(@"Assets\Title.map");
            Director.Background.FromGZippedBase64String(0, 30, 20, map);
        }

        UpdateHandler Control() {
            if (Director.Input.Pressed(Input.Keys.Start)) {
                Director.Poco.Background[0].Reset();
                Director.Background.Reset(0);
                Director.Enter<Stage>();
            }
            return Control;
        }
    }
}
