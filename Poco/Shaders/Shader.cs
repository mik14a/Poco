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
        public int Handle => _Program;

        public Shader(string vertexShaderPath, string fragmentShaderPath) {
            _Program = GL.CreateProgram();
            _VertexShader = CreateShader(ShaderType.VertexShader, vertexShaderPath);
            _FragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderPath);
            GL.AttachShader(_Program, _VertexShader);
            GL.AttachShader(_Program, _FragmentShader);
            GL.LinkProgram(_Program);
            Debug.WriteLine(GL.GetProgramInfoLog(_Program));
        }

        public int CreateShader(ShaderType shaderType, string path) {
            var shader = GL.CreateShader(shaderType);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(path))
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
            GL.DeleteProgram(_Program);
            GL.DeleteShader(_VertexShader);
            GL.DeleteShader(_FragmentShader);
        }
        protected abstract void RenderImpl(Matrix4 projection);

        readonly int _Program;
        readonly int _VertexShader;
        readonly int _FragmentShader;
    }
}
