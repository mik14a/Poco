using System;
using System.Linq;

namespace Poco
{
    public struct MachineSettings
    {
        public static readonly MachineSettings Default = new MachineSettings {
            Lcd = LcdSettings.Default,
            Backgrounds = BackgroundsSettings.Default,
            Sprite = SpriteSettings.Default
        };

        public struct LcdSettings
        {
            public int Width;
            public int Height;
            public float Scale;
            public static LcdSettings Default = new LcdSettings {
                Width = 240,
                Height = 160,
                Scale = 4
            };
        }
        public struct BackgroundsSettings
        {
            public int Layers;
            public BackgroundSettings Background;
            public static BackgroundsSettings Default = new BackgroundsSettings {
                Layers = 4,
                Background = BackgroundSettings.Default
            };
        }
        public struct BackgroundSettings
        {
            public int MapSize;
            public int VideoRamSize;
            public static BackgroundSettings Default = new BackgroundSettings {
                MapSize = 32,
                VideoRamSize = 256
            };
        }

        public struct SpriteSettings
        {
            public int AttributeSize;
            public int VideoRamSize;
            public static SpriteSettings Default = new SpriteSettings {
                AttributeSize = 256,
                VideoRamSize = 256
            };
        }

        public LcdSettings Lcd;
        public BackgroundsSettings Backgrounds;
        public SpriteSettings Sprite;
    }
}
