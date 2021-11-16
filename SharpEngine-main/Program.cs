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
                    new Vertex(new Vector(-.1f, 0f), Color.Red),
                    new Vertex(new Vector(.1f, 0f), Color.Green),
                    new Vertex(new Vector(0f, .133f), Color.Blue)
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
            // var newtriangle = new Triangle(new Vertex[] {
            //     new Vertex(new Vector(-.1f, 0f), Color.Red),
            //     new Vertex(new Vector(.1f, 0f), Color.Green),
            //     new Vertex(new Vector(0f, .133f), Color.Blue)
            // }, material);

            // var circle = new Circle(0.3f, 25, material);
            // var cone = new Cone(1, 50, 1, material);
            var rectangle = new Rectangle(0.4f, 0.4f, material);
            
            // scene.Add(newtriangle);
            // scene.Add(circle);
            // scene.Add(cone);
            scene.Add(rectangle);
            
            // circle.Transform.Move(new Vector(0.2f,0,0));Ho


            // engine rendering loop
            var direction = new Vector(0.01f, 0.01f);
            var multiplier = 0.95f;
            var rotation = 0.05f;

            const int fixedStepNumberPerSecond = 30;
            const double fixedStepDuration = 1.0 / fixedStepNumberPerSecond; 
            double previousFixedStep = 0.0;

            const int FixedFramerate = 2;
            const double FrameTime = 1.0 / FixedFramerate;
            double NextFrameTimeTarget = 0.0;
            
            // newtriangle.Transform.Move(new Vector(0.5f,0f));
            
            while (window.IsOpen()) {

                if (Glfw.Time > NextFrameTimeTarget)
                {
                    NextFrameTimeTarget += FrameTime;
                    Console.WriteLine(Glfw.Time);
                    
                    // Update Triangles
                    for (var i = 0; i < scene.triangles.Count; i++) 
                    {
                        var triangle = scene.triangles[i];
                
                        // 2. Keep track of the Scale, so we can reverse it
                        if (triangle.Transform.CurrentScale.x <= 0.5f) {
                            multiplier = 1.05f;
                        }
                        if (triangle.Transform.CurrentScale.x >= 1f) {
                            multiplier = 0.95f;
                        }
                    
                        triangle.Transform.Scale(new Vector(multiplier,multiplier,1));
                        triangle.Transform.Rotate(new Vector(0,0,rotation));
                    
                    
                        // 4. Check the X-Bounds of the Screen
                        if (triangle.GetMaxBounds().x >= 1 && direction.x > 0 || triangle.GetMinBounds().x <= -1 && direction.x < 0) {
                            direction.x *= -1;
                        }
                
                        // 5. Check the Y-Bounds of the Screen
                        if (triangle.GetMaxBounds().y >= 1 && direction.y > 0 || triangle.GetMinBounds().y <= -1 && direction.y < 0) {
                            direction.y *= -1;
                        }

                        triangle.Transform.Move(direction);
                    }
                
                
                    window.Render();
                }
                
                // TODO ------------------------------------------------------------------------------------------------
                // Macs framerate locker. 
                if (Glfw.Time > previousFixedStep + fixedStepDuration)
                {
                    previousFixedStep = Glfw.Time;
                    
                    // Console.WriteLine(Glfw.Time);

                }
            }
        }
    }
}
