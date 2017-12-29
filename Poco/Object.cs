using System;
using System.Linq;

namespace Poco
{
    public struct Object
    {
        public enum Shapes
        {
            Square = 0,
            Horizontal = 1,
            Vertical = 2
        }

        public int Name;
        public int X;
        public int Y;
        public Shapes Shape;
        public int Size;
    }
}
