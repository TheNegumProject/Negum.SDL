using System;
using System.Linq;
using Negum.Core.Containers;
using Negum.Game.Client;
using Negum.Game.Client.Screen;
using Negum.Game.Common.Containers;
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
        /// Negum Client.
        /// </summary>
        protected INegumClient Client { get; set; }

        /// <summary>
        /// Pointer to the main window.
        /// </summary>
        protected IntPtr WindowPtr { get; set; }

        /// <summary>
        /// Pointer to the main renderer.
        /// </summary>
        protected IntPtr RendererPtr { get; set; }

        /// <summary>
        /// Initializes SDL library.
        /// </summary>
        /// <exception cref="SystemException"></exception>
        /// <param name="negumDirPath">Path to Negum directory.</param>
        public virtual void Initialize(string negumDirPath)
        {
            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                throw new SystemException($"Error when initializing SDL: \"{SDL_GetError()}\"");
            }

            NegumContainer.RegisterKnownTypes();
            NegumGameContainer.RegisterKnownTypes();

            this.Client = NegumClientFactory.CreateAsync(negumDirPath).Result;
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

            this.RendererPtr = SDL_CreateRenderer(this.WindowPtr, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            if (this.RendererPtr == IntPtr.Zero)
            {
                throw new SystemException($"Error when creating a renderer: \"{SDL_GetError()}\"");
            }
        }

        /// <summary>
        /// Starts the main loop.
        /// </summary>
        public virtual void Start()
        {
            // TODO: this.Client.StartAsync().Wait();

            SDL_Event e;
            var quit = false;

            while (!quit)
            {
                // Handle events on queue
                while (SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;
                        case SDL_EventType.SDL_KEYDOWN:
                            // TODO: this.Client.Hooks.OnKeyPressed(new [] { (int)e.key.keysym.sym });
                            break;
                    }
                }

                // Clear screen
                SDL_RenderClear(this.RendererPtr);

                // Render textures / sprites to screen
                // TODO: this.Client.Hooks.Render(this.Render);

                // Update screen
                SDL_RenderPresent(this.RendererPtr);
            }
        }

        /// <summary>
        /// Clears everything on closing.
        /// </summary>
        public virtual void Clear()
        {
            // TODO: this.Client.StopAsync().Wait();

            SDL_DestroyWindow(this.WindowPtr);
            SDL_Quit();
        }

        /// <summary>
        /// </summary>
        /// <returns>Window title.</returns>
        protected virtual string GetTitle() =>
            NegumSdlTitleBuilder.Build();

        /// <summary>
        /// Contains logic which renders data to screen.
        /// </summary>
        protected virtual void Render(RenderContext ctx)
        {
            // TODO: How to render multiple textures / sprites to the screen ???

            foreach (var layerEntry in ctx.OrderBy(x => x.Key))
            {
                foreach (var spriteCtx in layerEntry.Value)
                {
                    // TODO: Render sprite
                }
            }

            // unsafe
            // {
            //     var files = this.Client.Engine.Characters.ElementAt(1).Sprite.SpriteSubFiles;
            //     var pixels = files.ElementAt(150).Image.ToArray();
            //
            //     fixed (void* p = &pixels[0])
            //     {
            //         var surfacePtr = SDL_CreateRGBSurfaceFrom(new IntPtr(p), 128, 128, 24, 128 * 24, 0x0000FF, 0x00FF00, 0xFF0000, 0);
            //         var texturePtr = SDL_CreateTextureFromSurface(this.RendererPtr, surfacePtr);
            //         SDL_RenderCopy(this.RendererPtr, texturePtr, IntPtr.Zero, IntPtr.Zero);
            //         SDL_RenderPresent(this.RendererPtr);
            //     }
            // }
        }
    }
}