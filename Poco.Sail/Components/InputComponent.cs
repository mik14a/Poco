using System;
using System.Linq;
using Poco.Controllers;

namespace Poco.Components
{
    public class InputComponent : Component, InputController.IInputComponent
    {
        protected InputComponent() {
        }

        public bool Key(Input.Keys keys) {
            return (_Key & keys) != 0;
        }

        public bool Pressed(Input.Keys keys) {
            return (_Pressed & keys) != 0;
        }

        Input.Keys _Key = 0;
        Input.Keys _Pressed = 0;

        void InputController.IInputComponent.Update(Input.Keys key, Input.Keys pressed) {
            _Key = key;
            _Pressed = pressed;
        }
    }
}
