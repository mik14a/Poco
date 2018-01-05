using System;
using System.Linq;
using OpenTK.Input;

namespace Poco
{
    public class Input
    {
        public enum Keys : uint
        {
            Up = 1 << 7, Down = 1 << 6, Left = 1 << 5, Right = 1 << 4,
            Select = 1 << 3, Start = 1 << 2,
            A = 1 << 1, B = 1 << 0
        }

        public Keys Key { get; private set; } = 0;

        public void Populate() {
            var gamePad = GetGamePad(GamePad.GetState(0));
            var keyboard = GetKeyboard(Keyboard.GetState(0));
            Key = (Keys)gamePad | (Keys)keyboard;
        }

        static uint GetGamePad(GamePadState state) {
            var key = 0u;
            if (state.DPad.Up == ButtonState.Pressed) key |= (uint)Keys.Up;
            if (state.DPad.Down == ButtonState.Pressed) key |= (uint)Keys.Down;
            if (state.DPad.Left == ButtonState.Pressed) key |= (uint)Keys.Left;
            if (state.DPad.Right == ButtonState.Pressed) key |= (uint)Keys.Right;
            if (state.Buttons.A == ButtonState.Pressed) key |= (uint)Keys.A;
            if (state.Buttons.B == ButtonState.Pressed) key |= (uint)Keys.B;
            if (state.Buttons.Back == ButtonState.Pressed) key |= (uint)Keys.Select;
            if (state.Buttons.Start == ButtonState.Pressed) key |= (uint)Keys.Start;
            return key;
        }

        static uint GetKeyboard(KeyboardState state) {
            var key = 0u;
            if (state.IsKeyDown(OpenTK.Input.Key.Up)) key |= (uint)Keys.Up;
            if (state.IsKeyDown(OpenTK.Input.Key.Down)) key |= (uint)Keys.Down;
            if (state.IsKeyDown(OpenTK.Input.Key.Left)) key |= (uint)Keys.Left;
            if (state.IsKeyDown(OpenTK.Input.Key.Right)) key |= (uint)Keys.Right;
            if (state.IsKeyDown(OpenTK.Input.Key.Z)) key |= (uint)Keys.A;
            if (state.IsKeyDown(OpenTK.Input.Key.X)) key |= (uint)Keys.B;
            if (state.IsKeyDown(OpenTK.Input.Key.Escape)) key |= (uint)Keys.Select;
            if (state.IsKeyDown(OpenTK.Input.Key.Enter)) key |= (uint)Keys.Start;
            return key;
        }
    }
}
