using System;
using System.Text;

namespace Server.Common
{
    public class Point
    {
        public int Id { get; set; }
        public double[] Coordinates { get; set; }
        public int Dimension { get; private set; }

        public Point(int dimension)
        {
            Id = -1;
            Coordinates = new double[dimension];
            Dimension = dimension;
        }

        public Point(int id, params double[] coordinates)
        {
            Id = id;
            Coordinates = coordinates;
            Dimension = coordinates.Length;
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;

            if (point == null || Dimension != point.Dimension)
            {
                return false;
            }

            for (var index = 0; index < Dimension; index++)
            {
                if (Math.Abs(Coordinates[index] - point.Coordinates[index]) > 0.0)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var bits = 0L;

            foreach (var coordinate in Coordinates)
            {
                bits ^= BitConverter.DoubleToInt64Bits(coordinate) * 31;
            }

            return (((int)bits) ^ ((int)(bits >> 32)));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('{');
            for (var index = 0; index < Coordinates.Length; index++)
            {
                stringBuilder.Append(Coordinates[index]);
                if (index != Coordinates.Length - 1)
                {
                    stringBuilder.Append(',');
                }
            }
            stringBuilder.Append('}');

            return stringBuilder.ToString();
        }

        public static double FindDistance(Point pt1, Point pt2, int dimension)
        {
            if (pt1.Dimension != dimension || pt2.Dimension != dimension)
            {
                throw new Exception("Points has different dimensions");
            }

            var squaresSum = 0.0;

            for (var index = 0; index < dimension; index++)
            {
                squaresSum += Math.Pow(pt1.Coordinates[index] - pt2.Coordinates[index], 2.0);
            }

            return Math.Sqrt(squaresSum);
        }
    }
}
