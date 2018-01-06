using System;
using System.Linq;

namespace Poco.Sail.Controllers
{
    public class InputController : Controller
    {
        public InputController(Input input) {
            _Input = input;
        }

        public bool Key(Input.Keys keys) {
            return (_Input.Key & keys) != 0;
        }

        public bool Pressed(Input.Keys keys) {
            return (_Pressed & keys) != 0;
        }

        public override void Update() {
            var key = _Input.Key;
            var changed = _Previous ^ key;
            _Pressed = changed & key;
            _Previous = key;
            var input = _Component.Last?.Value as IInputComponent;
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
