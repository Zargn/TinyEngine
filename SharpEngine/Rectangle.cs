namespace SharpEngine
{
    public class Rectangle : Shape
    {
        public Rectangle(float width, float height, Vector position) : base(new Vertex[6])
        {
            vertices[0] = new Vertex(new Vector(position.x + width / 2, position.y + height / 2, 0), Color.Blue);
            vertices[1] = new Vertex(new Vector(position.x - width / 2, position.y + height / 2, 0), Color.Blue);
            vertices[2] = new Vertex(new Vector(position.x - width / 2, position.y - height / 2, 0), Color.Blue);
            vertices[3] = new Vertex(new Vector(position.x - width / 2, position.y - height / 2, 0), Color.Blue);
            vertices[4] = new Vertex(new Vector(position.x + width / 2, position.y - height / 2, 0), Color.Blue);
            vertices[5] = new Vertex(new Vector(position.x + width / 2, position.y + height / 2, 0), Color.Blue);

            centerPoint = GetCenter();
        }
    }
}