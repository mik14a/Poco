using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Poco
{
    public class Sprite : IDisposable
    {
        public Sprite(int videoRamSize) {
            _VideoRam = new VideoRam(videoRamSize);
        }

        public void Draw() {
            //throw new NotImplementedException();
        }

        public void Dispose() {
            _VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }


        readonly VideoRam _VideoRam;
    }
}
