using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Shape 
    {
        protected Vertex[] vertices;
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
            fixed (Vertex* vertex = &this.vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length); // TODO: Try triangle_fan?
        }


        private float currentRotationDegrees;
        public void Rotate(float degrees)
        {
            float rDegrees = degrees * ((float)Math.PI / 180);
            float rotateRadians = rDegrees - currentRotationDegrees;
            currentRotationDegrees = rDegrees;

            float cosR = (float)Math.Cos(rotateRadians);
            float sinR = (float) Math.Sin(rotateRadians);
            
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector temp = new Vector(vertices[i].position.x, vertices[i].position.y);
                
                vertices[i].position.x = cosR * (temp.x - centerPoint.x) - sinR * (temp.y - centerPoint.y) + centerPoint.x;
                vertices[i].position.y = sinR * (temp.x - centerPoint.x) + cosR * (temp.y - centerPoint.y) + centerPoint.y;
            }
        }
        
        
        
        static unsafe void LoadShapeIntoBuffer() 
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }
    }
}