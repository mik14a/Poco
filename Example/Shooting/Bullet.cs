using System;
using System.Drawing;
using System.Linq;

namespace Shooting
{
    class Bullet : Task
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Bullet() {
            OnExecute = Control;
            OnDraw = Render;
        }

        ExecuteHandler Control() {
            Y -= 4;
            return -8 < Y ? (ExecuteHandler)Control : Kill;
        }

        ExecuteHandler Kill() {
            Task.Remove(this);
            return null;
        }

        Poco.Object Render() {
            return new Poco.Object() {
                Enable = true,
                Name = _Index,
                X = X,
                Y = Y,
                Size = new Size(1, 1)
            };
        }

        public static void Construct() {
            _Index = Game.Sprite.Load(@"Assets\Bullet.png");
        }

        public static void Destruct() {
            _Index = -1;
        }

        static int _Index = -1;

    }
}
