using System.Collections.Generic;
using Server.Common;

namespace Server.Core
{
    public class KMeans
    {
        public static List<PointCollection> DoClustering(PointCollection points, int clusterCount, int dimension)
        {
            var allClusters = new List<PointCollection>();
            List<List<Point>> allGroups = ListUtility.SplitList(points, clusterCount);

            foreach (List<Point> group in allGroups)
            {
                var cluster = new PointCollection(dimension);
                cluster.AddRange(group);
                allClusters.Add(cluster);
            }

            int movements = 1;
            while (movements > 0)
            {
                movements = 0;

                foreach (PointCollection cluster in allClusters)
                {
                    for (int pointIndex = 0; pointIndex < cluster.Count; pointIndex++)
                    {
                        Point point = cluster[pointIndex];

                        int nearestCluster = FindNearestCluster(allClusters, point, dimension);
                        if (nearestCluster != allClusters.IndexOf(cluster) && cluster.Count > 1)
                        {
                            Point removedPoint = cluster.RemovePoint(point);
                            allClusters[nearestCluster].AddPoint(removedPoint);
                            movements += 1;
                        }
                    }
                }
            }

            return allClusters;
        }

        private static int FindNearestCluster(List<PointCollection> allClusters, Point point, int dimension)
        {
            double minimumDistance = 0.0;
            int nearestClusterIndex = -1;

            for (int clusterIndex = 0; clusterIndex < allClusters.Count; clusterIndex++)
            {
                double distance = Point.FindDistance(point, allClusters[clusterIndex].Centroid, dimension);
                if (clusterIndex == 0)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = 0;
                }
                else if (minimumDistance > distance)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = clusterIndex;
                }
            }

            return nearestClusterIndex;
        }
    }
}
