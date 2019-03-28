using OpenTK.Graphics;
using OpenTK;
using System;

namespace Render3DObject.Components
{
    public class ShapeFactory
    {
        public static Vertex[] CreateSolidCube(float side, Color4 color)
        {
            Vertex[] vertices =
            {
               new Vertex(new Vector4(-side, -side, -side, 1.0f),   color),
               new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
               new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
               new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
               new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
               new Vertex(new Vector4(-side, side, side, 1.0f),     color),

               new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
               new Vertex(new Vector4(side, side, -side, 1.0f),     color),
               new Vertex(new Vector4(side, -side, side, 1.0f),     color),
               new Vertex(new Vector4(side, -side, side, 1.0f),     color),
               new Vertex(new Vector4(side, side, -side, 1.0f),     color),
               new Vertex(new Vector4(side, side, side, 1.0f),      color),

               new Vertex(new Vector4(-side, -side, -side, 1.0f),   color),
               new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
               new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
               new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
               new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
               new Vertex(new Vector4(side, -side, side, 1.0f),     color),

               new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
               new Vertex(new Vector4(-side, side, side, 1.0f),     color),
               new Vertex(new Vector4(side, side, -side, 1.0f),     color),
               new Vertex(new Vector4(side, side, -side, 1.0f),     color),
               new Vertex(new Vector4(-side, side, side, 1.0f),     color),
               new Vertex(new Vector4(side, side, side, 1.0f),      color),

               new Vertex(new Vector4(-side, -side, -side, 1.0f),   color),
               new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
               new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
               new Vertex(new Vector4(side, -side, -side, 1.0f),    color),
               new Vertex(new Vector4(-side, side, -side, 1.0f),    color),
               new Vertex(new Vector4(side, side, -side, 1.0f),     color),

               new Vertex(new Vector4(-side, -side, side, 1.0f),    color),
               new Vertex(new Vector4(side, -side, side, 1.0f),     color),
               new Vertex(new Vector4(-side, side, side, 1.0f),     color),
               new Vertex(new Vector4(-side, side, side, 1.0f),     color),
               new Vertex(new Vector4(side, -side, side, 1.0f),     color),
               new Vertex(new Vector4(side, side, side, 1.0f),      color),
            };
            return vertices;
        }

        public static Vertex[] CreateHeightMap()
        {
            var map = new Surface("heightmap.png");
            var h = new float[128, 128];

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    h[i, j] = 0.25f * ((float)(map.pixels[i + j * 128] & 255)) / 256;
                }
            }

            int index = 0;
            var vertices = new Vertex[127 * 127 * 2 * 3];
            Func<float, float, Color4> createBrightColor = (v, b) => new Color4(v * b, v * b, v * b, 1.0f); 
            for (int i = 0; i < 127; i++)
            {
                for (int j = 0; j < 127; j++)
                {
                    float x1 = (float)(i - 64) / 128;
                    float y1 = (float)(j - 64) / 128;

                    float x2 = (float)(i + 1 - 64) / 128;
                    float y2 = (float)(j + 1 - 64) / 128;

                    float z11 = h[i, j];
                    float z12 = h[i, j + 1];
                    float z22 = h[i + 1, j + 1];
                    float z21 = h[i + 1, j];

                    float bright = 5;
                    vertices[index++] = new Vertex(new Vector4(x1, y1, z11, 1.0f), createBrightColor(z11, bright));
                    vertices[index++] = new Vertex(new Vector4(x2, y1, z21, 1.0f), createBrightColor(z21, bright));
                    vertices[index++] = new Vertex(new Vector4(x1, y2, z12, 1.0f), createBrightColor(z12, bright));
                    vertices[index++] = new Vertex(new Vector4(x2, y1, z21, 1.0f), createBrightColor(z21, bright));
                    vertices[index++] = new Vertex(new Vector4(x1, y2, z12, 1.0f), createBrightColor(z12, bright));
                    vertices[index++] = new Vertex(new Vector4(x2, y2, z22, 1.0f), createBrightColor(z22, bright));
                }
            }

            if (index != (vertices.Length))
                Console.WriteLine($"Init array error {index} <> {vertices.Length}");

            return vertices;
        }

        public static Vertex[] CreateQueenMap()
        {
            var map = new Surface("queen.png");
            var width = map.width;
            var height = map.height;
            var h = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    h[i, j] = 0.25f * ((float)(map.pixels[i + j * height] & 255)) / 256;
                }
            }

            int index = 0;
            var vertices = new Vertex[(width - 1) * (height - 1) * 2 * 3];
            Func<float, float, Color4> createBrightColor = (v, b) => new Color4(v * b, v * b, v * b, 1.0f);
            for (int i = 0; i < (width - 1); i++)
            {
                for (int j = 0; j < (height - 1); j++)
                {
                    float x1 = (float)(i - (width / 2)) / width;
                    float y1 = (float)(j - (height / 2)) / height;

                    float x2 = (float)(i + 1 - (width / 2)) / width;
                    float y2 = (float)(j + 1 - (height / 2)) / height;

                    float z11 = h[i, j];
                    float z12 = h[i, j + 1];
                    float z22 = h[i + 1, j + 1];
                    float z21 = h[i + 1, j];

                    float bright = 5;
                    vertices[index++] = new Vertex(new Vector4(x1, y1, z11, 1.0f), createBrightColor(z11, bright));
                    vertices[index++] = new Vertex(new Vector4(x2, y1, z21, 1.0f), createBrightColor(z21, bright));
                    vertices[index++] = new Vertex(new Vector4(x1, y2, z12, 1.0f), createBrightColor(z12, bright));
                    vertices[index++] = new Vertex(new Vector4(x2, y1, z21, 1.0f), createBrightColor(z21, bright));
                    vertices[index++] = new Vertex(new Vector4(x1, y2, z12, 1.0f), createBrightColor(z12, bright));
                    vertices[index++] = new Vertex(new Vector4(x2, y2, z22, 1.0f), createBrightColor(z22, bright));
                }
            }

            if (index != (vertices.Length))
                Console.WriteLine($"Init array error {index} <> {vertices.Length}");

            return vertices;
        }
    }
}
