using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;

namespace Poco.Managers
{
    public class FontManager
    {
        public unsafe FontManager(Background background) {
            _Background = background;
        }

        public void LoadFont(Font font, int index, char[] glyphs) {
            using (var image = new Bitmap(glyphs.Length * 8, 8, PixelFormat.Format32bppArgb))
                using (var brush = new SolidBrush(Color.White))
                    using (var graphics = Graphics.FromImage(image)) {
                        graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                        glyphs.ForEach((c, i) => graphics.DrawString(c.ToString(), font, brush, i * 8, 0));
                        _Background.Load(index, image);
                    }
            glyphs.ForEach((c, i) => _Characters.Add(c, i));
        }

        public void PutString(int x, int y, string value) {
            var array = value.Select(c => _Characters[c]).ToArray();
            array.ForEach((c, i) => _Background[x + i, y] =
                new Character() {
                No = c
            });
        }

        readonly Dictionary<char, int> _Characters = new Dictionary<char, int>();
        readonly Background _Background;
    }
}
