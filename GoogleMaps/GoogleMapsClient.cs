using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMaps
{
    public class GoogleMapsClient
    {
        const string _url = "https://maps.googleapis.com/maps/api";
        readonly string _apiKey;
        HttpClient _httpClient;
        public GoogleMapsClient(string apiKey) { 
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }
        public async Task<DistanceMatrixResponse> GetDistanceMatrixResponse(string city1, string city2)
        {
            var uri = $"{_url}/distancematrix/json?destinations={city1}&origins={city2}&units=metric&language=pl&mode=driving&region=pl&key={_apiKey}";
            var distanceResponse = await _httpClient.GetAsync(uri);
            var distanceMatrix = JsonConvert.DeserializeObject<DistanceMatrixResponse>(await distanceResponse.Content.ReadAsStringAsync());

            return distanceMatrix;
        }
    }
}
