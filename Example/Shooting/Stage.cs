using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poco;

namespace Shooting
{
    class Stage
    {
        public Stage() {
            Game.Background.Load(0, @"Assets\Font.png");
            _Manager = new TaskManager(Game.Machine.Sprite);
            _Manager.Add(new Player());
        }

        public void Execute(long elapsed) {
            DisplayElapsedMilliseconds(elapsed);
            DisplayKey(Game.Machine.Input.Key);
            _Manager.Execute();
        }
        public void Draw() {
            _Manager.Draw();
        }

        static void DisplayElapsedMilliseconds(long elapsedMilliseconds) {
            var value = elapsedMilliseconds.ToString();
            value.Take(3).ForEach((c, i) => Game.Machine.Backgrounds[0][i, 0].No = c);
        }

        static void DisplayKey(Input.Keys keys) {
            var a = "\u001e\u001f\u0011\u0010\u0015\u000fAB";
            var key = 0x80u;
            Game.Machine.Backgrounds[0][0, 1].No = '[';
            for (var i = 0; i < 8; ++i) {
                Game.Machine.Backgrounds[0][i + 1, 1].No = a[i];
                key >>= 1;
            }
            Game.Machine.Backgrounds[0][9, 1].No = ']';
        }

        readonly TaskManager _Manager;
    }
}
