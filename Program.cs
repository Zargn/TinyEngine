using System;
using GLFW;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new NativeWindow(800, 600, "MyWindowTitle"))
            {
                while (!window.IsClosing)
                {
                    window.SwapBuffers();
                
                    Glfw.PollEvents();
                }
            }
        }
    }
}
