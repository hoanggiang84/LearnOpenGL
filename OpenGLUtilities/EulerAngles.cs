using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    public struct EulerAngles
    {
        public float PitchRad; // X
        public float YawRad;   // Y
        public float RollRad;  // Z

        public EulerAngles(float pitchRad, float yawRad, float rollRad)
        {
            PitchRad = pitchRad;
            YawRad = yawRad;
            RollRad = rollRad;
        }

        public Vector3 UnityDirection()
        {
            return new Vector3()
            {
                X = (float)(Math.Cos(PitchRad) * Math.Sin(YawRad)),
                Y = (float)(Math.Sin(PitchRad)),
                Z = (float)(Math.Cos(PitchRad) * Math.Cos(YawRad))
            };
        }
    }
}
