using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using GLFW;
using static OpenGL.Gl;

namespace Engine_Prototype
{
    class Triangle
    {
        public void AddToPipeline()
        {
            vertices.CopyTo(globalVertices, 3 * id);
        }
    
        public static int NumberOfTriangles = 0;

        private static Vertex[] globalVertices = new Vertex[3];

        
        
        public static unsafe void AddToBuffer()
        {
            fixed (Vertex* vertex = &globalVertices[0])
            {
                // Put the updated data in the buffer.
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * globalVertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
        }
        
        
        
        private static uint nextId;
        private static uint nextIndex;
        private uint id;
        private uint index;
        
        
        public Triangle(Vertex vertex)
        {
            // Put the center of the triangle at the desired offset.
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position.x += vertex.position.x;
                vertices[i].position.y += vertex.position.y;
                vertices[i].position.z += vertex.position.z;
                vertices[i].color = vertex.color;
            }
            
            // Get the centerPoint
            Vector min = vertices[0].position;
            Vector max = vertices[0].position;
            for (int i = 0; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
                max = Vector.Max(max, vertices[i].position);
            }

            centerPoint = (max + min) / 2;
            

            id = nextId;
            index = nextIndex;
            nextIndex += 2;
            nextId++;
    
            NumberOfTriangles++;
            
            Array.Resize(ref globalVertices, globalVertices.Length + vertices.Length);
            
            createTriangle();
        }
    
        
        
        unsafe void createTriangle()
        {
            // glVertexAttribPointer(id, 3, GL_FLOAT, false, 3 * sizeof(float), null);
    
            glVertexAttribPointer(index, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            
            glVertexAttribPointer(index + 1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));

            
            glEnableVertexAttribArray(index);
            glEnableVertexAttribArray(index + 1);
        }

        
        
        public void Rotate(float degrees)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector temp = new Vector(vertices[i].position.x, vertices[i].position.y);
                
                vertices[i].position.x = (float) Math.Cos(degrees) * (temp.x - centerPoint.x) -
                    (float)Math.Sin(degrees) * (temp.y - centerPoint.y) + centerPoint.x;
                vertices[i].position.y = (float) Math.Sin(degrees) * (temp.x - centerPoint.x) +
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

                tempX = vertices[i].position.x - centerPoint.x;
                tempY = vertices[i].position.y - centerPoint.y;

                tempX *= scaleMultiplier;
                tempY *= scaleMultiplier;

                tempX += centerPoint.x;
                tempY += centerPoint.y;

                vertices[i].position.x = tempX;
                vertices[i].position.y = tempY;
            }
        }

        
        
        private Vector centerPoint = new Vector(0, 0, 0);

        
        
        Vector[] verticesOLD = new[]
        {
            // new Vector(-.1f,-.1f),
            // new Vector(.1f,-.1f),
            // new Vector(0f,.1f)
            new Vector(-.1f,-.07f),
            new Vector(.1f,-.07f),
            new Vector(0f,.123f)
        };

        private Vertex[] vertices = new[]
        {
            new Vertex(new Vector(-.1f, -.07f), Color.Red),
            new Vertex(new Vector(.1f, -.07f), Color.Red),
            new Vertex(new Vector(0f, .123f), Color.Red)
        };
    }
}