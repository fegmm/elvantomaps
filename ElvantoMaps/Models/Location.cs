using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElvantoMaps.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string FormattedAddress { get; set; }
    }
}
