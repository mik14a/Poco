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
        public Background[] Background => _Background;
        public Sprite Sprite => _Sprite;

        protected Machine(string title, MachineSettings machineSettings)
            : base((int)(machineSettings.Lcd.Width * machineSettings.Lcd.Scale),
                  (int)(machineSettings.Lcd.Height * machineSettings.Lcd.Scale),
                  title, GameWindowFlags.FixedWindow, GraphicsMode.Default, DisplayDevice.Default) {
            var scaleFactor = 1f;
            using (var graphics = Graphics.FromHwnd(WindowInfo.Handle)) {
                scaleFactor = machineSettings.Lcd.Scale * graphics.DpiX / 96f;
            }

            _Context = new GraphicsContext(GraphicsMode.Default, WindowInfo, 3, 0, GraphicsContextFlags.Default);
            _Context.MakeCurrent(WindowInfo);
            _Context.LoadAll();

            _Input = new Input();
            _Background = new Background[machineSettings.Background.Backgrounds];
            for (int i = 0; i < _Background.Length; ++i) {
                _Background[i] = new Background(machineSettings.Background);
            }
            _Sprite = new Sprite(machineSettings.Sprite);
            _Rasterizer = new Rasterizer(ClientSize.Width, ClientSize.Height, scaleFactor, _Background, _Sprite);
        }

        public void Execute() {
            EnsureUndisposed();
            if (Exists) {
                _Input.Populate();
                _Rasterizer.Rasterize();
                _Context.SwapBuffers();
            }
        }

        public override void Dispose() {
            _Context.Dispose();
            _Sprite.Dispose();
            Array.ForEach(_Background, background => background.Dispose());
            base.Dispose();
        }

        readonly GraphicsContext _Context;
        readonly Input _Input;
        readonly Background[] _Background;
        readonly Sprite _Sprite;
        readonly Rasterizer _Rasterizer;
    }
}
