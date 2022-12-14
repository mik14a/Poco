using System;
using System.Drawing;
using System.Linq;
using Poco.Controllers;

namespace Poco.Components
{
    public class BackgroundComponent : Component, BackgroundController.IBackgroundComponent
    {
        public bool IsDirty { get; set; } = false;

        public int Plane {
            get { return _Plane; }
            set
            {
                if (_Plane == value)
                    return;
                _Plane = value;
                IsDirty = true;
            }
        }

        public Rectangle Rectangle {
            get { return _Rectangle; }
            set
            {
                if (_Rectangle == value)
                    return;
                _Rectangle = value;
                _Map = new Background.Character[_Rectangle.Width * _Rectangle.Height];
                IsDirty = true;
            }
        }


        public ref Background.Character this[int x, int y] {
            get
            {
                var index = x + y * _Rectangle.Width;
                return ref _Map[index];
            }
        }

        protected BackgroundComponent() {
        }

        public void MakeRenderDirty() {
            IsDirty = true;
        }

        int _Plane;
        Rectangle _Rectangle;
        Background.Character[] _Map;
    }
}
