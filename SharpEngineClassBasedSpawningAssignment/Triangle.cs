namespace SharpEngine
{
    public class Triangle : Shape
    {
        public Triangle(Vertex[] vertices, Vector position) : base(vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += position;
            }
            centerPoint = GetCenter();
        }
    }
}