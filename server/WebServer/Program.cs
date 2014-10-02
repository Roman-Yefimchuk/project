using System;
using WebServer.Services;

namespace WebServer
{

    class Program
    {
        static void Main(string[] args)
        {

            var webServer = new WebServer("http://localhost:8080", 
                new LoginService(),
                new SignUpService(),
                new SearchService()
            );

            webServer.Run();
            Console.WriteLine("WebServer started. Press a key to quit.");
            Console.ReadKey();
            webServer.Stop();
        }
    }
}
