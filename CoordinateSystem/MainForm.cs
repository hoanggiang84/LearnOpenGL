using OpenGLUtilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoordinateSystem
{
    public partial class MainForm : Form
    {
        private const float SCALE_FACTOR = 5.0f;
        private const float FOV = 45.0f;
        private const float ZNEAR = 0.1f;
        private const float ZFAR = 100.0f;
        private float aspect;
        private int pgmID;
        private int vsID;
        private int fsID;
        private int attribute_vpos;
        private int attribute_vcol;
        private int uniform_mview;
        private int vbo_position;
        private int vbo_color;
        private int vbo_mview;
        private int ibo_elements;

        private int[] indicedata;

        private Vector3[] vertdata;
        private Vector3[] coldata;
        private Matrix4[] mviewdata;

        private List<Volume> objs = new List<Volume>();

        private Camera camera = new Camera();
        private Quaternion rotate = Quaternion.Identity;
        private Matrix4 transform = Matrix4.Identity;
        private Point startPoint;
        private List<Transform3D> transforms = new List<Transform3D>();
        private Transform3D currentTrans = new TranslateTransform3D();
        private Quaternion rotation;
        private Cursor rotateCursor;
        private Cursor scaleCursor;
        private Cursor translateCursor;

        private Keys clickedKey = Keys.None;
        private ChangeMode changeMode = ChangeMode.None;
        private Vector3 startCickedPosition;

        public MainForm()
        {
            InitializeComponent();
        }

        private void panelOpenGL_Paint(object sender, PaintEventArgs e)
        {
            GL.Viewport(panelOpenGL.ClientRectangle);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);

            int indiceat = 0;
            foreach (Volume v in objs)
            {
                GL.UniformMatrix4(uniform_mview, false, ref v.ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(int));
                indiceat += v.IndiceCount;
            }
            GL.PopMatrix();
            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);

            GL.Flush();

            panelOpenGL.SwapBuffers();
        }

        private void timerUpdateFrame_Tick(object sender, EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.UseProgram(pgmID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            var perspectiveFov = Matrix4.CreatePerspectiveFieldOfView(ExtensionGL.Radians(FOV), aspect, ZNEAR, ZFAR); 
            foreach (Volume v in objs)
            {
                v.ViewProjectionMatrix = camera.GetViewMatrix() * perspectiveFov;
                v.ModelViewProjectionMatrix = transform * v.ModelMatrix * v.ViewProjectionMatrix ;
            }
            panelOpenGL.Invalidate();
        }

        private void initProgram()
        {
            camera.Position = new Vector3(0f, 0f, 1f);
            pgmID = GL.CreateProgram();
            ExtensionGL.LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            ExtensionGL.LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            attribute_vpos = GL.GetAttribLocation(pgmID, "vPos");
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelView");
            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                MessageBox.Show("Error binding attributes [vPos vColor modelView]" +
                                $"= [{attribute_vpos} {attribute_vcol} {uniform_mview}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
            GL.GenBuffers(1, out ibo_elements);
        }

        private void panelOpenGL_Load(object sender, EventArgs e)
        {
            translateCursor = Cursors.SizeAll;
            var rotateBmp = new Bitmap(new Bitmap("Icons/rotate.png"), 32, 32);
            rotateCursor = new Cursor(rotateBmp.GetHicon());
            var zoomBmp = new Bitmap(new Bitmap("Icons/zoom.png"), 16, 16);
            scaleCursor = new Cursor(zoomBmp.GetHicon());
            aspect = panelOpenGL.ClientRectangle.Width / (float)panelOpenGL.ClientRectangle.Height;

            initProgram();
            objs = new List<Volume> { new Cube() };
            transforms.Add(new ScaleTransform3D(new Vector3(0.3f)));
            transforms.Add(new TranslateTransform3D(new Vector3()));
            updateModelMatricies();
            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            int vertcount = 0;
            foreach (Volume v in objs)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                vertcount += v.VertCount;
            }

            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            mviewdata = new Matrix4[] { Matrix4.Identity };

            GL.ClearColor(Color.CornflowerBlue);
        }

        private void panelOpenGL_Resize(object sender, EventArgs e)
        {
            GL.Viewport(panelOpenGL.ClientRectangle);
            aspect = panelOpenGL.ClientSize.Width / (float)panelOpenGL.ClientSize.Height;
            var projection = Matrix4.CreatePerspectiveFieldOfView(ExtensionGL.Radians(FOV), aspect, ZNEAR, ZFAR);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        private void panelOpenGL_KeyDown(object sender, KeyEventArgs e)
        {
            if (changeMode != ChangeMode.None)
                return;
                
            if (e.Control)
            {
                clickedKey = Keys.Control;
                Cursor = translateCursor;
            }
            else if (e.Shift)
            {
                clickedKey = Keys.Shift;
                Cursor = scaleCursor;
            }
        }

        private void panelOpenGL_MouseDown(object sender, MouseEventArgs e)
        {
            if (clickedKey == Keys.Control)
            {
                changeMode = ChangeMode.Translate;
                startCickedPosition = getPointerPositionOnObjectPlane(e.Location);
            }
            else if (clickedKey == Keys.Shift)
            {
                changeMode = ChangeMode.Scale;
            }
            else
            {
                changeMode = ChangeMode.Rotate;
                Cursor = rotateCursor;
            }

            startPoint = e.Location;
        }

        private void panelOpenGL_MouseUp(object sender, MouseEventArgs e)
        {
            disableChangeObject();
        }

        private void disableChangeObject()
        {
            Cursor = Cursors.Default;
            clickedKey = Keys.None;
            changeMode = ChangeMode.None;
            var t = currentTrans.Clone() as Transform3D;
            if(t != null)
                transforms.Add(t);
            currentTrans = new TranslateTransform3D();
        }

        private void panelOpenGL_MouseLeave(object sender, EventArgs e)
        {
            disableChangeObject();
        }

        private void panelOpenGL_MouseMove(object sender, MouseEventArgs e)
        {
            switch(changeMode)
            {
                case ChangeMode.None:
                default:
                    break;

                case ChangeMode.Rotate:
                    getRotation(e.Location);
                    break;
                case ChangeMode.Scale:
                    getScale(e.Location);
                    break;
                case ChangeMode.Translate:
                    //getRotateAroundCamera(e.Location);
                    getTranslation(e.Location);
                    break;
            }
        }

        private void getTranslation(Point endPoint)
        {
            var endClickedPosition = getPointerPositionOnObjectPlane(endPoint);
            var dv = endClickedPosition - startCickedPosition;
            if(dv.Length > float.Epsilon)
            {
                currentTrans = new TranslateTransform3D(dv);
                updateModelMatricies();
            }
        }

        private Vector3 getPointerPositionOnObjectPlane(Point endPoint)
        {
            // 3D line equation by 2 points camera position "Vc" and near plane position "Vn" (from mouse position) 
            // V = Vc + t*(a,b,c)
            // with (a,b,c) = Vn - Vc;
            float adjustPos = (float)Math.Tan(ExtensionGL.Radians(FOV / 2.0f)) * 0.1f / (panelOpenGL.Height / 2.0f);
            float dx = (endPoint.X - panelOpenGL.Width / 2);
            float dy = (endPoint.Y - panelOpenGL.Height / 2);
            float xn = dx * adjustPos;
            float yn = -dy * adjustPos;
            Vector3 vn = new Vector3(xn, yn, camera.Position.Z - ZNEAR);
            Vector3 vc = camera.Position;
            Vector3 abc = vn - vc;

            Matrix4 tmpTrans = getPreviousTransform();
            Vector3 objectOrigin = tmpTrans.ExtractTranslation();
            Vector3 v = new Vector3();
            v.Z = objectOrigin.Z;

            float t = (v.Z - vc.Z) / abc.Z;
            v.X = t * abc.X;
            v.Y = t * abc.Y;
            return v;
        }

        private Matrix4 getPreviousTransform()
        {
            Matrix4 preTrans = Matrix4.Identity;
            foreach (var tr in transforms)
                preTrans *= tr.Value;
            return preTrans;
        }

        private void getRotateAroundCamera(Point endPoint)
        {
            float adjustPos = (float)Math.Tan(ExtensionGL.Radians(FOV / 2.0f)) * 0.1f / (panelOpenGL.Height / 2.0f);
            float dx = (endPoint.X - startPoint.X);
            float dy = (endPoint.Y - startPoint.Y);
            float dxmin = panelOpenGL.Width / 100f;
            float dymin = panelOpenGL.Height / 100f;

            float x = (Math.Abs(dx) >= dxmin) ? dx * adjustPos : 0;
            float y = (Math.Abs(dy) >= dymin) ? dy * adjustPos : 0;

            Vector3 perp = new Vector3(-y, -x, 0f);
            float theta = (float)Math.Atan(perp.Length / 0.1f);
            //Compute the length of the perpendicular vector
            if (perp.Length > float.Epsilon)             //if its non-zero
            {
                // Return the perpendicular vector as the transform after all
                rotation.X = perp.X;
                rotation.Y = perp.Y;
                rotation.Z = perp.Z;
                //In the quaternion values, w is cosine (theta / 2), where theta is the rotation angle
                rotation.W = (float)Math.Cos(theta / 2);

                var trans = new Transform3DGroup();
                trans.TranslateBy(-camera.Position);
                trans.RotateBy(rotation);
                trans.TranslateBy(camera.Position);
                currentTrans = trans;

                updateModelMatricies();
            }
        }

        private void updateModelMatricies()
        {
            Matrix4 preTrans = getPreviousTransform();
            foreach (var v in objs)
                v.ModelMatrix = preTrans * currentTrans.Value;
        }

        private void getScale(Point endPoint)
        {
            float dy = startPoint.Y - endPoint.Y;
            if (Math.Abs(dy) > float.Epsilon)
            {
                float scale = 1.0f + dy * SCALE_FACTOR / panelOpenGL.Height;
                currentTrans = new ScaleTransform3D(scale, scale, scale);
                updateModelMatricies();
            }
        }

        private void getRotation(Point endPoin)
        {
            rotation = Quaternion.Identity;

            //Return the quaternion equivalent to the rotation
            //Compute the vector perpendicular to the begin and end vectors
            float dx = (endPoin.X - startPoint.X);
            float dy = (endPoin.Y - startPoint.Y);
            float dxmin = panelOpenGL.Width / 100f;
            float dymin = panelOpenGL.Height / 100f;

            float x = (Math.Abs(dx) >= dxmin) ? dx : 0;
            float y = (Math.Abs(dy) >= dymin) ? dy : 0;

            Vector3 perp = new Vector3(y, x, 0);
            float theta = (float)(perp.Length * (2 * Math.PI / Math.Max(panelOpenGL.Height, panelOpenGL.Width)));

            //Compute the length of the perpendicular vector
            if (perp.Length > float.Epsilon)             //if its non-zero
            {
                // Return the perpendicular vector as the transform after all
                rotation.X = perp.X;
                rotation.Y = perp.Y;
                rotation.Z = perp.Z;
                //In the quaternion values, w is cosine (theta / 2), where theta is the rotation angle
                rotation.W = (float) Math.Cos(theta / 2);
                currentTrans = new QuaternionRotateTransform3D(rotation);
                updateModelMatricies();
            }
        }

        private void panelOpenGL_KeyUp(object sender, KeyEventArgs e)
        {
            disableChangeObject();
        }
    }
}
