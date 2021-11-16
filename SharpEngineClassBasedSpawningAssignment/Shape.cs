using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Shape 
    {
        protected Vertex[] vertices;
        protected uint[] indices;
        
        
        protected Vector centerPoint;

        public float CurrentScale { get; private set; }
            
        
        public Shape(Vertex[] vertices) 
        {
            this.vertices = vertices;
            this.CurrentScale = 1f;
            centerPoint = GetCenter();
            LoadShapeIntoBuffer();
        }

        protected void CreateShape(Vertex[] vertices)
        {
            this.vertices = vertices;
            centerPoint = GetCenter();
            LoadShapeIntoBuffer();
        }
        
        
        
        public Vector GetMinBounds() 
        {
            var min = this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                min = Vector.Min(min, this.vertices[i].position);
            }

            return min;
        }
          
        
        
        public Vector GetMaxBounds() 
        {
            var max = this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                max = Vector.Max(max, this.vertices[i].position);
            }

            return max;
        }

        
        
        public Vector GetCenter() {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }
        
        

        public void Scale(float multiplier) 
        {
            // We first move the triangle to the center, to avoid
            // the triangle moving around while scaling.
            // Then, we move it back again.
            var center = GetCenter();
            Move(center*-1);
            for (var i = 0; i < this.vertices.Length; i++) {
                this.vertices[i].position *= multiplier;
            }
            Move(center);

            this.CurrentScale *= multiplier;
        }

        
        
        public void Move(Vector direction) 
        {
            for (var i = 0; i < this.vertices.Length; i++) {
                this.vertices[i].position += direction;
            }

            centerPoint += direction;
        }



        public unsafe void Render() 
        {
            // glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
            
            
            fixed (Vertex* vertex = &this.vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length); // TODO: Try triangle_fan?
        }



        private Vector currentRotation;
        public void Rotate(Vector rotations)
        {
            float xDegrees = rotations.x * ((float) Math.PI / 180);
            float yDegrees = rotations.y * ((float) Math.PI / 180);
            float zDegrees = rotations.z * ((float) Math.PI / 180);

            float xRadians = xDegrees - currentRotation.x;
            float yRadians = yDegrees - currentRotation.y;
            float zRadians = zDegrees - currentRotation.z;

            currentRotation.x = xDegrees;
            currentRotation.y = yDegrees;
            currentRotation.z = zDegrees;

            if (rotations.x != 0)
            {
                // Rotate around X
                float cosR = (float)Math.Cos(xRadians);
                float sinR = (float)Math.Sin(xRadians);

                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector temp = new Vector(vertices[i].position.x, vertices[i].position.y, vertices[i].position.z);

                    vertices[i].position.y = cosR * (temp.y - centerPoint.y) - sinR * (temp.z - centerPoint.z) + centerPoint.y;
                    vertices[i].position.z = sinR * (temp.y - centerPoint.y) + cosR * (temp.z - centerPoint.z) + centerPoint.z;
                }

                Console.WriteLine("Rotated on x axis");
            }

            if (rotations.y != 0)
            {
                // Rotate around Y
                float cosR = (float)Math.Cos(yRadians);
                float sinR = (float)Math.Sin(yRadians);
                
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector temp = new Vector(vertices[i].position.x, vertices[i].position.y, vertices[i].position.z);

                    vertices[i].position.x = cosR * (temp.x - centerPoint.x) + sinR * (temp.z - centerPoint.z) + centerPoint.x;
                    vertices[i].position.z = -sinR * (temp.x - centerPoint.x) + cosR * (temp.z - centerPoint.z) + centerPoint.z;
                }
            }

            if (rotations.z != 0)
            {
                // Rotate around Z
                float cosR = (float)Math.Cos(zRadians);
                float sinR = (float)Math.Sin(zRadians);
                
                for (int i = 0; i < vertices.Length; i++)
                {
                    
                    Vector temp = new Vector(vertices[i].position.x, vertices[i].position.y, vertices[i].position.z);
                
                    vertices[i].position.x = cosR * (temp.x - centerPoint.x) - sinR * (temp.y - centerPoint.y) + centerPoint.x;
                    vertices[i].position.y = sinR * (temp.x - centerPoint.x) + cosR * (temp.y - centerPoint.y) + centerPoint.y;
                }
            }
        }
        
        
        
        static unsafe void LoadShapeIntoBuffer()
        {
            // uint EBO;
            // glGenBuffers(1, &EBO);
            //
            // glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);


            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            // glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }
    }
}