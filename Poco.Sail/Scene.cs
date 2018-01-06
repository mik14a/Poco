using System;
using System.Collections.Generic;
using System.Linq;

namespace Poco.Sail
{
    public abstract class Scene : Director.IScene
    {
        public delegate UpdateHandler UpdateHandler();

        public Director Director => _Director;

        protected Scene() {
            _Entity = new LinkedList<Entity>();
        }

        public void Update() {
            OnUpdate?.Invoke();
            var entity = _Entity.First;
            while (entity != null) {
                var next = entity.Next;
                entity.Value.Update();
                entity = next;
            }
        }

        public T Add<T>() where T : Entity {
            var entity = Activator<T>.CreateInstance();
            _Entity.AddLast(entity);
            ((IEntity)entity).Add(this);
            return entity;
        }

        public void Remove(Entity entity) {
            _Entity.Remove(entity);
            ((IEntity)entity).Remove();
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }

        protected UpdateHandler OnUpdate;

        readonly LinkedList<Entity> _Entity;
        Director _Director;

        void Director.IScene.Enter(Director director) {
            _Director = director;
            OnEnter();
        }

        void Director.IScene.Exit() {
            OnExit();
            _Entity.Clear();
            _Director = null;
        }

        internal interface IEntity
        {
            void Add(Scene scene);
            void Remove();
        }
    }
}
