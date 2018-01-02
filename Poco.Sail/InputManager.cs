using System;
using System.Linq;

namespace Poco.Sail
{
    public class InputManager
    {
        public Input.Keys Key => _Input.Key;
        public Input.Keys Pressed => _Pressed;

        public InputManager(Input input) {
            _Input = input;
        }
        public void Execute() {
            var key = _Input.Key;
            var changed = _Previous ^ key;
            _Pressed = changed & key;
            _Previous = key;
        }

        public bool IsKey(Input.Keys keys) {
            return (_Input.Key & keys) != 0;
        }

        public bool IsPressed(Input.Keys keys) {
            return (_Pressed & keys) != 0;
        }

        readonly Input _Input;
        Input.Keys _Previous = 0;
        Input.Keys _Pressed = 0;
    }
}
