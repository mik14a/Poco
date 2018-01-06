using System;
using System.Drawing;
using System.Linq;
using Poco.Sail.Managers;

namespace Poco.Sail.Components
{
    public class SpriteComponent : Component, SpriteController.ISpriteComponent
    {
        public int Name;
        public int Priority;
        public int X;
        public int Y;
        public Size Size;

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
