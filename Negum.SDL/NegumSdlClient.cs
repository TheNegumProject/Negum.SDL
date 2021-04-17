using System;
using static SDL2.SDL;

namespace Negum.SDL
{
    /// <summary>
    /// Main client which contains logic required to start window, initial game loop, etc.
    /// </summary>
    /// 
    /// <author>
    /// https://github.com/TheNegumProject/Negum.SDL
    /// </author>
    public class NegumSdlClient
    {
        /// <summary>
        /// Pointer to the main window.
        /// </summary>
        protected IntPtr WindowPtr { get; set; }

        /// <summary>
        /// Initializes SDL library.
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public virtual void Initialize()
        {
            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                throw new SystemException($"Error when initializing SDL: \"{SDL_GetError()}\"");
            }
        }

        /// <summary>
        /// Creates window with context.
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public virtual void CreateWindow()
        {
            var title = this.GetTitle();

            this.WindowPtr = SDL_CreateWindow(
                title,
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                1020,
                800,
                SDL_WindowFlags.SDL_WINDOW_SHOWN |
                SDL_WindowFlags.SDL_WINDOW_OPENGL |
                SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            if (this.WindowPtr == IntPtr.Zero)
            {
                throw new SystemException($"Error when creating a window: \"{SDL_GetError()}\"");
            }
        }

        /// <summary>
        /// Starts the main loop.
        /// </summary>
        public virtual void Start()
        {
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
        }

        /// <summary>
        /// Clears everything on closing.
        /// </summary>
        public virtual void Clear()
        {
            SDL_DestroyWindow(this.WindowPtr);
            SDL_Quit();
        }

        /// <summary>
        /// </summary>
        /// <returns>Window title.</returns>
        protected virtual string GetTitle() =>
            NegumSdlTitleBuilder.Build();
    }
}