using System;
using System.Drawing;
using System.Linq;
using Poco;
using Poco.Components;

namespace Shooting.Entities
{
    class Bullet : Entity
    {
        public static int Index { get; set; } = -1;

        public SpriteComponent Sprite { get; private set; }

        protected override void OnAdd() {
            OnUpdate = Initialize;
            Sprite = Attach<SpriteComponent>();
            Scene.Director.Sprite.Add(Sprite);
        }

        protected override void OnRemove() {
            Scene.Director.Sprite.Remove(Sprite);
        }

        UpdateHandler Initialize() {
            Sprite.Name = Index;
            Sprite.Size = new Size(1, 1);
            return Control;
        }

        UpdateHandler Control() {
            Sprite.Y -= 4;
            return -8 < Sprite.Y ? Control : (UpdateHandler)Kill;
        }

        UpdateHandler Kill() {
            Scene.Remove(this);
            return null;
        }
    }
}
