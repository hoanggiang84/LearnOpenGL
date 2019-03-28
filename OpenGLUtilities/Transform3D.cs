using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    /// <summary>
    /// Defines the parent class of all 3D transforms that can be applied to 3D objects
    /// such as translation, rotation and scaling.
    /// </summary>
    public abstract class Transform3D : ICloneable
    {
        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Matrix4 Identity
        {
            get
            {
                return Matrix4.Identity;
            }
        }

        /// <summary>
        /// Gets the value of this transform in Matrix4d format
        /// </summary>
        public abstract Matrix4 Value { get; }

        public abstract object Clone();

        /// <summary>
        /// Transforms the given 4D vector by this Transform3D
        /// </summary>
        /// <param name="vector">The vector to be transformed</param>
        /// <returns>The transformed vector</returns>
        public Vector4 Transform(Vector4 vector)
        {
            return Vector4.Transform(vector, Value);
        }
    }
}
