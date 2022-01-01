
namespace CEServerPS4
{
    class Program
    {


        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            CheatEngineServer server = new CheatEngineServer();

            //server.Start();
            server.StartAsync().Wait();

        }
    }
   
}
