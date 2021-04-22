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
                SDL_SetRenderDrawColor(this.RendererPtr, 255, 255, 255, 255);
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
            foreach (var layerEntry in ctx.OrderBy(x => x.Key))
            {
                foreach (var spriteCtx in layerEntry.Value)
                {
                    unsafe
                    {
                        var pixels = spriteCtx.Image.ToArray();
                    
                        fixed (void* p = &pixels[0])
                        {
                            var texturePtr = SDL_CreateTexture(this.RendererPtr, SDL_PIXELFORMAT_ABGR8888,
                                (int) SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, spriteCtx.Width, spriteCtx.Height);
                    
                            var rect = new SDL_Rect
                            {
                                w = spriteCtx.Width * ctx.Scale,
                                h = spriteCtx.Height * ctx.Scale,
                                x = spriteCtx.PosX,
                                y = spriteCtx.PosY
                            };
                    
                            SDL_SetRenderTarget(this.RendererPtr, texturePtr);
                            SDL_UpdateTexture(texturePtr, IntPtr.Zero, new IntPtr(p), spriteCtx.Width * 4);
                            SDL_RenderCopy(this.RendererPtr, texturePtr, IntPtr.Zero, ref rect);
                        }
                    }
                }
            }
        }
    }
}