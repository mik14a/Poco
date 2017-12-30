using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;

namespace Poco
{
    public class Machine : NativeWindow
    {
        public static Machine Create(string title) {
            return new Machine(title, MachineSettings.Default);
        }
        public static Machine Create(string title, MachineSettings machineSettings) {
            return new Machine(title, machineSettings);
        }

        public Input Input => _Input;
        public Sprite Sprite => _Sprite;
        public Backgrounds Backgrounds => _Backgrounds;


        protected Machine(string title, MachineSettings machineSettings)
            : base((int)(machineSettings.Lcd.Width * machineSettings.Lcd.Scale),
                  (int)(machineSettings.Lcd.Height * machineSettings.Lcd.Scale),
                  title, GameWindowFlags.FixedWindow, GraphicsMode.Default, DisplayDevice.Default) {
            var scaleFactor = 1f;
            using (var graphics = Graphics.FromHwnd(WindowInfo.Handle)) {
                scaleFactor = machineSettings.Lcd.Scale * graphics.DpiX / 96f;
            }

            _Context = new GraphicsContext(GraphicsMode.Default, WindowInfo, 1, 0, GraphicsContextFlags.Default);
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            _Input = new Input();
            _Backgrounds = new Backgrounds(machineSettings.Backgrounds);
            _Sprite = new Sprite(machineSettings.Sprite);
            _Rasterizer = new Rasterizer(ClientSize.Width, ClientSize.Height, scaleFactor);
        }

        public void Execute() {
            EnsureUndisposed();
            if (Exists) {
                _Input.Populate();
                _Rasterizer.Rasterize(_Sprite, _Backgrounds);
                _Context.SwapBuffers();
            }
        }

        public override void Dispose() {
            _Context.Dispose();
            _Sprite.Dispose();
            _Backgrounds.Dispose();
            base.Dispose();
        }

        readonly GraphicsContext _Context;
        readonly Input _Input;
        readonly Backgrounds _Backgrounds;
        readonly Sprite _Sprite;
        readonly Rasterizer _Rasterizer;
    }
}
