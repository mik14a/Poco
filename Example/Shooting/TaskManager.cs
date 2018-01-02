using System;
using System.Collections.Generic;
using System.Linq;
using Poco;

namespace Shooting
{
    public class TaskManager
    {
        public TaskManager(Sprite sprite) {
            _Sprite = sprite;
            _Head._Previous = _Head._Next = _Head;
        }

        public void Execute() {
            var task = Begin();
            while (task != _Head) {
                task.Execute();
                task = task._Next;
            }
        }

        public void Draw() {
            var index = 0;
            var task = Begin();
            while (task != _Head) {
                var @object = task.Draw();
                if (@object.HasValue) {
                    _Sprite[index++] = @object.Value;
                }
                task = task._Next;
            }
        }

        public Task Begin() {
            return _Head._Next;
        }

        public Task End() {
            return _Head._Previous;
        }

        public void Add(Task task) {
            Add(End(), task);
        }

        public void Add(Task previous, Task task) {
            task._Previous = previous;
            task._Next = previous._Next;
            previous._Next = previous._Next._Previous = task;
        }

        public void Remove(Task task) {
            task._Previous._Next = task._Next;
            task._Next._Previous = task._Previous;
            task._Previous = task._Next = null;
        }

        readonly Sprite _Sprite;
        readonly Task _Head = new Task();
    }
}
