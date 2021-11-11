using System;
using OpenGL;

namespace Engine_Protoype
{
    class Triangle
    {
        public void AddToPipeline()
        {
            vertices.CopyTo(globalVertices, 3 * id);
        }
    
        public static int NumberOfTriangles = 0;
        
        private static Vector[] globalVertices = new Vector[3];

        public static unsafe void Render()
        {
            fixed (Vector* vertex = &globalVertices[0])
            {
                // Put the updated data in the buffer.
                Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vector) * globalVertices.Length, vertex, Gl.GL_STATIC_DRAW);
            }
        }
        
        private static uint nextId;
        private uint id;
        
        public Triangle(Vector value)
        {
            // Put the center of the triangle at the desired offset.
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x += value.x;
                vertices[i].y += value.y;
                vertices[i].z += value.z;
            }

            centerPoint = value;

            id = nextId;
            nextId++;
    
            NumberOfTriangles++;
            
            Array.Resize(ref globalVertices, globalVertices.Length + vertices.Length);
            
            createTriangle();
        }
    
        unsafe void createTriangle()
        {
            Gl.glVertexAttribPointer(id, 3, Gl.GL_FLOAT, false, 3 * sizeof(float), null);
    
            Gl.glEnableVertexAttribArray(id);
        }

        public void Rotate(float degrees)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector temp = new Vector(vertices[i].x, vertices[i].y);
                
                vertices[i].x = (float) Math.Cos(degrees) * (temp.x - centerPoint.x) -
                    (float)Math.Sin(degrees) * (temp.y - centerPoint.y) + centerPoint.x;
                vertices[i].y = (float) Math.Sin(degrees) * (temp.x - centerPoint.x) +
                                (float)Math.Cos(degrees) * (temp.y - centerPoint.y) + centerPoint.y;
            }
        }

        public void Scale(float scaleMultiplier)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                // Todo: Make cleaner and more compact!

                float tempX;
                float tempY;

                tempX = vertices[i].x - centerPoint.x;
                tempY = vertices[i].y - centerPoint.y;

                tempX *= scaleMultiplier;
                tempY *= scaleMultiplier;

                tempX += centerPoint.x;
                tempY += centerPoint.y;

                vertices[i].x = tempX;
                vertices[i].y = tempY;
            }
        }

        private Vector centerPoint = new Vector(0, 0, 0);

        Vector[] vertices = new[]
        {
            // new Vector(-.1f,-.1f),
            // new Vector(.1f,-.1f),
            // new Vector(0f,.1f)
            new Vector(-.1f,-.07f),
            new Vector(.1f,-.07f),
            new Vector(0f,.123f)
        };
    }
}