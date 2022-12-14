using System;
using System.Collections.Generic;
using System.Linq;

namespace Poco
{
    public abstract class Entity : Scene.IEntity
    {
        public delegate UpdateHandler UpdateHandler();

        public Scene Scene { get; private set; }

        protected Entity() {
            _Component = new LinkedList<Component>();
        }

        public void Update() {
            OnUpdate = OnUpdate?.Invoke();
        }

        public T Attach<T>()
            where T : Component {
            var component = Activator<T>.CreateInstance();
            _Component.AddLast(component);
            ((IComponent)component).Attach(this);
            return component;
        }

        public void Detach(Component component) {
            ((IComponent)component).Detach();
            _Component.Remove(component);
        }

        protected virtual void OnAdd() {
        }
        protected virtual void OnRemove() {
        }

        protected UpdateHandler OnUpdate;

        readonly LinkedList<Component> _Component;

        void Scene.IEntity.Add(Scene scene) {
            Scene = scene;
            OnAdd();
        }

        void Scene.IEntity.Remove() {
            OnRemove();
            _Component.Clear();
            Scene = null;
        }

        internal interface IComponent
        {
            void Attach(Entity entity);

            void Detach();
        }
    }
}
