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
        
        public Triangle(Vector value)
        {
            // Put the center of the triangle at the desired offset.
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x += value.x;
                vertices[i].y += value.y;
                vertices[i].z += value.z;
            }

            centerPoint = value;

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
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector temp = new Vector(vertices[i].x, vertices[i].y);
                
                vertices[i].x = (float) Math.Cos(degrees) * (temp.x - centerPoint.x) -
                    (float)Math.Sin(degrees) * (temp.y - centerPoint.y) + centerPoint.x;
                vertices[i].y = (float) Math.Sin(degrees) * (temp.x - centerPoint.x) +
                    (float)Math.Cos(degrees) * (temp.y - centerPoint.y) + centerPoint.y;
            }
        }

        private Vector centerPoint = new Vector(0, 0, 0);

        Vector[] vertices = new[]
        {
            // new Vector(-.1f,-.1f),
            // new Vector(.1f,-.1f),
            // new Vector(0f,.1f)
            new Vector(-.1f,-.07f),
            new Vector(.1f,-.07f),
            new Vector(0f,.123f)
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
            triangles.Add(new Triangle(new Vector(0,0,0)));
            // triangles.Add(new Triangle(new Vector(0,0,0)));
            triangles.Add(new Triangle(new Vector(-.3f,0,0)));
            // triangles.Add(new Triangle(new Vector(.3f,0,0)));

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
                triangles[0].Rotate(0.01f);
                // triangles[1].Rotate(0.01f);

                angle += 0.001f;
                if (angle >= 3.14f)
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