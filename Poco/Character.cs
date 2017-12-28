using System;
using System.Linq;

namespace Poco
{
    public struct Character
    {
        [Flags]
        public enum Rotates
        {
            RotateNone = 0,
            Rotate0 = 0x00,
            Rotate90 = 0x01,
            Rotate180 = 0x10,
            Rotate270 = 0x11
        }
        [Flags]
        public enum Flips
        {
            FlipNone = 0,
            FlipHorizontal = 0x01,
            FlipVertical = 0x10
        }

        public int No;
        public Rotates Rotate;
        public Flips Flip;
    }
}
