using System;
using System.Drawing;
using System.Linq;
using Poco.Sail;
using Poco.Sail.Components;

namespace Shooting.Entities
{
    class Bullet : Entity
    {
        public static int Index { get; set; } = -1;

        public SpriteComponent Sprite { get; private set; }

        protected Bullet() {
            OnUpdate = Initialize;
            Scene.Director.Sprite.Add(Sprite = Attach<SpriteComponent>());
        }
        UpdateHandler Initialize() {
            Sprite.Name = Index;
            Sprite.Size = new Size(1, 1);
            return Control;
        }

        UpdateHandler Control() {
            Sprite.Y -= 4;
            return -8 < Sprite.Y ? (UpdateHandler)Control : Kill;
        }

        UpdateHandler Kill() {
            Scene.Remove(this);
            return null;
        }
    }
}
