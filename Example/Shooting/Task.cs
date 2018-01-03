using System;
using System.Linq;
using Poco;

namespace Shooting
{
    public class Task
    {
        protected delegate ExecuteHandler ExecuteHandler();
        protected delegate Poco.Object DrawHandler();

        public void Execute() {
            OnExecute = OnExecute?.Invoke();
        }

        public Poco.Object? Draw() {
            return OnDraw?.Invoke();
        }

        protected ExecuteHandler OnExecute;
        protected DrawHandler OnDraw;

        Task _Previous = null;
        Task _Next = null;

        static Task() {
            _Head._Previous = _Head._Next = _Head;
        }

        public static void Initialize(Sprite sprite) {
            _Sprite = sprite;
        }

        public static void Synchronize() {
            var task = Begin();
            while (task != _Head) {
                var next = task._Next;
                task.Execute();
                task = next;
            }
        }

        public static void Invalidate() {
            var index = 0;
            var task = Begin();
            while (task != _Head) {
                var next = task._Next;
                var @object = task.Draw();
                if (@object.HasValue) {
                    _Sprite[index++] = @object.Value;
                }
                task = next;
            }
        }

        public static Task Begin() {
            return _Head._Next;
        }

        public static Task End() {
            return _Head._Previous;
        }

        public static void Add(Task task) {
            Add(End(), task);
        }

        public static void Add(Task previous, Task task) {
            task._Previous = previous;
            task._Next = previous._Next;
            previous._Next = previous._Next._Previous = task;
        }

        public static void Remove(Task task) {
            task._Previous._Next = task._Next;
            task._Next._Previous = task._Previous;
            task._Previous = task._Next = null;
        }

        static Sprite _Sprite;
        readonly static Task _Head = new Task();

    }
}
