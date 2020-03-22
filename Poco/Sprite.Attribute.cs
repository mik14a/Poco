using System.Drawing;

namespace Poco
{
    public sealed  partial class Sprite
    {
        /// <summary>
        /// Sprite attribute structure.
        /// </summary>
        public struct Attribute
        {
            public bool Enable;
            public int Name;
            public int Priority;
            public int X;
            public int Y;
            public Size Size;
        }
    }
}
