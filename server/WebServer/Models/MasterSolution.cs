using System;
using System.Collections.Generic;
using Server.Common;
using Server.Core;

namespace Server.Models
{
    public class Suggestions
    {
        public string Problem { get; set; }
        public string Solution { get; set; }
    }

    public class MasterSolution
    {
        public class Port
        {
            public string Id { get; set; }
            public double VoltageValue { get; set; }
        }

        public int Id { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityAccessoryId { get; set; }
        public Port[] Ports { get; set; }
        public string Problem { get; set; }
        public string Solution { get; set; }

        public MasterSolution(string voltageValues)
        {
            var ports = voltageValues.Split(',');

            Ports = new Port[ports.Length];

            for (var index = 0; index < Ports.Length; index++)
            {
                var port = ports[index].Split('=');
                Ports[index] = new Port()
                {
                    Id = port[0],
                    VoltageValue = Double.Parse(port[1])
                };
            }
        }

        public static Suggestions[] GetSuggestions(MasterSolution[] masterSolutions, PointCollection points, Point mainPoint, int clustersNumber, int dimension)
        {
            List<PointCollection> allClusters = KMeans.DoClustering(points, clustersNumber, dimension);

            var solutions = new List<MasterSolution>();

            foreach (var cluster in allClusters)
            {
                if (cluster.IndexOf(mainPoint) == -1)
                {
                    continue;
                }

                foreach (var point in cluster)
                {
                    if (mainPoint.Id == point.Id)
                    {
                        continue;
                    }

                    foreach (var solution in masterSolutions)
                    {
                        if (solution.Id == point.Id)
                        {
                            solutions.Add(solution);
                        }
                    }
                }
            }

            var suggestions = new List<Suggestions>();
            foreach (var solution in solutions)
            {
                var suggestion = new Suggestions()
                {
                    Problem = solution.Problem,
                    Solution = solution.Solution
                };

                suggestions.Add(suggestion);
            }

            return suggestions.ToArray();
        }

        public static MasterSolution[] FindAll(int entityTypeId, int entityAccessoryId)
        {
            return Database.FindAllMasterSolutions(entityTypeId, entityAccessoryId);
        }

        public static void Add(int entityTypeId, int entityAccessoryId, string ports, string problem, string solution)
        {
            Database.AddMasterSolution(entityTypeId, entityAccessoryId, ports, problem, solution);
        }
    }
}
