using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using OpenTK.Input;

namespace Render3DObject.Components
{
    public class MainWindow : GameWindow
    {
        public MainWindow() :
            base(1280, 720,
                GraphicsMode.Default,
                "OpenGL with OpenTK Tutorial",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4, 0,
                GraphicsContextFlags.ForwardCompatible)
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var vertices = ShapeFactory.CreateQueenMap();
                //ShapeFactory.CreateHeightMap();
                //ShapeFactory.CreateSolidCube(.2f, Color4.LightGreen);
            renderObjs.Add(new RenderObject(vertices));
            programID = createProgram();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.Enable(EnableCap.DepthTest);
            Closed += MainWindow_Closed;
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
        }
     
        public override void Exit()
        {
            Console.WriteLine("Exit called");
            foreach (var obj in renderObjs)
                obj.Dispose();
            GL.DeleteProgram(programID);
            base.Exit();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            handleMouse();
            handleKeyboard();

            dtime += e.Time;
            var r1 = Matrix4.CreateRotationX(objRotate.Pitch);
            var r2 = Matrix4.CreateRotationY(objRotate.Yaw);
            var r3 = Matrix4.CreateRotationZ(objRotate.Roll);
            modelView = r1 * r2 * r3;

            modelView *= camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.0f, ClientSize.Width / (float)ClientSize.Height, 0.1f, 1000.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
            CursorVisible = false;
            Color4 backColor = new Color4 { A = 1.0f, B = 0.3f, G = 0.1f, R = 0.1f };
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(programID);
            GL.UniformMatrix4(20,   // match the layout location in the shader
                false,              // transpose
                ref modelView);     // our matrix

            foreach (var renderObject in renderObjs)
                renderObject.Render();

            SwapBuffers();
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            if (Focused)
                resetCursor();
        }

        #region Private functions
        int createProgram()
        {
            var program = GL.CreateProgram();
            var shaders = new List<int>() {
                compileShader(ShaderType.VertexShader, vertexShaderSource),
                compileShader(ShaderType.FragmentShader, fragmentShaderSource)
            };

            foreach (var shader in shaders)
                GL.AttachShader(program, shader);
            GL.LinkProgram(program);
            var info = GL.GetProgramInfoLog(program);
            if (!string.IsNullOrWhiteSpace(info))
                Console.WriteLine($"GL.LinkProgram had info log: {info}");

            foreach (var shader in shaders)
            {
                GL.DetachShader(program, shader);
                GL.DeleteShader(shader);
            }

            return program;
        }

        int compileShader(ShaderType shaderType, string source)
        {
            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(info))
                Console.WriteLine($"GL.CompileShader[{shaderType}] had info log: {info}");
            return shader;
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            Exit();
        }

        void MainWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            key = Key.Unknown;
        }

        void MainWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            key = e.Key;
        }

        void resetCursor()
        {
            Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            var state = Mouse.GetState();
            preMousePos = new Vector2(state.X, state.Y);
        }

        void handleKeyboard()
        {
            switch (key)
            {
                case Key.Unknown:
                default:
                    break;
                case Key.W:
                    camera.Move(0, 1f, 0);
                    break;
                case Key.S:
                    camera.Move(0, -1f, 0);
                    break;
                case Key.A:
                    camera.Move(-1f, 0, 0);
                    break;
                case Key.D:
                   camera.Move(1f, 0, 0);
                    break;
                case Key.Q:
                    camera.Move(0, 0, 1f);
                    break;
                case Key.E:
                    camera.Move(0, 0, -1f);
                    break;
                case Key.T:
                    objRotate.Pitch += .01f;
                    break;
                case Key.G:
                    objRotate.Pitch -= .01f;
                    break;
                case Key.Y:
                    objRotate.Roll += .01f;
                    break;
                case Key.H:
                    objRotate.Roll -= .01f;
                    break;
                case Key.U:
                    objRotate.Yaw += .01f;
                    break;
                case Key.J:
                    objRotate.Yaw -= .01f;
                    break;
                case Key.Escape:
                    Exit();
                    break;
                case Key.F:
                    WindowState = WindowState.Fullscreen;
                    break;
                case Key.N:
                    WindowState = WindowState.Normal;
                    break;

            }
        }

        private void handleMouse()
        {
            if (!Focused)
                return;
            
            var mouseState = Mouse.GetState();
            var delta = preMousePos - new Vector2(mouseState.X, mouseState.Y);
            if(delta.X != 0 || delta.Y != 0)
                camera.AddRotation(delta.X, delta.Y);
            resetCursor();
        }

        #endregion

        #region Shaders
        private const string vertexShaderSource = @"#version 450 core
            layout (location = 0) in vec4 position;
            layout(location = 1) in vec4 color;

            out vec4 vs_color;

            layout (location = 20) uniform  mat4 modelView;

            void main(void)
            {
             gl_Position = modelView * position;
             vs_color = color;
            }";

        private const string fragmentShaderSource = @"#version 450 core
            in vec4 vs_color;
            out vec4 color;

            void main(void)
            {
                color = vs_color;
            }";

        #endregion

        #region Fields
        private int programID;
        private List<RenderObject> renderObjs = new List<RenderObject>();
        private double dtime;
        private Matrix4 modelView;
        private Camera camera = new Camera();
        private Key key;
        private Vector2 preMousePos;
        private EulerAngles objRotate = new EulerAngles();
        #endregion
    }

}
