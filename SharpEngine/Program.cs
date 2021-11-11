using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    // class Triangle
    // {
    //     public void AddToPipeline()
    //     {
    //         float[] temp = new float[GlobalVertices.Length + vertices.Length];
    //         vertices.CopyTo(temp, 0);
    //         GlobalVertices.CopyTo(temp, 9);
    //         GlobalVertices = new float[temp.Length];
    //         temp.CopyTo(GlobalVertices, 0);
    //         // fixed (float* vertex = &vertices[0])
    //         // {
    //         //     // Will put the data in the buffer.
    //         //     glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
    //         //     // glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
    //         // }
    //         Console.WriteLine("Vertices:");
    //         foreach (float f in GlobalVertices)
    //         {
    //             Console.WriteLine(f);
    //         }
    //     }
    //
    //     public static int NumberOfTriangles = 0;
    //     
    //     private static float[] GlobalVertices = new float[9];
    //
    //     public static unsafe void Render()
    //     {
    //         fixed (float* vertex = &GlobalVertices[0])
    //         {
    //             // Will put the data in the buffer.
    //             glBufferData(GL_ARRAY_BUFFER, sizeof(float) * GlobalVertices.Length, vertex, GL_STATIC_DRAW);
    //             // glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
    //         }
    //     }
    //     
    //     private static uint nextId;
    //     private uint id;
    //     
    //     public Triangle(float x, float y, float z)
    //     {
    //         vertices[0] += x;
    //         vertices[1] += y;
    //         vertices[2] += z;
    //         vertices[3] += x;
    //         vertices[4] += y;
    //         vertices[5] += z;
    //         vertices[6] += x;
    //         vertices[7] += y;
    //         vertices[8] += z;
    //
    //         id = nextId;
    //         nextId++;
    //
    //         NumberOfTriangles++;
    //         
    //         createTriangle();
    //     }
    //
    //     unsafe void createTriangle()
    //     {
    //         // Render();
    //
    //         glVertexAttribPointer(id, 3, GL_FLOAT, false, 3 * sizeof(float), null);
    //
    //         glEnableVertexAttribArray(id);
    //     }
    //     
    //     float[] vertices = new[]
    //     {
    //         // Vertex 1
    //         -.5f, -.5f, 0f,
    //         // Vertex 2
    //         .5f, -.5f, 0f,
    //         // Vertex 3
    //         0f, .5f, 0f
    //     };
    // }
    
    
    public class Triangle
    {
        private Vertex[] vertices;
        
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
        }

        public void Scale(float multiplier)
        {
            var min = vertices[0].position;
            for (var i = 1; )
        }
    }
    
    
    
    
    class Program
    {
        private static Triangle triangle = new Triangle(
            new Vertex[] 
            {
                new Vertex(new Vector(-.1f, -.1f), new Color(0, 1, 1, 1)),
                new Vertex(new Vector(.1f, -.1f), new Color(1, 1, 0, 1)), 
                new Vertex(new Vector(0f, .1f), Color.Blue)
        });
        
        // static Vector[] vertices = new[]
        // {
        //     new Vector(-.1f, -.1f),
        //     new Vector(.1f, -.1f),
        //     new Vector(0f,.1f),
        //     new Vector(.4f, .4f),
        //     new Vector(.6f, .4f),
        //     new Vector(.5f,.6f)
        // };
        
        // static Vertex[] vertices = new Vertex[]
        // {
        //     new Vertex(new Vector(-.1f, -.1f), new Color(0,1,1,1)),
        //     new Vertex(new Vector(.1f, -.1f), new Color(1,1,0,1)),
        //     new Vertex(new Vector(0f, .1f), Color.Blue),
        //     
        //     // new Vertex(new Vector(0f, 0.4f), Color.Red),
        //     // new Vertex(new Vector(0.1f, 0.4f), Color.Green),
        //     // new Vertex(new Vector(0f, 0.5f), Color.Blue)
        // };

        private static Vector centerPoint = new Vector(0, 0);
        
        private static Vector centerPoint2 = new Vector(0, 0);

        static void Main(string[] args)
        {
            var window = CreateWindow();

            LoadTriangleIntoBuffer();
            
            LoadTriangleIntoBuffer();

            CreateShaderProgram();

            float movement = .001f;
            float rotationSpeed = .001f;

            centerPoint2.x = .5f;
            centerPoint2.y = .5f;

            Console.WriteLine(Vector.Distance(centerPoint2, centerPoint));

            Vector min = vertices[0].position;
            Vector max = vertices[0].position;
            for (int i = 0; i < vertices.Length / 2; i++)
            {
                min = Vector.Min(min, vertices[i].position);
                max = Vector.Max(max, vertices[i].position);
            }

            Vector newcenterPoint = (max + min) / 2;
            centerPoint = newcenterPoint;
            
             min = vertices[0].position;
             max = vertices[0].position;
            for (int i = 3; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
                max = Vector.Max(max, vertices[i].position);
            }

            newcenterPoint = (max + min) / 2;

            centerPoint2 = newcenterPoint;
            
            
            
            
            float scale = 1;
            float scaleMultiplier = 0.999f;
            
            // Render loop close the window if the X button is clicked.
            while (!Glfw.WindowShouldClose(window))
            {
                // Make window interactable and make it interact with the OS.
                Glfw.PollEvents();
                
                ClearScreen();

                Render(window);

                // Console.WriteLine(Vector.Distance(new Vector(0,0,0), centerPoint2));
                // Console.WriteLine(Vector.Distance(new Vector(0,0,0), centerPoint));

                // scaleY();
                // MoveRight();
                // MoveDown();
                // Shrink();
                // Grow();
                
                foreach (Vertex v in vertices)
                {
                    if (v.position.x >= 1 || v.position.y >= 1 || v.position.z >= 1)
                    {
                        movement = Math.Abs(movement) * -1;
                    }
                
                    if (v.position.x <= -1 || v.position.y <= -1 || v.position.z <= -1)
                    {
                        movement = Math.Abs(movement);
                    }
                }
                
                
                for (var i = 0; i < vertices.Length; i++)
                {
                    // vertices[i] *= 1.0001f;
                    vertices[i].position += new Vector(movement, movement, 0);
                }
                
                centerPoint.y += movement;
                centerPoint2.y += movement;
                centerPoint.x += movement;
                centerPoint2.x += movement;
                

                for (int i = 0; i < vertices.Length / 2; i++)
                {
                    Vector temp = new Vector(vertices[i].position.x, vertices[i].position.y);
                
                    vertices[i].position.x = (float) Math.Cos(rotationSpeed) * (temp.x - centerPoint.x) -
                        (float)Math.Sin(rotationSpeed) * (temp.y - centerPoint.y) + centerPoint.x;
                    vertices[i].position.y = (float) Math.Sin(rotationSpeed) * (temp.x - centerPoint.x) +
                                             (float)Math.Cos(rotationSpeed) * (temp.y - centerPoint.y) + centerPoint.y;
                }

                // Change the scale.
                scale *= scaleMultiplier;
                if (scale > 1)
                {
                    scaleMultiplier = 0.999f;
                }
                if (scale < .5f)
                {
                    scaleMultiplier = 1.001f;
                }
                
                for (int i = 0; i < vertices.Length / 2; i++)
                {
                    float tempX;
                    float tempY;
                
                    tempX = vertices[i].position.x - centerPoint.x;
                    tempY = vertices[i].position.y - centerPoint.y;
                
                    tempX *= scaleMultiplier;
                    tempY *= scaleMultiplier;
                
                    tempX += centerPoint.x;
                    tempY += centerPoint.y;
                
                    vertices[i].position.x = tempX;
                    vertices[i].position.y = tempY;
                }
                

                UpdateTriangleBuffer();
            }
        }

        private static void Render(Window window)
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            Glfw.SwapBuffers(window);
        }
        
        private static void ClearScreen()
        {
            // Set clear color
            glClearColor(0, 0, 0, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        # region Movement

        private const int vertexSize = 3;

        private const int vertexX = 0;
        private const int vertexY = 1;
        private const int vertexZ = 2;

        // static void scaleY()
        // {
        //     for (int iteration = vertexY; iteration < vertices.Length; iteration += vertexSize)
        //     {
        //         vertices[iteration] *= 0.0001f;
        //     }
        // }
        //
        // static void MoveRight()
        // {
        //     for (var i = 0; i < vertices.Length; i++)
        //     {
        //         vertices[i].position.x += 0.0001f;
        //     }
        // }
        //
        // static void MoveDown()
        // {
        //     for (int iteration = vertexY; iteration < vertices.Length; iteration += vertexSize)
        //     {
        //         vertices[iteration] -= 0.0001f;
        //     }
        // }
        //
        // static void Shrink()
        // {
        //     for (int iteration = 0; iteration < vertices.Length; iteration++)
        //     {
        //         vertices[iteration] *= 0.9999f;
        //     }
        // }
        //
        // static void Grow()
        // {
        //     for (int iteration = 0; iteration < vertices.Length; iteration++)
        //     {
        //         vertices[iteration] *= 1.0001f;
        //     }
        // }
        #endregion

        private static unsafe void LoadTriangleIntoBuffer()
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            
            UpdateTriangleBuffer();

            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
        
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }

        static unsafe void UpdateTriangleBuffer()
        {
            fixed (Vertex* vertex = &vertices[0])
            {
                // Will put the data in the buffer.
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
        }


        private static void CreateShaderProgram()
        {
            // Create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("Shaders/position-color.vert"));
            glCompileShader(vertexShader);

            // Create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("Shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // rendering pipeline - create shader program
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }


        private static Window CreateWindow()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, GLFW.Window.None);

            // Tell OpenGL to render the window?
            Glfw.MakeContextCurrent(window);

            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}