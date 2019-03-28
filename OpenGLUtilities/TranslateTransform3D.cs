using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    /// <summary>
    /// Represents a 3D translation transform
    /// </summary>
    public class TranslateTransform3D : Transform3D
    {
        /// <summary>
        /// The X offset
        /// </summary>
        public float OffsetX { get; set; }

        /// <summary>
        /// The Y offset
        /// </summary>
        public float OffsetY { get; set; }

        /// <summary>
        /// The Z offset
        /// </summary>
        public float OffsetZ { get; set; }

        /// <summary>
        /// Gets or sets the offset values of this translation transform
        /// in a vector format
        /// </summary>
        public Vector3 Offset
        {
            get
            {
                return new Vector3(OffsetX, OffsetY, OffsetZ);
            }

            set
            {
                OffsetX = value.X;
                OffsetY = value.Y;
                OffsetZ = value.Z;
            }
        }

        /// <summary>
        /// Gets the value of this transform in Matrix4d format
        /// </summary>
        public override Matrix4 Value
        {
            get
            {
                return Matrix4.CreateTranslation(OffsetX, OffsetY, OffsetZ);
            }
        }

        /// <summary>
        /// Initializes a new instance of TranslateTransform3D class
        /// </summary>
        public TranslateTransform3D()
        {
            Offset = new Vector3();
        }

        /// <summary>
        /// Initializes a new instance of TranslateTransform3D class with the specified
        /// offsets
        /// </summary>
        /// <param name="offsetX">The X offset</param>
        /// <param name="offsetY">The Y offset</param>
        /// <param name="offsetZ">The Z offset</param>
        public TranslateTransform3D(float offsetX, float offsetY, float offsetZ)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            OffsetZ = offsetZ;
        }

        /// <summary>
        /// Initializes a new instance of TranslateTransform3D class with the specified
        /// offset values in a vector format
        /// </summary>
        /// <param name="offset">The offset of the translation</param>
        public TranslateTransform3D(Vector3 offset) : this(offset.X, offset.Y, offset.Z) { }

        public override object Clone()
        {
            return new TranslateTransform3D(OffsetX, OffsetY, OffsetZ);
        }
    }
}
