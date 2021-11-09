using System;
using System.Data;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Check why this part is needed. The code still works with everything below commented out.
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);
            
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);

            // Tell OpenGL to render the window?
            Glfw.MakeContextCurrent(window);
            
            Import(Glfw.GetProcAddress);
            
            // Corner positions for triangle.
            float[] vertices = new[]
            {
                -.5f, -.5f, 0f,
                .5f, -.5f, 0f,
                0f, .5f, 0f
            };
            
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            unsafe 
            {
                // Create a pointer.
                fixed (float* vertex = &vertices[0])
                {
                    // Will put the data in the buffer.
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
                }

                glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), null);
            };
            glEnableVertexAttribArray(0);


            string vertexShaderSource = @"
#version 330 core
in vec3 pos;

void main()
{
    gl_Position = vec4(pos.x, pos.y, pos.z, 1.0)
}
";
            string fragmentShaderSource = @"
#version 330 core
out vec4 result;

void main()
{
    result = vec4(1, 0, 0, 1);
}
";
            
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, vertexShaderSource);
            glCompileShader(vertexShader);

            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, fragmentShaderSource);
            glCompileShader(fragmentShader);
            
            // rendering pipeline - create shader program
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
            
            // Close the window if the X button is clicked.
            // Render loop
            while (!Glfw.WindowShouldClose(window))
            {
                // Make window interactable and make it interact with the OS.
                Glfw.PollEvents();
                
                // Draw the array:
                glDrawArrays(GL_TRIANGLES, 0, 3);
                glFlush();
            }
        }
    }
}
