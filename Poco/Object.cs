using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
