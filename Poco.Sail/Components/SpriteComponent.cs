using System;
using System.Drawing;
using System.Linq;
using Poco.Managers;

namespace Poco.Components
{
    public class SpriteComponent : Component, SpriteController.ISpriteComponent
    {
        public int Name { get; set; }

        public int Priority { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Size Size { get; set; }

        protected SpriteComponent() {
        }

        Object SpriteController.ISpriteComponent.ToObject() {
            return new Object {
                Enable = true,
                Name = Name,
                Priority = Priority,
                X = X,
                Y = Y,
                Size = Size
            };
        }
    }
}
