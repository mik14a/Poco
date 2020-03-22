using System;
using System.Linq;
using Poco;
using Shooting.Entities;

namespace Shooting.Scenes
{
    class Stage : Scene
    {
        protected override void OnEnter() {
            OnUpdate = Control;
            Director.Background.Load(0, @"Assets\Font.png");
            Player.Index = Director.Sprite.Load(@"Assets\Player.png");
            Bullet.Index = Director.Sprite.Load(@"Assets\Bullet.png");
            _Player = Add<Player>();
            _Score = Add<Score>();
            _Exec = Add<Exec>();
            _Draw = Add<Draw>();
        }

        UpdateHandler Control() {
            _Score.Value = (int)Director.Elapsed;
            _Exec.Value = (int)Director.Exec;
            _Draw.Value = (int)Director.Draw;
            return Control;
        }

        protected override void OnExit() {
            Player.Index = Bullet.Index = -1;
        }

        Player _Player;
        Score _Score;
        Exec _Exec;
        Draw _Draw;
    }
}
