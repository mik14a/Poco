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
        public int X { get; set; }
        public int Y { get; set; }

        public Player() : base() {
            OnExecute = Initialize;
            OnDraw = Render;
        }

        ExecuteHandler Initialize() {
            X = Y = 100;
            return Control;
        }

        ExecuteHandler Control() {
            const int delta = 2;
            if (Game.Input.IsKey(Poco.Input.Keys.Up)) Y -= delta;
            if (Game.Input.IsKey(Poco.Input.Keys.Down)) Y += delta;
            if (Game.Input.IsKey(Poco.Input.Keys.Left)) X -= delta;
            if (Game.Input.IsKey(Poco.Input.Keys.Right)) X += delta;
            if (Game.Input.IsPressed(Poco.Input.Keys.A)) {
                var bullet = new Bullet() { X = X + 4, Y = Y };
                Task.Add(this, bullet);
            }
            return Control;
        }

        Poco.Object Render() {
            return new Poco.Object() {
                Name = _Index,
                X = X,
                Y = Y,
                Size = new Size(2, 2)
            };
        }

        public static void Construct() {
            _Index = Game.Sprite.Load(@"Assets\Player.png");
        }

        public static void Destruct() {
            _Index = -1;
        }

        static int _Index = -1;

    }
}
