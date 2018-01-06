using System;
using System.Linq;
using Poco;
using Poco.Sail;
using Shooting.Scenes;

namespace Shooting
{
    static class Game
    {
        public static Director Director { get; private set; }

        static void Main(string[] args) {
            Director = new Director(Machine.Create("Shooting"));
            Director.RunWithScene<Stage>();
        }
    }
}
