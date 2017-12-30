using System;
using System.Linq;

namespace Poco
{
    public struct MachineSettings
    {
        public static readonly MachineSettings Default = new MachineSettings {
            Lcd = new LcdSettings {
                Width = 240,
                Height = 160,
                Scale = 4
            },
            Backgrounds = new BackgroundsSettings {
                Layers = 4,
                Background = new BackgroundSettings {
                    MapSize = 32,
                    VideoRamSize = 256
                }
            },
            Sprite = new SpriteSettings {
                AttributeSize = 256,
                VideoRamSize = 256
            }
        };

        public struct LcdSettings
        {
            public int Width;
            public int Height;
            public float Scale;
        }
        public struct BackgroundsSettings
        {
            public int Layers;
            public BackgroundSettings Background;
        }
        public struct BackgroundSettings
        {
            public int MapSize;
            public int VideoRamSize;
        }
        public struct SpriteSettings
        {
            public int AttributeSize;
            public int VideoRamSize;
        }

        public LcdSettings Lcd;
        public BackgroundsSettings Backgrounds;
        public SpriteSettings Sprite;
    }
}
