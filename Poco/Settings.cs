using System;
using System.Linq;

namespace Poco
{
    /// <summary>
    /// Poco machine settings.
    /// </summary>
    public struct Settings
    {
        public static readonly Settings Default = new Settings {
            Screen = Screens.Default,
            Background = Backgrounds.Default,
            Sprite = Sprites.Default
        };

        public Screens Screen;
        public Backgrounds Background;
        public Sprites Sprite;

        public struct Screens
        {
            public int Width;
            public int Height;
            public float Scale;
            public static Screens Default = new Screens {
                Width = 240,
                Height = 160,
                Scale = 4
            };
        }

        public struct Backgrounds
        {
            public int Plane;
            public int MapSize;
            public int VideoRamSize;
            public static Backgrounds Default = new Backgrounds {
                Plane = 4,
                MapSize = 32,
                VideoRamSize = 256
            };
        }

        public struct Sprites
        {
            public int AttributeSize;
            public int VideoRamSize;
            public static Sprites Default = new Sprites {
                AttributeSize = 256,
                VideoRamSize = 256
            };
        }
    }
}
