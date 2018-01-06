using System;
using System.Linq;
using Poco.Sail;
using Shooting.Entities;

namespace Shooting.Scenes
{
    class Stage : Scene
    {
        public Stage() {
        }

        protected override void OnEnter() {
            Player.Index = Director.Sprite.Load(@"Assets\Player.png");
            Bullet.Index = Director.Sprite.Load(@"Assets\Bullet.png");
            Director.Background.Load(0, @"Assets\Font.png");
            Add<Player>();
        }

        protected override void OnExit() {
            Player.Index = Bullet.Index = -1;
        }
    }
}
