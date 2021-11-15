using System;

namespace SharpEngine
{
    public class Cone : Shape
    {
        public Cone(float diameter, int sections, float heigth, Vector position) : base(new Vertex[sections * 3])
        {
            for (int u = 0; u < vertices.Length; u++)
            {
                vertices[u] = new Vertex(new Vector(diameter/2,0,0), Color.Red);
            }
            

            float currentDegree = 0;
            float degreesPerTriangle = (float)360 / sections;
            Console.WriteLine(degreesPerTriangle);

            for (int i = 0; i < vertices.Length; i += 3)
            {
                
                
                for (int i2 = 0; i2 < 2; i2++)
                {
                    float rDegrees = (currentDegree + (degreesPerTriangle * i2)) * ((float)Math.PI / 180);
                    // float rotateRadians = rDegrees - currentDegree;
                    currentDegree = rDegrees * (float)(180/Math.PI);
                    
                    float cosR = (float)Math.Cos(rDegrees);
                    float sinR = (float) Math.Sin(rDegrees);
                
                    Vector temp = new Vector(vertices[i + i2].position.x, vertices[i + i2].position.y);
                
                    vertices[i + i2].position.x = cosR * (temp.x - 0) - sinR * (temp.y - 0) + 0;
                    vertices[i + i2].position.y = sinR * (temp.x - 0) + cosR * (temp.y - 0) + 0;
                    vertices[i + i2].color = Color.Green;
                    vertices[i + i2].position += position;
                    // Console.WriteLine(vertices[i+i2].position.x);
                    Console.WriteLine(rDegrees);
                }

                vertices[i + 2] = new Vertex(position, new Color(0, 0, 0, 1));
                vertices[i + 2].position.z += heigth;
            }
        }
    }
}