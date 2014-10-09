using System;
using System.IO;
using Server.Services;

namespace Server
{
    public class Program
    {
        private const string Host = "http://localhost:8080";

        public static void Main(string[] args)
        {
            //Database.Create();

            var webServer = new Server(Host,
                new LoginService(),
                new SignUpService(),
                new SessionManagerService(),
                new DatabaseService()
            );

            Console.WriteLine("Server starting...");
            webServer.Run();
            Console.WriteLine("Server started successful.");

            Console.WriteLine();

            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();

            Console.WriteLine();

            Console.WriteLine("Server stopping...");
            webServer.Stop();
        }
    }
}
