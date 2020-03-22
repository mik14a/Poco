using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Poco.Shaders
{
    abstract class Shader : IDisposable
    {
        public int Handle { get; }

        public Shader(string vertexShaderPath, string fragmentShaderPath) {
            Handle = GL.CreateProgram();
            _VertexShader = CreateShader(ShaderType.VertexShader, vertexShaderPath);
            _FragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderPath);
            GL.AttachShader(Handle, _VertexShader);
            GL.AttachShader(Handle, _FragmentShader);
            GL.LinkProgram(Handle);
            Debug.WriteLine(GL.GetProgramInfoLog(Handle));
        }

        public int CreateShader(ShaderType shaderType, string name) {
            var shader = GL.CreateShader(shaderType);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(name))
                using (var reader = new StreamReader(stream)) {
                    GL.ShaderSource(shader, reader.ReadToEnd());
                }
            GL.CompileShader(shader);
            Debug.WriteLine(GL.GetShaderInfoLog(shader));
            return shader;
        }

        public void Render(Matrix4 projection) {
            RenderImpl(projection);
        }

        public void Dispose() {
            GL.DeleteProgram(Handle);
            GL.DeleteShader(_VertexShader);
            GL.DeleteShader(_FragmentShader);
            GC.SuppressFinalize(this);
        }

        protected abstract void RenderImpl(Matrix4 projection);

        readonly int _VertexShader;
        readonly int _FragmentShader;
    }
}
