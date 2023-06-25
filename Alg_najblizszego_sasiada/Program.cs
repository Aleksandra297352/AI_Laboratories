using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

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
class NajblizszySasiad
{
    static List<City> ReadFile(string fileName)
    {
        var textLinesFromFile = File.ReadAllLines(fileName);
        var result = new List<City>();
        foreach (var line in textLinesFromFile)
        {
            var splittedLine = line.Split(';');
            result.Add(new City
            {
                Name = splittedLine[0],
                Longitude = double.Parse(splittedLine[1], CultureInfo.InvariantCulture),
                Latitude = double.Parse(splittedLine[2], CultureInfo.InvariantCulture)
            });
        }
        return result;
    }

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
    static double CalculateEuklidesDistance(double x1, double x2, double y1, double y2)
    {
        return Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2))) / 1000;  //w km
    }
    static double[,] NeighbourMatrix(List<City> cities)
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
    public static (string path, double cost) NearestNeighbour(List<City> cities, int startCityIndex, double[,] matrix)
    {
        var resultPath = new List<int>() { startCityIndex };
        var minimum = double.MaxValue;
        var minimumIndex = int.MaxValue;

        var currentIndex = startCityIndex;
        var finalCost = 0d;
        do
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                if (currentIndex == j || resultPath.Contains(j)) continue;
                if (matrix[currentIndex, j] < minimum)
                {
                    minimumIndex = j;
                    minimum = matrix[currentIndex, j];
                }

            }
            if (resultPath.Contains(minimumIndex) == false)
            { // jezeli nowy element nie byl juz uzyty
                resultPath.Add(minimumIndex);
                finalCost += matrix[currentIndex, minimumIndex];
            }
            minimum = double.MaxValue;
            currentIndex = minimumIndex;
        } while (resultPath.Count < cities.Count);

        finalCost += matrix[resultPath.Last(), startCityIndex];
        var path = "";

        foreach (var i in resultPath)
        {
            path += cities[i].Name + $"-->";
        }
        return (path + cities[startCityIndex].Name,finalCost);
    }
 
    static void Main()
    {
        var cities = ReadFile("input.txt");
        Console.WriteLine("------Dane wejsciowe------");
        foreach (var r in cities)
        {
            wsp(r);
            Console.WriteLine(r);
        }


        Console.WriteLine("------Macierz sasiedztwa------");
        var matrix = NeighbourMatrix(cities);
        for (var i = 0; i < cities.Count; i++)
        {
            for (var j = 0; j < cities.Count; j++)
            {
                Console.Write($"|{matrix[i, j]}| ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-------------------------------- NAJBLIZSZY SASIAD --------------------------------");

        var paths = new List<(string path, double cost)>();
        for (var i = 0; i < cities.Count; i++)
        {
            var path = NearestNeighbour(cities, i, matrix);
            paths.Add(path);
            Console.WriteLine(path.path);
            Console.WriteLine("Koszt sciezki: " + path.cost);
        }
        Console.WriteLine("\nNajkrotsza sciezka: ");
        var shortestPath = paths.MinBy(x => x.cost);
        Console.WriteLine(shortestPath.path);
        Console.WriteLine(shortestPath.cost);

    }

}

