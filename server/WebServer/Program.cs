using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Server.Common;
using Server.Core;
using Server.Services;

namespace Server
{
    public class Program
    {
        private const string Host = "http://localhost:8080";

        public static void Main(string[] args)
        {
            //Database.Create();

/*                        const int dimension = 2;
                        var points = new PointCollection(dimension)
                                                {
                                                    new Point(0, 1, 1),
                                                    new Point(1, 5, 4),
                                                    new Point(2, 2, 1),
                                                    new Point(3, 4, 3),
                                                    new Point(4, 3, 2)
                                                };

                        List<PointCollection> allClusters = KMeans.DoClustering(points, 2, dimension);

                        foreach (var cluster in allClusters)
                        {
                            string json = JsonConvert.SerializeObject(cluster);
                            Console.WriteLine(json);                
                        }

                        Console.ReadKey();*/

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
