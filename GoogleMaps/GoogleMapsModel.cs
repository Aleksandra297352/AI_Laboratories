using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMaps
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Distance
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("value")]
        public int? Value;
    }

    public class Duration
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("value")]
        public int? Value;
    }

    public class Element
    {
        [JsonProperty("distance")]
        public Distance Distance;

        [JsonProperty("duration")]
        public Duration Duration;

        [JsonProperty("status")]
        public string Status;
    }

    public class DistanceMatrixResponse
    {
        [JsonProperty("destination_addresses")]
        public List<string> DestinationAddresses;

        [JsonProperty("origin_addresses")]
        public List<string> OriginAddresses;

        [JsonProperty("rows")]
        public List<Row> Rows;

        [JsonProperty("status")]
        public string Status;
    }

    public class Row
    {
        [JsonProperty("elements")]
        public List<Element> Elements;
    }


}
