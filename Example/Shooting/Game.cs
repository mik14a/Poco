using System;
using System.Linq;
using Poco;
using Poco.Sail;
using Shooting.Scenes;

namespace Shooting
{
    static class Game
    {
        static void Main(string[] args) {
            var machine = Machine.Create("Shooting");
            var director = new Director(machine);
            director.RunWithScene<Title>();
        }
    }
}
