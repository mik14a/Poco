using System;
using System.Collections.Generic;
using System.Linq;

namespace Poco.Sail
{
    public abstract class Controller
    {
        public Controller() {
            _Component = new LinkedList<Component>();
        }

        public abstract void Update();

        public void Add(Component component) {
            _Component.AddLast(component);
        }

        public void Remove(Component component) {
            _Component.Remove(component);
        }

        protected readonly LinkedList<Component> _Component;
    }
}
