using System;
using Negum.Core.Containers;
using Negum.Game.Common.Containers;

namespace Negum.SDL.Client
{
    /// <summary>
    /// Standard runner for Negum SDL Client.
    /// </summary>
    /// 
    /// <author>
    /// https://github.com/TheNegumProject/Negum.SDL
    /// </author>
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintMissingPathMessage();
                return;
            }

            var gameFullPath = args[0];

            // We need to initialize containers at the beginning
            NegumContainer.RegisterKnownTypes();
            NegumGameContainer.RegisterKnownTypes();

            var client = new NegumSdlClient();

            Console.WriteLine($"Loading directory: {gameFullPath}");

            client.Initialize(gameFullPath);

            Console.WriteLine("Directory loaded.");

            client.CreateWindow();
            client.Start();
            
            client.Clear();
        }

        private static void PrintMissingPathMessage()
        {
            Console.WriteLine("Please provide full path to the game files directory as first argument.");
            Console.WriteLine("Game files directory should contain sub-directories like: chars, data, font, stages, sound, etc.");
        }
    }
}