using System;
using System.Drawing;
using System.Linq;
using Poco.Sail;
using Poco.Sail.Components;

namespace Shooting.Entities
{
    class Score : Entity
    {
        public int Value { get; set; }

        public BackgroundComponent Background { get; private set; }

        protected override void OnAdd() {
            OnUpdate = Initialize;
            Background = Attach<BackgroundComponent>();
            Scene.Director.Background.Add(Background);
        }

        UpdateHandler Initialize() {
            Background.Layer = 0;
            Background.Rectangle = new Rectangle(0, 0, 8, 1);
            return Control;
        }

        UpdateHandler Control() {
            var value = Value.ToString("00000000");
            value.Take(8).ForEach((c, i) => Background[i, 0].No = c);
            Background.MakeRenderDirty();
            return Control;
        }
    }
}
