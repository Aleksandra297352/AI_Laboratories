using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Algorytm_Astar
{
    public class DuplicateKeyComparer<TKey>
                 :
              IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as beeing greater
            else
                return result;
        }

        #endregion
    }
    class City
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Latitude { get; set; } //szerokość
        public double Longitude { get; set; } //długość
        public override string ToString()
        {
            return $"{Name}:{Latitude};{Longitude}";
        }
    }

    internal class Astar_City
    {
        public static void wsp(City city)
        {
            var a = 6378137.0;
            var b = 6356752.3;
            var e2 = 1 - ((b * b) / (a * a));
            var latr = city.Latitude / 90 * 0.5 * Math.PI; //latitude in radians
            var lonr = city.Longitude / 180 * Math.PI;  //longituede in radians
            var Nphi = a / Math.Sqrt(1 - e2 * Math.Sin(latr) * Math.Sin(latr));
            city.X = (Nphi + 0) * Math.Cos(latr) * Math.Cos(lonr);
            city.Y = (Nphi + 0) * Math.Cos(latr) * Math.Sin(lonr);

        }
        public double[,] CreateMatrixFromFile(string fileName, List<City> cities, string matrixGoogle)
        {
            var textLinesFromFile = File.ReadAllLines(fileName);
            var result = new double[cities.Count, cities.Count];
            var matrix = File.ReadAllLines(matrixGoogle);
            var resultGoogle = new double[cities.Count, cities.Count];

            for (var i = 0; i < matrix.Length; i++)
            {
                var splittedLine = matrix[i].Split(';');
                for (var j = 0; j < splittedLine.Length; j++)
                {
                    resultGoogle[i, j] = double.Parse(splittedLine[j], CultureInfo.InvariantCulture);
                }
            }
            foreach (var line in textLinesFromFile)
            {
                var splittedLine = line.Split(' ');
                var city1Google = splittedLine[0];
                var city2Google = splittedLine[1];
                var city1GoogleIndex = cities.IndexOf(cities.First(x => x.Name == city1Google));
                var city2GoogleIndex = cities.IndexOf(cities.First(x => x.Name == city2Google));
                result[city1GoogleIndex, city2GoogleIndex] = resultGoogle[city1GoogleIndex, city2GoogleIndex];
                result[city2GoogleIndex, city1GoogleIndex] = resultGoogle[city2GoogleIndex, city1GoogleIndex];

            }
            return result;
        }
        public List<City> ReadFile(string fileName)
        {
            var textLinesFromFile = File.ReadAllLines(fileName);
            var result = new List<City>();
            foreach (var line in textLinesFromFile)
            {
                var splittedLine = line.Split(';');
                var city = new City
                {
                    Name = splittedLine[0],
                    Longitude = double.Parse(splittedLine[1], CultureInfo.InvariantCulture),
                    Latitude = double.Parse(splittedLine[2], CultureInfo.InvariantCulture),

                };
                wsp(city);
                result.Add(city);
            }
            return result;
        }
        static double CalculateEuklidesDistance(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));  //w km
        }
        public double[,] NeighbourMatrix(List<City> cities)
        {
            var result = new double[cities.Count, cities.Count];

            for (var i = 0; i < cities.Count; i++)
            {
                for (var j = 0; j < cities.Count; j++)
                {
                    result[i, j] = CalculateEuklidesDistance(cities[i].X, cities[j].X, cities[i].Y, cities[j].Y);

                }
            }

            return result;
        }
        double FunctionG(City city1, City city2) //odleglosc w linii prostej z danego punktu do docelowego
        {
            var x1 = city1.X;
            var x2 = city2.X;
            var y1 = city1.Y;
            var y2 = city2.Y;
            return CalculateEuklidesDistance(x1, x2, y1, y2);
        }
        double FunctionH(List<int> citiesList, double[,] matrix) // suma kosztow sciezek od punktu startowego do obecnego
        {
            double suma = 0;
            for (var i = 0; i < citiesList.Count - 1; i++)
            {
                var index = citiesList[i];
                var index2 = citiesList[i + 1];
                suma += matrix[index, index2];
            }
            return suma;

        }
        double FunctionF(List<int> citiesList, double[,] matrix, City city1, City city2)
        {
            return FunctionG(city1, city2) + FunctionH(citiesList, matrix);
        }
        List<int> GetNeighbours(int cityIndex, double[,] matrix)
        {
            var result = new List<int>();

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[cityIndex, i] > 0)
                {
                    result.Add(i);
                }
            }

            return result;
        }
        public (string path, double cost) Astar(List<City> cities, int startCityIndex, int finalCityIndex, double[,] matrix)
        {
            var queue = new SortedList<double, List<int>>(new DuplicateKeyComparer<double>());

            queue.Add(FunctionF(new List<int> { startCityIndex, finalCityIndex }, matrix, cities[startCityIndex], cities[finalCityIndex]), new List<int> { startCityIndex });

            var closed = new List<List<int>>();
            while (true)
            {
                var cur = GetFirstNotInClosed(queue, closed);

                if (cur.Last() == finalCityIndex) return CreatePath(cur, cities, matrix);

                foreach (var sasiad in GetNeighbours(cur.Last(), matrix))
                {
                    var sciezkaDoSasiada = new List<int>(cur)
                    {
                        sasiad
                    };
                    var kosztDojsciaDoSasiada = FunctionF(sciezkaDoSasiada, matrix, cities[sasiad], cities[finalCityIndex]);
                    queue.Add(kosztDojsciaDoSasiada, sciezkaDoSasiada);
                }
                closed.Add(cur);
            }
        }

        private (string path, double cost) CreatePath(List<int> cur, List<City> cities, double[,] matrix)
        {
            var koszt = FunctionH(cur, matrix);
            var result = "";
            for (var i = 0; i < cur.Count; i++)
            {

                result += cities[cur[i]].Name;
                if (i != cur.Count - 1)
                {
                    result += "→";
                }
            }
            return (result, koszt);
        }

        private List<int> GetFirstNotInClosed(SortedList<double, List<int>> queue, List<List<int>> closed)
        {
            while (queue.Count > 0)
            {
                var element = queue.First();
                queue.RemoveAt(0);
                if (closed.Any(x => ComparePaths(x, element.Value)) == false) return element.Value;
            }
            return null;
        }

        private bool ComparePaths(List<int> path1, List<int> path2)
        {
            if (path1.Count != path2.Count) return false;

            for (var i = 0; i < path1.Count; i++)
            {
                if (path1[i] != path2[i]) return false;
            }
            return true;
        }
    }
}
