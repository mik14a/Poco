using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting
{
    class Player : Task
    {
        public Player() {
            OnExecute = Initialize;
            OnDraw = Render;
        }

        ExecuteHandler Initialize() {
            _Index = Game.Sprite.Load(@"Assets\Player.png");
            _X = _Y = 100;
            return Control;
        }

        ExecuteHandler Control() {
            const int delta = 2;
            if (Game.Input.IsKey(Poco.Input.Keys.Up)) _Y -= delta;
            if (Game.Input.IsKey(Poco.Input.Keys.Down)) _Y += delta;
            if (Game.Input.IsKey(Poco.Input.Keys.Left)) _X -= delta;
            if (Game.Input.IsKey(Poco.Input.Keys.Right)) _X += delta;
            return Control;
        }

        Poco.Object Render() {
            return new Poco.Object() {
                Name = _Index,
                X = _X,
                Y = _Y,
                Size = new Size(2, 2)
            };
        }

        int _Index;
        int _X, _Y;
    }
}
