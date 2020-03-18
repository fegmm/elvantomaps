using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElvantoMaps.Models
{
    public class GeocodingResponse
    {
        public Result[] results { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public Address_Components[] address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }
    }

    public class Geometry
    {
        public Coordinate location { get; set; }
        public string location_type { get; set; }
        public Dictionary<string, Location> viewport { get; set; }
    }

    public class Coordinate
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Address_Components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
}
