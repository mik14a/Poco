using System;
using System.Collections.Generic;
using System.Linq;

namespace Poco.Sail
{
    public class TaskManager : IExecutable, IDrawable
    {
        public TaskManager() {
            _List = new LinkedList<Task>();
        }

        public void Execute() {
            _List.ForEach(task => task.Execute());
        }

        public void Draw() {
            _List.ForEach(task => task.Draw());
        }

        readonly LinkedList<Task> _List;
    }
}
