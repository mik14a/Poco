using System;
using System.Linq;

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

        internal Task _Previous = null;
        internal Task _Next = null;
    }
}
