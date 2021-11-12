﻿using System.IO;
using System.Runtime.InteropServices;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {

        static Triangle triangle = new Triangle (
            new Vertex[] {
                new Vertex(new Vector(-.1f, -.07f), Color.Red),
                new Vertex(new Vector(.1f, -.07f), Color.Green),
                new Vertex(new Vector(.0f, .125f), Color.Blue)
            }, new Vector(0.5f,-.2f,0f)
        );
        
        static Triangle triangle2 = new Triangle (
            new Vertex[] {
                new Vertex(new Vector(-.1f, -.07f), Color.Red),
                new Vertex(new Vector(.1f, -.07f), Color.Green),
                new Vertex(new Vector(.0f, .125f), Color.Blue)
            }, new Vector(0.5f,-.2f,0f)
        );
        
        // static Shape triangle2 = new Shape (
        //     new Vertex[] {
        //         new Vertex(new Vector(0f, 0f), Color.Red),
        //         new Vertex(new Vector(0.5f, 0f), Color.Green),
        //         new Vertex(new Vector(0f, 0.5f), Color.Blue)
        //     }
        // );
        
        static Shape triangle3 = new Shape (
            new Vertex[] {
                new Vertex(new Vector(0f, 0f), Color.Red),
                new Vertex(new Vector(-0.5f, 0f), Color.Green),
                new Vertex(new Vector(0f, -0.5f), Color.Blue)
            }
        );
        
        static Rectangle rectangle = new Rectangle(0.8f, 0.4f, new Vector(0f, 0f, 0f));

        private static Circle circle = new Circle(1, 100,new Vector(-0.5f, 0, 0));

        private static Cone cone = new Cone(0.25f, 25, 0.3f, new Vector(0.3f, -0.5f, 0f));
        
        static void Main(string[] args) {
            
            var window = CreateWindow();

            // LoadTriangleIntoBuffer();

            CreateShaderProgram();

            
            

            // engine rendering loop
            var direction = new Vector(0.0003f, 0.0003f);
            var multiplier = 0.999f;
            float currentRotation = 0;
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                
                // triangle.Scale(multiplier);
                // triangle2.Scale(multiplier);

                // 2. Keep track of the Scale, so we can reverse it
                if (triangle.CurrentScale <= 0.5f) {
                    multiplier = 1.001f;
                }
                if (triangle.CurrentScale >= 1f) {
                    multiplier = 0.999f;
                }

                // 3. Move the Triangle by its Direction
                // triangle.Move(direction);
                
                // 4. Check the X-Bounds of the Screen
                if (triangle.GetMaxBounds().x >= 1 && direction.x > 0 || triangle.GetMinBounds().x <= -1 && direction.x < 0) {
                    direction.x *= -1;
                }
                
                // 5. Check the Y-Bounds of the Screen
                if (triangle.GetMaxBounds().y >= 1 && direction.y > 0 || triangle.GetMinBounds().y <= -1 && direction.y < 0) {
                    direction.y *= -1;
                }

                // triangle2.Rotate(90);
                triangle.Rotate(currentRotation);
                rectangle.Rotate(currentRotation);
                // circle.Rotate(currentRotation);
                currentRotation += 0.1f;
                if (currentRotation >= 360)
                {
                    currentRotation = 0;
                }
            }
        }
        static void Render(Window window) {
            triangle.Render();
            triangle2.Render();
            rectangle.Render();
            circle.Render();
            cone.Render();
            // triangle3.Render();
            Glfw.SwapBuffers(window);
        }

        static void ClearScreen() {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/position-color.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }


        static Window CreateWindow() {
            // initialize and configure
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}