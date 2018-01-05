using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public class VideoRam : IDisposable
    {
        public int Size => _Size;
        public int Texture => _Texture;
        public int Sampler => _Sampler;

        public VideoRam(int size) {
            _Size = size;
            _Bitmap = new Bitmap(_Size, _Size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            _Texture = GL.GenTexture();
            _Sampler = GL.GenSampler();
            GL.BindTexture(TextureTarget.Texture2D, _Texture);
            TexImage2D(_Size, _Size, IntPtr.Zero);
            GL.SamplerParameter(_Sampler, SamplerParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.SamplerParameter(_Sampler, SamplerParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public void Load(int index, Image image) {
            var srcStride = image.Width / 8;
            var destStride = Size / 8;
            var srcCount = srcStride * image.Height / 8;
            using (var ram = CreateGraphics()) {
                for (int srcIndex = 0, destIndex = index; srcIndex < srcCount; ++srcIndex, ++destIndex) {
                    var srcRect = new Rectangle((srcIndex % srcStride) * 8, srcIndex / srcStride * 8, 8, 8);
                    var destRect = new Rectangle((destIndex % destStride) * 8, destIndex / destStride * 8, 8, 8);
                    ram.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            Invalidate();
        }

        public void Dispose() {
            _Bitmap.Dispose();
            GL.DeleteTexture(_Texture);
            GC.SuppressFinalize(this);
        }

        Graphics CreateGraphics() {
            return Graphics.FromImage(_Bitmap);
        }

        void Invalidate() {
            GL.BindTexture(TextureTarget.Texture2D, _Texture);
            var bitmapData = LockBits(_Bitmap);
            TexSubImage2D(bitmapData.Width, bitmapData.Height, bitmapData.Scan0);
            UnlockBits(_Bitmap, bitmapData);
        }

        readonly int _Size;
        readonly Bitmap _Bitmap;
        readonly int _Texture;
        readonly int _Sampler;

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

        static void TexSubImage2D(int width, int height, IntPtr pixels) {
            var target = TextureTarget.Texture2D;
            var format = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
            var type = PixelType.UnsignedByte;
            GL.TexSubImage2D(target, 0, 0, 0, width, height, format, type, pixels);
        }

        static void UnlockBits(Bitmap bitmap, BitmapData bitmapData) {
            bitmap.UnlockBits(bitmapData);
        }

    }
}
