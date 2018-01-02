using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Poco.Sail
{
    public class FontManager
    {
        //const string MISAKI = "Witchcraft.Core.Fonts.misaki_gothic.ttf";

        public unsafe FontManager(Background background) {
            _Background = background;
            //var assembly = Assembly.GetExecutingAssembly();
            //using (var stream = assembly.GetManifestResourceStream(MISAKI)) {
            //    using (var collection = new PrivateFontCollection()) {
            //        var unmanaged = (UnmanagedMemoryStream)stream;
            //        collection.AddMemoryFont((IntPtr)unmanaged.PositionPointer, (int)stream.Length);
            //        _Font = new Font(collection.Families[0], 8, GraphicsUnit.Pixel);
            //    }
            //}
        }

        public void LoadFont(int index) {
            var font = new Font("MS Gothic", 4);
            var glyphs = Hiragana.Concat(Katakana).Concat(Forms).ToArray();
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
            array.ForEach((c, i) => _Background[x + i, y] = new Character() { No = c });
        }

        readonly Dictionary<char, int> _Characters = new Dictionary<char, int>();
        readonly Background _Background;

        static readonly char[] Hiragana = Enumerable.Range(0x3040, 0x60).Select(r => (char)r).ToArray();
        static readonly char[] Katakana = Enumerable.Range(0x30A0, 0x60).Select(r => (char)r).ToArray();
        static readonly char[] Forms = Enumerable.Range(0xFF00, 0xA0).Select(r => (char)r).ToArray();
    }
}
