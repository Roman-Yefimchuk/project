using System;
using WebServer.Services;

namespace WebServer
{

    public class Program
    {
        public static void Main(string[] args)
        {

            var webServer = new WebServer("http://localhost:8080", 
                new LoginService(),
                new SignUpService(),
                new SearchService(),
                new SessionManagerService()
            );

            Console.WriteLine("Connecting to database...");
            Database.GetInstance();
            Console.WriteLine("Connected to database successful.");

            Console.WriteLine("WebServer starting...");
            webServer.Run();
            Console.WriteLine("WebServer started successful.");

            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();

            Console.WriteLine("WebServer stopping...");
            webServer.Stop();
        }
    }
}
