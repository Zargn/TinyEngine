using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GLFW;
using static OpenGL.Gl;

namespace Engine_Protoype
{
    struct Vector {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }
    }
    
    
    class Triangle
    {
        public void AddToPipeline()
        {
            vertices.CopyTo(globalVertices, 3 * id);
        }
    
        public static int NumberOfTriangles = 0;
        
        private static Vector[] globalVertices = new Vector[3];

        public static unsafe void Render()
        {
            fixed (Vector* vertex = &globalVertices[0])
            {
                // Put the updated data in the buffer.
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * globalVertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
        
        private static uint nextId;
        private uint id;
        
        public Triangle(float x, float y, float z)
        {
            // Put the center of the triangle at the desired offset.
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x += x;
                vertices[i].y += y;
                vertices[i].z += z;
            }

            id = nextId;
            nextId++;
    
            NumberOfTriangles++;
            
            Array.Resize(ref globalVertices, globalVertices.Length + vertices.Length);
            
            createTriangle();
        }
    
        unsafe void createTriangle()
        {
            glVertexAttribPointer(id, 3, GL_FLOAT, false, 3 * sizeof(float), null);
    
            glEnableVertexAttribArray(id);
        }

        public void Rotate(float degrees)
        {
            // List<float> temp = new List<float>();
            // vertices.CopyTo(s
            // temp = null;
            
            // x
            // vertices[4] = (float) Math.Cos(degrees);
            // vertices[5] = (float) - Math.Sin(degrees);
            // vertices[7] = (float) Math.Sin(degrees);
            // vertices[8] = (float) Math.Cos(degrees);

            // y
            // vertices[0] = (float) Math.Cos(degrees);
            // vertices[2] = (float) Math.Sin(degrees);
            // vertices[6] = (float) - Math.Sin(degrees);
            // vertices[8] = (float) Math.Cos(degrees);

            // z
            // vertices[0] += (float) Math.Cos(degrees);
            // vertices[1] += (float) - Math.Sin(degrees);
            // vertices[3] += (float) Math.Sin(degrees);
            // vertices[4] += (float) Math.Cos(degrees);
            // vertices[8] += 1;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = vertices[i].x * (float)Math.Cos(degrees) - vertices[i].y * (float)Math.Sin(degrees);
                vertices[i].y = vertices[i].x * (float)Math.Sin(degrees) + vertices[i].y * (float)Math.Cos(degrees);
            }
            
            
            // for (var i = 0; i < vertices.Length; i++)
            // {
            //     vertices[i].x = (float)(vertices[i].x * Math.Cos(0.01f) + vertices[i].y * Math.Sin(0.01f));
            //     vertices[i].y = (float)(vertices[i].y * Math.Cos(0.01f) - vertices[i].x * Math.Sin(0.01f));  
            // }
            
            // xf = cx + (int)((float)(px - cx) * cos(theta))
            //      - ((float)(py - cy) * sin(theta));
            // yf = cy + (int)((float)(px - cx) * sin(theta))
            //         + ((float)(py - cy) * cos(theta));
            
            
            
        }
        
        Vector[] vertices = new[]
        {
            // new Vector(-.1f,-.1f),
            // new Vector(.1f,-.1f),
            // new Vector(0f,.1f)
            new Vector(-.4f,-.3f),
            new Vector(.4f,-.3f),
            new Vector(0f,.5f)
        };
    }


    class Program
    {
        static void Main(string[] args)
        {
            var window = CreateWindow();
            
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            
            
            List<Triangle> triangles = new List<Triangle>();
            triangles.Add(new Triangle(0, 0, 0));
            // triangles.Add(new Triangle(0, 0, 0));
            // triangles.Add(new Triangle(-.3f, 0, 0));
            // triangles.Add(new Triangle(.3f, 0, 0));

            CreateShaderProgram();

            float angle = 0;
            // Render loop close the window if the X button is clicked.
            while (!Glfw.WindowShouldClose(window))
            {
                // Make window interactable and make it interact with the OS.
                Glfw.PollEvents();
                
                ClearScreen();
                
                glDrawArrays(GL_TRIANGLES, 0, Triangle.NumberOfTriangles*3);
                
                glFlush();
                
                foreach (var triangle in triangles)
                {
                    triangle.AddToPipeline();
                }
                triangles[0].Rotate(angle);

                angle += 0.1f;
                if (angle >= 360)
                    angle = 0;
                Triangle.Render();
            }
        }

        private static void ClearScreen()
        {
            // Set clear color
            glClearColor(0, 0, 0, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        // # region Movement
        //
        // private const int vertexSize = 3;
        //
        // private const int vertexX = 0;
        // private const int vertexY = 1;
        // private const int vertexZ = 2;
        //
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
        //     for (int iteration = vertexX; iteration < vertices.Length; iteration += vertexSize)
        //     {
        //         vertices[iteration] += 0.0001f;
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
        // #endregion
        //
        // private static unsafe void LoadTriangleIntoBuffer()
        // {
        //     var vertexArray = glGenVertexArray();
        //     var vertexBuffer = glGenBuffer();
        //     glBindVertexArray(vertexArray);
        //     glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
        //     
        //     UpdateTriangleBuffer();
        //
        //     glVertexAttribPointer(0, 3, GL_FLOAT, false, vertexSize * sizeof(float), null);
        //
        //     glEnableVertexAttribArray(0);
        // }

        // static unsafe void UpdateTriangleBuffer()
        // {
        //     fixed (float* vertex = &vertices[0])
        //     {
        //         // Will put the data in the buffer.
        //         glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
        //     }
        // }


        private static void CreateShaderProgram()
        {
            // Create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("Shaders/ScreenCoordinate.vert"));
            glCompileShader(vertexShader);

            // Create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("Shaders/green.frag"));
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