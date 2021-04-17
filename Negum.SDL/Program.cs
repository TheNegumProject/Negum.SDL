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
            var client = new NegumSdlClient();

            client.Initialize();
            client.CreateWindow();
            client.Start();
            client.Clear();
        }
    }
}