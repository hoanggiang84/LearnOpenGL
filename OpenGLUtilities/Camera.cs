using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    public class Camera
    {
        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set {
                _position = value;
                Reset();
            }
        }

        /// <summary>
        /// Always look at worldspace origin
        /// </summary>
        public Vector3 Target { get
            {
                return new Vector3();
            }
        } 

        public float MoveSpeed = 0.1f;
        public float MouseSensitivity = 0.0025f;
        public readonly Vector3 presetPosition;
        private Vector3 up;
        private Vector3 right;
        private Vector3 front;

        public Camera()
        {
            presetPosition = Vector3.Zero;
            Position = presetPosition;
            Reset();
        }

        public Camera(Vector3 position)
        {
            presetPosition = position;
            Position = presetPosition;
            Reset();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Target, up);
        }

        public void Reset()
        {
            front = Vector3.NormalizeFast(Position - Target);
            up = new Vector3(0f, 1f, 0f);
            right = Vector3.NormalizeFast(Vector3.Cross(up, front));
            up = Vector3.Cross(front, right);
        }
    }
}
