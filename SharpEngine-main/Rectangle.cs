namespace SharpEngine
{
    public class Rectangle : Shape
    {
        public Rectangle(float width, float height, Material material) : base(new Vertex[6], material)
        {
            vertices[0] = new Vertex(new Vector(+ width / 2, + height / 2, 0), Color.Blue);
            vertices[1] = new Vertex(new Vector(- width / 2, + height / 2, 0), Color.Red);
            vertices[2] = new Vertex(new Vector(- width / 2, - height / 2, 0), Color.Green);
            vertices[3] = new Vertex(new Vector(- width / 2, - height / 2, 0), Color.Green);
            vertices[4] = new Vertex(new Vector(+ width / 2, - height / 2, 0), Color.Red);
            vertices[5] = new Vertex(new Vector(+ width / 2, + height / 2, 0), Color.Blue);
        }
    }
}