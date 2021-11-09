using System;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            
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

            // Close the window if the X button is clicked.
            while (!Glfw.WindowShouldClose(window))
            {
                // Make window interactable and make it interact with the OS.
                Glfw.PollEvents();
                
                // Draw the array:
                glDrawArrays(GL_TRIANGLES, 0, 3);
                Glfw.SwapBuffers(window);
            }
        }
    }
}
