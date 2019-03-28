using System;
using OpenTK;

namespace Render3DObject.Components
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0, 0, 3.0f);
        public EulerAngles Orientation = new EulerAngles(0, (float)Math.PI, 0);
        public float MoveSpeed = 0.01f;
        public float MouseSensitivity = 0.001f;

        public Matrix4 GetViewMatrix()
        {
            var lookat = Orientation.UnityDirection();
            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            var offset = new Vector3();
            var forward = Orientation.UnityDirection();             //new Vector3((float)Math.Sin(Orientation.Yaw), 0, (float)Math.Cos(Orientation.Yaw));
            var right = Vector3.Cross(forward, Vector3.UnitY);      //new Vector3(-forward.Z, 0, forward.X);

            if (forward.X == 0)
            {
                Console.WriteLine("");
            }

            offset += x * right;
            offset += y * forward; ;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);
            Position += offset;
        }

        public void AddRotation(float x = 0, float y = 0)
        {
            var yaw  = x * MouseSensitivity;
            var pitch = y * MouseSensitivity;

            Orientation.Yaw = (Orientation.Yaw + yaw) % ((float)Math.PI * 2.0f);
            Orientation.Pitch = Math.Max(
                Math.Min(Orientation.Pitch + pitch, 
                         (float)Math.PI / 2.0f - 0.1f),
                (float)-Math.PI / 2.0f + 0.1f);
        }

        public void ResetPosition()
        {
            Position = new Vector3(0, 0, 3.0f);
            Orientation = new EulerAngles(0, (float)Math.PI, 0);
        }

        static float radians(float deg)
        {
            return (float)((deg/180) * Math.PI);
        }
    }
}
