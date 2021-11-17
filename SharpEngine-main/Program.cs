using System;
using System.Collections.Generic;
using GLFW;

namespace SharpEngine
{
    class Program {
        static float Lerp(float from, float to, float t) {
            return from + (to - from) * t;
        }

        static float GetRandomFloat(Random random, float min = 0, float max = 1) {
            return Lerp(min, max, (float)random.Next() / int.MaxValue);
        }
        
        static void FillSceneWithTriangles(Scene scene, Material material) {
            var random = new Random();
            for (var i = 0; i < 10; i++) {
                var triangle = new Shape(new Vertex[] {
                    new Vertex(new Vector(-.1f, 0f), Color.Blue),
                    new Vertex(new Vector(.1f, 0f), Color.Blue),
                    new Vertex(new Vector(0f, .133f), Color.Red)
                }, material);
                triangle.Transform.Rotate(new Vector(0,0,GetRandomFloat(random)));
                triangle.Transform.Move(new Vector(GetRandomFloat(random, -1, 1), GetRandomFloat(random, -1, 1)));
                scene.Add(triangle);
            }
        }
        
        static void Main(string[] args) {
            
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);

            // FillSceneWithTriangles(scene, material);
            var newtriangle = new Triangle(new Vertex[] {
                new Vertex(new Vector(-.05f, -.05f), Color.Blue),
                new Vertex(new Vector(.05f, -.05f), Color.Blue),
                new Vertex(new Vector(0f, .08f), Color.Red)
            }, material);
            
            newtriangle.Transform.Scale(new Vector(1, 1, 1));

            var circle = new Circle(0.3f, 25, material);
            circle.Transform.Move(new Vector(.5f,-.5f,0f));
            
            var cone = new Cone(1, 50, 1, material);
            
            var rectangle = new Rectangle(0.2f, 0.2f, material);
            rectangle.Transform.Move(new Vector(0,-.5f,0));
            
            
            scene.Add(newtriangle);
            scene.Add(circle);
            // scene.Add(cone);
            scene.Add(rectangle);
            
            // circle.Transform.Move(new Vector(0.2f,0,0));Ho


            var ground = new Rectangle(2f, 0.1f, material);
            ground.Transform.Move(new Vector(0,-0.95f,0));
            scene.Add(ground);
            

            // engine rendering loop
            var direction = new Vector(0.01f, 0.01f);
            var multiplier = 0.95f;
            // var rotation = 0.05f;

            const int FixedFramerate = 30;
            const double FrameTime = 1.0 / FixedFramerate;
            double NextFrameTimeTarget = 0.0;

            float movementSpeed = 0.5f;
            
            
            while (window.IsOpen()) {

                if (Glfw.Time > NextFrameTimeTarget)
                {
                    NextFrameTimeTarget += FrameTime;
                    // Console.WriteLine(Glfw.Time);
                    
                    
                    var walkDirection = new Vector();

                    if (window.GetKey(Keys.W))
                    {
                        walkDirection += newtriangle.Transform.Forward;
                    }
                    if (window.GetKey(Keys.S))
                    {
                        walkDirection +=newtriangle.Transform.Backward;
                    }
                    if (window.GetKey(Keys.Q))
                    {
                        walkDirection += newtriangle.Transform.Left;
                    }
                    if (window.GetKey(Keys.E))
                    {
                        walkDirection += newtriangle.Transform.Right;
                    }
                    if (window.GetKey(Keys.A))
                    {
                        var rotation = newtriangle.Transform.Rotation;
                        rotation.z += 2 * MathF.PI * (float)FrameTime;
                        newtriangle.Transform.Rotation = rotation;
                    }
                    if (window.GetKey(Keys.D))
                    {
                        var rotation = newtriangle.Transform.Rotation;
                        rotation.z -= 2 * MathF.PI * (float)FrameTime;
                        newtriangle.Transform.Rotation = rotation;
                    }
                    
                    # region The Scared Rectangle
                    if (Vector.Dot(newtriangle.Transform.Forward, rectangle.Transform.Forward) < 0)
                    {
                        rectangle.SetColor(Color.Green);
                    }
                    else
                    {
                        rectangle.SetColor(Color.Red);
                    }
                    #endregion
                    
                    # region The Spooky light
                    double piColorRelation = 1 / Math.PI;
                    float circleColor = 1 - MathF.Acos(Vector.Dot((newtriangle.Transform.Position - circle.Transform.Position).Normalize(), newtriangle.Transform.Forward.Normalize())) * (float)piColorRelation;
                    circle.SetColor(new Color(circleColor, circleColor, circleColor, circleColor));
                    
                    #endregion
                    
                    
                    
                    walkDirection = walkDirection.Normalize();

                    newtriangle.Transform.Position += walkDirection * movementSpeed * (float)FrameTime;
                    
                    window.Render();
                }
            }
        }
    }
}
