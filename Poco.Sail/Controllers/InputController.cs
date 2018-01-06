using System;
using System.Linq;

namespace Poco.Sail.Controllers
{
    public class InputController : Controller
    {
        public InputController(Input input) {
            _Input = input;
        }

        public override void Update() {
            var key = _Input.Key;
            var changed = _Previous ^ key;
            _Pressed = changed & key;
            _Previous = key;
            var input = _Component.Cast<IInputComponent>().LastOrDefault();
            input?.Update(_Input.Key, _Pressed);
        }

        readonly Input _Input;
        Input.Keys _Previous = 0;
        Input.Keys _Pressed = 0;

        public interface IInputComponent
        {
            void Update(Input.Keys key, Input.Keys pressed);
        }
    }
}
