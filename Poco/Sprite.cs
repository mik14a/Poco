using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Poco
{
    /// <summary>
    /// Virtual sprite memory.
    /// </summary>
    public sealed  partial class Sprite : IEnumerable<Sprite.Attribute>, IDisposable
    {
        /// <summary>
        /// Create virtual sprite memory.
        /// </summary>
        /// <param name="sprites"></param>
        /// <returns></returns>
        public static Sprite Create(Settings.Sprites sprites) {
            return new Sprite(sprites.AttributeSize, sprites.VideoRamSize);
        }

        /// <summary>
        /// Get attribute size.
        /// </summary>
        public int Size { get { return _Attributes.Length; } }

        /// <summary>
        /// Get sprite attribute.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref Attribute this[int index] {
            get { return ref _Attributes[index]; }
        }

        /// <summary>
        /// Get sprite video ram.
        /// </summary>
        public VideoRam VideoRam { get; }

        /// <summary>
        /// Construct virtual sprite memory with attribute and vram size.
        /// </summary>
        /// <param name="attributeSize"></param>
        /// <param name="videoRamSize"></param>
        Sprite(int attributeSize, int videoRamSize) {
            _Attributes = new Attribute[attributeSize];
            VideoRam = new VideoRam(videoRamSize);
        }

        /// <summary>
        /// Load image to video ram.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="image"></param>
        public void Load(int index, Image image) {
            VideoRam.Load(index, image);
        }

        /// <summary>
        /// Reset attributes.
        /// </summary>
        public void Reset() {
            Array.Clear(_Attributes, 0, _Attributes.Length);
        }

        public IEnumerator<Attribute> GetEnumerator() {
            return ((IEnumerable<Attribute>)_Attributes).GetEnumerator();
        }

        public void Dispose() {
            VideoRam.Dispose();
            GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Attributes.GetEnumerator();
        }

        readonly Attribute[] _Attributes;
    }
}
