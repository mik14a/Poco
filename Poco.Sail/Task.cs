using System;
using System.Linq;
using Poco.Sail.Interface;

namespace Poco.Sail
{
    public class Task : IExecutable, IDrawable
    {
        protected delegate ExecuteHandler ExecuteHandler();
        protected delegate void DrawHandler();

        public void Execute() {
            OnExecute = OnExecute?.Invoke();
        }

        public void Draw() {
            OnDraw?.Invoke();
        }

        protected ExecuteHandler OnExecute;
        protected DrawHandler OnDraw;
    }
}
