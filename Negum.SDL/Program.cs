using System;

namespace Negum.SDL
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
        static void Main(string[] args)
        {
            // TODO: This path needs to be provided in some configurable way...
            const string temporaryPath = "/Users/kdobrzynski/Downloads/UnpackedMugen-main";

            var client = new NegumSdlClient();

            Console.WriteLine($"Loading directory: {temporaryPath}");

            client.Initialize(temporaryPath);

            Console.WriteLine("Directory loaded.");

            client.CreateWindow();
            client.Start();
            client.Clear();
        }
    }
}