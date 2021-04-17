using System;
using static SDL2.SDL;

namespace Negum.SDL
{
    /// <summary>
    /// </summary>
    /// 
    /// <author>
    /// https://github.com/TheNegumProject/Negum.SDL
    /// </author>
    class Program
    {
        static void Main(string[] args)
        {
            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine($"Error occurred: {SDL_GetError()}");
                return;
            }

            var window = SDL_CreateWindow(
                "Sample Window Test",
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                1020,
                800,
                SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL_WindowFlags.SDL_WINDOW_OPENGL |
                SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine($"Error occurred: {SDL_GetError()}");
                return;
            }

            SDL_Event e;
            var quit = false;

            while (!quit)
            {
                while (SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;
                        case SDL_EventType.SDL_KEYDOWN:
                            switch (e.key.keysym.sym)
                            {
                                case SDL_Keycode.SDLK_q:
                                    quit = true;
                                    break;
                            }

                            break;
                    }
                }
            }

            SDL_DestroyWindow(window);

            SDL_Quit();
        }
    }
}
