using System;
using System.Drawing;
using System.Linq;
using Poco.Sail.Controllers;

namespace Poco.Sail.Components
{
    class BackgroundComponent : Component, BackgroundController.IBackgroundComponent
    {
        public bool IsDirty => throw new NotImplementedException();

        public int Layer => throw new NotImplementedException();

        public Rectangle Rectangle => throw new NotImplementedException();

        public Character[] Map => throw new NotImplementedException();
    }
}
