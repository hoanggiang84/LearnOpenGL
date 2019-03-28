using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    /// <summary>
    /// Represents a 3D rotation transform by a quaternion
    /// </summary>
    public class QuaternionRotateTransform3D : Transform3D
    {
        /// <summary>
        /// Gets or sets the rotation of this QuaternionRotateTransform3D
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Gets the value of this transform in Matrix4d format
        /// </summary>
        public override Matrix4 Value
        {
            get
            {
                Matrix4 transform = Matrix4.Identity;
                transform *= Matrix4.CreateFromQuaternion(Rotation);
                return transform;
            }
        }

        /// <summary>
        /// Initializes a new instance of QuaternionRotateTransform3D class with identity rotation
        /// </summary>
        public QuaternionRotateTransform3D()
        {
            Rotation = Quaternion.Identity;
        }

        /// <summary>
        /// Initializes a new instance of QuaternionRotateTransform3D class with the specified
        /// rotation and center
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="center"></param>
        public QuaternionRotateTransform3D(Quaternion rotation)
        {
            Rotation = rotation;
        }

        public override object Clone()
        {
            return new QuaternionRotateTransform3D(Rotation);
        }
    }
}