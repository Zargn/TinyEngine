using GLFW;
using static OpenGL.Gl;
using System.IO;

namespace SharpEngine
{
    public class Triangle {
            
        Vertex[] vertices;
            
        public float CurrentScale { get; private set; }
            
        public Triangle(Vertex[] vertices) {
            this.vertices = vertices;
            this.CurrentScale = 1f;
        }

        public Vector GetMinBounds() {
            var min = this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                min = Vector.Min(min, this.vertices[i].position);
            }

            return min;
        }
            
        public Vector GetMaxBounds() {
            var max = this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                max = Vector.Max(max, this.vertices[i].position);
            }

            return max;
        }

        public Vector GetCenter() {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }

        public void Scale(float multiplier) {
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

        public void Move(Vector direction) {
            for (var i = 0; i < this.vertices.Length; i++) {
                this.vertices[i].position += direction;
            }
        }

        public unsafe void Render() {
            fixed (Vertex* vertex = &this.vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length);
        }
    }
}