using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLUtilities
{
    public class Transform3DGroup : Transform3D
    {
        /// <summary>
        /// Gets or sets the children of this Transform3DGroup instance
        /// </summary>
        public IList<Transform3D> Children { get; set; }

        /// <summary>
        /// Gets the result of the combination of all Transform3D's in this Transform3DGroup
        /// </summary>
        public override Matrix4 Value
        {
            get
            {
                Matrix4 transforms = Matrix4.Identity;
                foreach (var item in Children)
                    transforms *= item.Value;
                return transforms;
            }
        }

        /// <summary>
        /// Initializes a new instance of CodeFull.Graphics.Transform.Transform3D class.
        /// </summary>
        public Transform3DGroup()
        {
            Children = new List<Transform3D>();
        }

        /// <summary>
        /// Gets all the child transforms of the specified type
        /// </summary>
        /// <typeparam name="T">The type of the transform to look for (must be a subclass of Transform3D)</typeparam>
        /// <returns>A list of all the transforms of the specified type in the children of this instance</returns>
        public IEnumerable<Transform3D> GetTransforms<T>() where T : Transform3D
        {
            var temp = (from x in Children
                        where x.GetType() == typeof(T)
                        select x);

            return temp;
        }

        //------------------------------------------------------
        //
        //  Translation Modifiers
        //
        //------------------------------------------------------

        #region Translation Modifiers

        /// <summary>
        /// Translates this Tranform3D instance by the specified TranslateTransform3D instance.
        /// </summary>
        /// <param name="translate">The translation to be applied</param>
        public void TranslateBy(TranslateTransform3D translate)
        {
            Children.Add(new TranslateTransform3D(translate.OffsetX, translate.OffsetY, translate.OffsetZ));
        }

        /// <summary>
        /// Translates this Tranform3D instance by the specified offsets
        /// </summary>
        /// <param name="offsetX">The X offset</param>
        /// <param name="offsetY">The Y offset</param>
        /// <param name="offsetZ">The Z offset</param>
        public void TranslateBy(float offsetX, float offsetY, float offsetZ)
        {
            TranslateBy(new Vector3(offsetX, offsetY, offsetZ));
        }

        /// <summary>
        /// Translates this Tranform3D instance by the offsets specified by the Vector3d instance
        /// </summary>
        /// <param name="offset">The offsets of each axis in a vector format</param>
        public void TranslateBy(Vector3 offset)
        {
            TranslateBy(new TranslateTransform3D(offset));
        }

        /// <summary>
        /// Sets the traslation of this Tranform3D instance to the specified TranslateTransform3D instance.
        /// </summary>
        /// <param name="translate">The translation to be applied</param>
        public void SetTranslation(TranslateTransform3D translate)
        {
            // Find and remove all translation transforms
            var translations = GetTransforms<TranslateTransform3D>();
            foreach (var item in translations)
                Children.Remove(item);
            Children.Add(new TranslateTransform3D(translate.OffsetX, translate.OffsetY, translate.OffsetZ));
        }

        /// <summary>
        /// Sets the traslation of this Tranform3D instance to the specified offsets
        /// </summary>
        /// <param name="offsetX">The X offset</param>
        /// <param name="offsetY">The Y offset</param>
        /// <param name="offsetZ">The Z offset</param>
        public void SetTranslation(float offsetX, float offsetY, float offsetZ)
        {
            SetTranslation(new Vector3(offsetX, offsetY, offsetZ));
        }

        /// <summary>
        /// Sets the traslation of this Tranform3D instance to the specified Vector3d instance
        /// </summary>
        /// <param name="offset">The offsets of each axis in a vector format</param>
        public void SetTranslation(Vector3 offset)
        {
            SetTranslation(new TranslateTransform3D(offset));
        }

        #endregion

        //------------------------------------------------------
        //
        //  Scaling Modifiers
        //
        //------------------------------------------------------

        #region Scaling Modifiers

        /// <summary>
        /// Scales this Tranform3D instance by the specified ScaleTransform3D instance.
        /// </summary>
        /// <param name="scale">The scale to be applied</param>
        public void ScaleBy(ScaleTransform3D scale)
        {
            // Have to add 1 to make the transformation additive!
            Children.Add(new ScaleTransform3D(scale.ScaleX + 1, scale.ScaleY + 1, scale.ScaleZ + 1));
        }

        /// <summary>
        /// Scales this Tranform3D instance by the amounts specified by the Vector3d instance and around
        /// the specified center
        /// </summary>
        /// <param name="scale">The scales of each axis in a vector format</param>
        public void ScaleBy(Vector3 scale)
        {
            ScaleBy(new ScaleTransform3D(scale));
        }

        /// <summary>
        /// Scale this Tranform3D instance by the specified amounts
        /// </summary>
        /// <param name="scaleX">The X scale</param>
        /// <param name="scaleY">The Y scale</param>
        /// <param name="scaleZ">The Z scale</param>
        public void ScaleBy(float scaleX, float scaleY, float scaleZ)
        {
            ScaleBy(new Vector3(scaleX, scaleY, scaleZ));
        }

        /// <summary>
        /// Sets the scale of this Tranform3D instance to the specified ScaleTransform3D instance.
        /// </summary>
        /// <param name="scale">The scale to be applied</param>
        public void SetScale(ScaleTransform3D scale)
        {
            // Find and remove all scale transforms
            var scales = GetTransforms<ScaleTransform3D>();
            foreach (var item in scales)
                Children.Remove(item);
            Children.Add(new ScaleTransform3D(scale.ScaleX, scale.ScaleY, scale.ScaleZ));
        }

        /// <summary>
        /// Sets the scale of this Tranform3D instance to the specified offsets
        /// </summary>
        /// <param name="scaleX">The X scale</param>
        /// <param name="scaleY">The Y scale</param>
        /// <param name="scaleZ">The Z scale</param>
        public void SetScale(float scaleX, float scaleY, float scaleZ)
        {
            SetScale(new Vector3(scaleX, scaleY, scaleZ));
        }

        /// <summary>
        /// Sets the scale of this Tranform3D instance to the specified Vector3d instance
        /// </summary>
        /// <param name="offset">The scale of each axis in a vector format</param>
        public void SetScale(Vector3 offset)
        {
            SetScale(new ScaleTransform3D(offset));
        }

        #endregion

        #region Quaternion Rotation modifiers
        public void RotateBy(Quaternion rotate)
        {
            Children.Add(new QuaternionRotateTransform3D(rotate));
        }

        public void SetRotation(Quaternion rotate)
        {
            var rotations = GetTransforms<QuaternionRotateTransform3D>();
            foreach (var item in rotations)
                Children.Remove(item);
            Children.Add(new QuaternionRotateTransform3D(rotate));
        }
        #endregion
        
        /// <summary>
        /// Creates a deep clone of this Transform3DGroup instance with all of its children
        /// </summary>
        /// <returns>A deep clone of this transform</returns>
        public override object Clone()
        {
            Transform3DGroup result = new Transform3DGroup();
            foreach (var item in Children)
                result.Children.Add(item.Clone() as Transform3D);
            return result;
        }
    }
}

