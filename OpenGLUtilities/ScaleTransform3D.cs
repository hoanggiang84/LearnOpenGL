using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    /// <summary>
    /// Represents a 3D scaling transform
    /// </summary>
    public class ScaleTransform3D : Transform3D
    {
        /// <summary>
        /// The X scale
        /// </summary>
        public float ScaleX { get; set; }

        /// <summary>
        /// The Y scale
        /// </summary>
        public float ScaleY { get; set; }

        /// <summary>
        /// The Z scale
        /// </summary>
        public float ScaleZ { get; set; }

        /// <summary>
        /// Gets or sets the scale values of this scale transform
        /// in a vector format
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return new Vector3(ScaleX, ScaleY, ScaleZ);
            }

            set
            {
                ScaleX = value.X;
                ScaleY = value.Y;
                ScaleZ = value.Z;
            }
        }

        /// <summary>
        /// Gets the value of this transform in Matrix4d format
        /// </summary>
        public override Matrix4 Value
        {
            get
            {
                Matrix4 transform = Matrix4.Identity;
                transform *= Matrix4.CreateScale(ScaleX, ScaleY, ScaleZ);
                return transform;
            }
        }

        /// <summary>
        /// Initializes a new instance of ScaleTransform3D class with identity scale
        /// </summary>
        public ScaleTransform3D()
        {
            Scale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Initializes a new instance of ScaleTransform3D class with the provided scales
        /// and the center
        /// </summary>
        /// <param name="scaleX">The X scale</param>
        /// <param name="scaleY">The X scale</param>
        /// <param name="scaleZ">The X scale</param>
        /// <param name="center">The center of the transform</param>
        public ScaleTransform3D(float scaleX, float scaleY, float scaleZ)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleZ = scaleZ;
        }

        /// <summary>
        /// Initializes a new instance of ScaleTransform3D class with the provided scales
        /// and the center
        /// </summary>
        /// <param name="scale">The scale values in vector format</param>
        /// <param name="center">The center of the transform</param>
        public ScaleTransform3D(Vector3 scale) : this(scale.X, scale.Y, scale.Z) { }

        public override object Clone()
        {
            return new ScaleTransform3D(ScaleX, ScaleY, ScaleZ);
        }
    }
}
