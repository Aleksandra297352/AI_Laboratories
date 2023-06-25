namespace GoogleMaps
{
    internal class Program
    {
        static List<string> ReadCitiesFromFile(string path)
        {

            var textLinesFromFile = File.ReadAllLines(path);
            var result = new List<string>();

            foreach (var line in textLinesFromFile)
            {
                result.Add(line.Split(';').First());
            }

            return result;
        }
        static async Task<double[,]> CreateNeighboursMatrix(List<string> cities, GoogleMapsClient googleMapsClient)
        {
            var neighbourMatrix = new double[cities.Count, cities.Count];

            var citiesTasks = new List<Task<DistanceMatrixResponse>>();
            var visitedCities = new List<string>();
            foreach (var city in cities)
            {
                visitedCities.Add(city);
                foreach (var neighbour in cities.Where(x => visitedCities.Contains(x) == false))
                {
                    citiesTasks.Add(googleMapsClient.GetDistanceMatrixResponse(city, neighbour));
                }
            }

            var citiesFromGoogle = await Task.WhenAll(citiesTasks);

            foreach (var distanceMatrix in citiesFromGoogle)
            {
                try
                {
                    var firstCity = cities.First(distanceMatrix.OriginAddresses.First().Contains);
                    var secondCity = cities.First(distanceMatrix.DestinationAddresses.First().Contains);
                    var indexOfFirstCity = cities.IndexOf(firstCity);
                    var indexOfSecondCity = cities.IndexOf(secondCity);
                    neighbourMatrix[indexOfFirstCity, indexOfSecondCity] = distanceMatrix.GetDistanceInMeters() ?? -1;
                    neighbourMatrix[indexOfSecondCity, indexOfFirstCity] = distanceMatrix.GetDistanceInMeters() ?? -1;
                }
                catch (Exception e)
                {
                    ;
                }
            }


            return neighbourMatrix;
        }
        static void SaveNeighboursMatirxToFile(double[,] neighboursMatrix, string path)
        {
            var result = new List<string>();

            for(var i = 0; i < neighboursMatrix.GetLength(0); i++)
            {
                var row = string.Empty;
                for(var j = 0; j<neighboursMatrix.GetLength(1); j++)
                {
                    row += neighboursMatrix[i,j];
                    if(j != neighboursMatrix.GetLength(1) - 1)
                    {
                        row += ";";
                    }
                }
                result.Add(row);
            }

            File.WriteAllLines(path, result);
        }
        static async Task Main(string[] args)
        {
            const string apiKey = "AIzaSyBfJedOMvFT_7q24m6UfTQAgcyaMS-iIq4";
            var googleMapsClient = new GoogleMapsClient(apiKey);

            var cities = ReadCitiesFromFile("input.txt");
            var neighboursMatrix = await CreateNeighboursMatrix(cities, googleMapsClient);
            SaveNeighboursMatirxToFile(neighboursMatrix, "neighbourMatrix.txt");
            Console.WriteLine("saved");
            //var res = await googleMapsClient.GetDistanceMatrixResponse("Toruń", "Warszawa");
            //Console.WriteLine(res.GetDistanceInMeters());
        }
    }
}