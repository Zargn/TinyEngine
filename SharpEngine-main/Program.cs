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
            
            var random = new Random();
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            var physics = new Physics(scene);
            window.Load(scene);

            // FillSceneWithTriangles(scene, material);
            // var newtriangle = new Triangle(new Vertex[] {
            //     new Vertex(new Vector(-.05f, -.05f), Color.Blue),
            //     new Vertex(new Vector(.05f, -.05f), Color.Blue),
            //     new Vertex(new Vector(0f, .08f), Color.Red)
            // }, material);
            
            // newtriangle.Transform.Scale(new Vector(1, 1, 1));

            // var circle = new Circle(0.3f, 25, material);
            // circle.Transform.Move(Vector.Left);
            // circle.velocity = Vector.Right * 0.3f;
            //
            // var circle2 = new Circle(0.3f, 25, material);
            // circle2.Transform.Move(Vector.Right * 0.5f);
            
            
            for (var i = 0; i < 30; i++) {
                var radius = GetRandomFloat(random, 0.05f, 0.15f);
                var circle = new Circle(radius * 2, 25, material);
                // circle.Transform.CurrentScale = new Vector(radius, radius, 1f);
                circle.Transform.Position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1), 0f);
                circle.velocity = -circle.Transform.Position.Normalize() * GetRandomFloat(random, 0.15f, 0.3f);
                circle.Mass = MathF.PI * radius * radius;
                scene.Add(circle);
            }

            // var square = new Rectangle(0.2f, 0.2f, material);
            // square.Transform.Move(Vector.Left + Vector.Backward * 0.3f);
            // square.Mass = 4f;
            // square.linearForce = Vector.Right * 0.3f;
            
            // var cone = new Cone(1, 50, 1, material);
            
            // var rectangle = new Rectangle(0.2f, 0.2f, material);
            // rectangle.Transform.Move(new Vector(0,-.5f,0));
            
            
            // scene.Add(newtriangle);
            // scene.Add(circle);
            // scene.Add(circle2);
            // scene.Add(cone);
            // scene.Add(rectangle);
            
            // circle.Transform.Move(new Vector(0.2f,0,0));Ho


            // var ground = new Rectangle(2f, 0.1f, material);
            // ground.Transform.Move(new Vector(0,-0.95f,0));
            // // ground.Mass = float.PositiveInfinity;
            // ground.gravityScale = 0f;
            // scene.Add(ground);
            //

            // engine rendering loop
            var direction = new Vector(0.01f, 0.01f);
            var multiplier = 0.95f;
            // var rotation = 0.05f;

            const float MaxColorRange = 1;
            const double piColorRelation = MaxColorRange / Math.PI;
            const int FixedFramerate = 30;
            const float FrameTime = 1.0f / FixedFramerate;
            double NextFrameTimeTarget = 0.0;

            float movementSpeed = 0.5f;
            
            
            while (window.IsOpen()) {

                if (Glfw.Time > NextFrameTimeTarget)
                {
                    NextFrameTimeTarget += FrameTime;
                    // Console.WriteLine(Glfw.Time);
                    
                    physics.Update(FrameTime);
                    
                    window.Render();
                }
            }
        }
    }
}















//                     var walkDirection = new Vector();
//
//                     if (window.GetKey(Keys.W))
//                     {
//                         walkDirection += newtriangle.Transform.Forward;
//                     }
//                     if (window.GetKey(Keys.S))
//                     {
//                         walkDirection +=newtriangle.Transform.Backward;
//                     }
//                     if (window.GetKey(Keys.Q))
//                     {
//                         walkDirection += newtriangle.Transform.Left;
//                     }
//                     if (window.GetKey(Keys.E))
//                     {
//                         walkDirection += newtriangle.Transform.Right;
//                     }
//                     if (window.GetKey(Keys.A))
//                     {
//                         var rotation = newtriangle.Transform.Rotation;
//                         rotation.z += 2 * MathF.PI * (float)FrameTime;
//                         newtriangle.Transform.Rotation = rotation;
//                     }
//                     if (window.GetKey(Keys.D))
//                     {
//                         var rotation = newtriangle.Transform.Rotation;
//                         rotation.z -= 2 * MathF.PI * (float)FrameTime;
//                         newtriangle.Transform.Rotation = rotation;
//                     }
//                     
//                     # region The Scared Rectangle
//
//                     rectangle.SetColor(Vector.Dot(newtriangle.Transform.Forward, rectangle.GetCenter()) < 0 ?
//                         Color.Green : Color.Red
//                         );
//
//                     #endregion
//                     
//                     
//                     // Scale the color of the circle with the angle between the triangles forward and the circles position.
//                     circle.SetColor(new Color(MaxColorRange - Vector.GetAngleTo(newtriangle.Transform.Position - circle.Transform.Position, newtriangle.Transform.Forward) * (float)piColorRelation));
//
//
//
//                     walkDirection = walkDirection.Normalize();
//
//                     newtriangle.Transform.Position += walkDirection * movementSpeed * (float)FrameTime;
//                     
