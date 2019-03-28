using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    public static class ExtensionGL
    {
        public static string LoadShader(string filename, ShaderType type, int program, out int id)
        {
            id = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
                GL.ShaderSource(id, sr.ReadToEnd());
            GL.CompileShader(id);
            GL.AttachShader(program, id);
            return GL.GetShaderInfoLog(id);
        }


        public static float Radians(float deg)
        {
            return (float)((deg / 180) * Math.PI);
        }

        public static float Degree(float rad)
        {
            return (float)((rad / Math.PI) * 180);
        }
    }
}
