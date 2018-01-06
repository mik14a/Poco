using System;
using System.Linq;

namespace Poco.Sail
{
    public abstract class Component : Entity.IComponent
    {
        protected Component() {
        }

        protected virtual void OnAttach() { }

        protected virtual void OnDetach() { }

        Entity _Entity;

        void Entity.IComponent.Attach(Entity entity) {
            _Entity = entity;
            OnAttach();
        }

        void Entity.IComponent.Detach() {
            OnDetach();
            _Entity = null;
        }
    }
}
