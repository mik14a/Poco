using System;
using System.Drawing;
using System.Linq;
using Poco.Sail;
using Poco.Sail.Components;

namespace Shooting.Entities
{
    class Player : Entity
    {
        public static int Index { get; set; } = -1;

        public SpriteComponent Sprite { get; private set; }
        public InputComponent Input { get; private set; }

        protected Player() {
            OnUpdate = Initialize;
            Scene.Director.Sprite.Add(Sprite = Attach<SpriteComponent>());
            Scene.Director.Input.Add(Input = Attach<InputComponent>());
        }

        UpdateHandler Initialize() {
            Sprite.Name = Index;
            Sprite.X = Sprite.Y = 100;
            Sprite.Size = new Size(2, 2);
            return Control;
        }

        UpdateHandler Control() {
            const int delta = 2;
            if (Input.Key(Poco.Input.Keys.Up)) Sprite.Y -= delta;
            if (Input.Key(Poco.Input.Keys.Down)) Sprite.Y += delta;
            if (Input.Key(Poco.Input.Keys.Left)) Sprite.X -= delta;
            if (Input.Key(Poco.Input.Keys.Right)) Sprite.X += delta;
            if (Input.Pressed(Poco.Input.Keys.A)) {
                var bullet = Scene.Add<Bullet>();
                bullet.Sprite.X = Sprite.X + 4;
                bullet.Sprite.Y = Sprite.Y;
            }
            return Control;
        }
    }
}
