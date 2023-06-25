using System;
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
class Program
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
    static void Main(string[] args)
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
        Console.WriteLine("-------------------------------- WYZARZANIE --------------------------------");

        var path = SalesmanAnnealing.SimulatedAnnealing(matrix);
        Console.WriteLine("\nNajkrotsza sciezka: ");
        var cost = 0d;
        var result = "";
        for(var i = 0; i<path.Length-1; i++)
        {
            result += cities[path[i]].Name + "-->";
            cost += matrix[path[i], path[i + 1]];
        }
        result += cities[path[path.Length - 1]].Name+"-->";

        //dodajemy punkt startowy
        result += cities[path[0]].Name;
        cost += matrix[path[0], path[path.Length - 1]];

        Console.WriteLine(result);
        Console.WriteLine("Koszt: " + cost);

    }
}
