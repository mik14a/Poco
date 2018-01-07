using System;
using System.Drawing;
using System.Linq;
using Poco.Controllers;

namespace Poco.Components
{
    public class BackgroundComponent : Component, BackgroundController.IBackgroundComponent
    {
        public int Layer {
            get { return _Layer; }
            set {
                if (_Layer == value) return;
                _Layer = value;
                _IsDirty = true;
            }
        }

        public Rectangle Rectangle {
            get { return _Rectangle; }
            set {
                if (_Rectangle == value) return;
                _Rectangle = value;
                _Map = new Character[_Rectangle.Width * _Rectangle.Height];
                _IsDirty = true;
            }
        }

        public ref Character this[int x, int y] {
            get {
                var index = x + y * _Rectangle.Width;
                return ref _Map[index];
            }
        }

        protected BackgroundComponent() { }

        public void MakeRenderDirty() {
            _IsDirty = true;
        }

        bool _IsDirty;
        int _Layer;
        Rectangle _Rectangle;
        Character[] _Map;

        bool BackgroundController.IBackgroundComponent.IsDirty {
            get { return _IsDirty; }
            set { _IsDirty = value; }
        }
    }
}
