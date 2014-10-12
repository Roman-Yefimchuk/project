using System;
using System.Collections.Generic;

namespace Server.Common
{
    public class PointCollection : List<Point>
    {
        public Point Centroid { get; set; }
        private readonly int _dimension;

        public PointCollection(int dimension)
        {
            Centroid = new Point(dimension);
            _dimension = dimension;
        }

        public void AddPoint(Point p)
        {
            if (p.Dimension != _dimension)
            {
                throw new Exception("Incompatible point dimension");
            }

            Add(p);
            UpdateCentroid();
        }

        public Point RemovePoint(Point p)
        {
            var removedPoint = new Point(p.Id, p.Coordinates);
            Remove(p);
            UpdateCentroid();

            return removedPoint;
        }

        private void UpdateCentroid()
        {
            var centroids = new double[_dimension];

            for (var pointIndex = 0; pointIndex < Count; pointIndex++)
            {
                var point = this[pointIndex];

                for (var index = 0; index < _dimension; index++)
                {
                    centroids[index] += point.Coordinates[index];
                }
            }

            for (var index = 0; index < _dimension; index++)
            {
                Centroid.Coordinates[index] = centroids[index] / Count;
            }
        }
    }
}
