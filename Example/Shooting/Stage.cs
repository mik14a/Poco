using System;
using System.Linq;
using Poco;

namespace Shooting
{
    class Stage
    {
        public Stage() {
            Player.Construct();
            Bullet.Construct();
            Game.Background.Load(0, @"Assets\Font.png");
            Task.Add(new Player());
        }

        public void Synchronize(long elapsed) {
            DisplayElapsedMilliseconds(elapsed);
            Task.Synchronize();
        }
        public void Invalidate() {
            Task.Invalidate();
        }

        static void DisplayElapsedMilliseconds(long elapsedMilliseconds) {
            var value = elapsedMilliseconds.ToString("0000");
            value.Take(4).ForEach((c, i) => Game.Machine.Background[0][i, 0].No = c);
        }
    }
}
