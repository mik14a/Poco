using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public class VideoRam : IDisposable
    {
        public int Texture => _Texture;

        public int Size => _Size;

        public VideoRam(int size) {
            _Size = size;
            _Bitmap = new Bitmap(_Size, _Size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            _Texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _Texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            InvalidateTexture();
        }

        public Graphics CreateGraphics() {
            return Graphics.FromImage(_Bitmap);
        }

        public void InvalidateTexture() {
            var bitmapData = LockBits(_Bitmap);
            TexImage2D(bitmapData.Width, bitmapData.Height, bitmapData.Scan0);
            UnlockBits(_Bitmap, bitmapData);
        }

        public void Dispose() {
            _Bitmap.Dispose();
            GL.DeleteTexture(_Texture);
            GC.SuppressFinalize(this);
        }

        readonly int _Size;
        readonly Bitmap _Bitmap;
        int _Texture;

        static BitmapData LockBits(Bitmap bitmap) {
            var rectangle = new Rectangle(Point.Empty, bitmap.Size);
            var flags = ImageLockMode.ReadOnly;
            var format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            return bitmap.LockBits(rectangle, flags, format);
        }

        static void TexImage2D(int width, int height, IntPtr pixels) {
            var target = TextureTarget.Texture2D;
            var internalFormat = PixelInternalFormat.Rgba;
            var format = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
            var type = PixelType.UnsignedByte;
            GL.TexImage2D(target, 0, internalFormat, width, height, 0, format, type, pixels);
        }

        static void UnlockBits(Bitmap bitmap, BitmapData bitmapData) {
            bitmap.UnlockBits(bitmapData);
        }

    }
}
