using OpenTK;
using OpenTK.Graphics;

namespace Render3DObject.Components
{
    public struct Vertex
    {
        public const int SIZE = (4 + 4) * 4;

        private readonly Vector4 _position;
        private readonly Color4 _color;

        public Vertex(Vector4 position, Color4 color)
        {
            _position = position;
            _color = color;
        }
    }
}
