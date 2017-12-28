using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Poco.Sail.Manager
{
    public class FontManager
    {
        const string MISAKI = "Witchcraft.Core.Fonts.misaki_gothic.ttf";

        public unsafe FontManager(Background background) {
            _Background = background;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(MISAKI)) {
                using (var collection = new PrivateFontCollection()) {
                    var unmanaged = (UnmanagedMemoryStream)stream;
                    collection.AddMemoryFont((IntPtr)unmanaged.PositionPointer, (int)stream.Length);
                    _Font = new Font(collection.Families[0], 8, GraphicsUnit.Pixel);
                }
            }
        }

        public void LoadFont(int index) {
            using (var glyph = new Bitmap(8, 8, PixelFormat.Format32bppArgb))
            using (var fill = new SolidBrush(Color.Black))
            using (var draw = new SolidBrush(Color.White))
            using (var graphics = Graphics.FromImage(glyph))
            using (var ram = _Background.VideoRam.CreateGraphics()) {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                Hiragana.Concat(Katakana).Concat(Forms).ForEach((c, i) => {
                    graphics.FillRectangle(fill, 0, 0, 8, 8);
                    graphics.DrawString(c.ToString(), _Font, draw, PointF.Empty);
                    var no = i + index;
                    var size = _Background.VideoRam.Size / 8;
                    var point = new Point((no % size) * 8, (no / size) * 8);
                    ram.DrawImage(glyph, point);
                    _Characters.Add(c, no);
                });

            }
            _Background.VideoRam.Invalidate();
        }

        public void PutString(int x, int y, string value) {
            var array = value.Select(c => _Characters[c]).ToArray();
            array.ForEach((c, i) => _Background[x + i, y] = new Character() { No = c });
        }

        readonly Font _Font;
        readonly Dictionary<char, int> _Characters = new Dictionary<char, int>();
        readonly Background _Background;

        static readonly char[] Hiragana = Enumerable.Range(0x3040, 0x60).Select(r => (char)r).ToArray();
        static readonly char[] Katakana = Enumerable.Range(0x30A0, 0x60).Select(r => (char)r).ToArray();
        static readonly char[] Forms = Enumerable.Range(0xFF00, 0xA0).Select(r => (char)r).ToArray();
    }
}
