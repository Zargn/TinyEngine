using System;
using GLFW;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);

            // Tell OpenGL to render the window?
            Glfw.MakeContextCurrent(window);

            // Close the window if the X button is clicked.
            while (!Glfw.WindowShouldClose(window))
            {
                // Make window interactable and make it interact with the OS.
                Glfw.PollEvents();
            }
        }
    }
}
