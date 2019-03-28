using System;
using OpenTK;

namespace Render3DObject.Components
{
    public struct EulerAngles
    {
        public float Pitch; // X
        public float Yaw;   // Y
        public float Roll;  // Z

        public EulerAngles(float pitch, float yaw, float roll)
        {
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }

        public Vector3 UnityDirection()
        {
            return new Vector3() {
                X = (float)(Math.Cos(Pitch) * Math.Sin(Yaw)),
                Y = (float)(Math.Sin(Pitch)),
                Z = (float)(Math.Cos(Pitch) * Math.Cos(Yaw))
            };
        }
    }
}
