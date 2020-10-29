using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.Model
{
    public class SingleCountry
    {
        public string country { get; set; }
        public string code { get; set; }
        public int confirmed { get; set; }
        public int recovered { get; set; }
        public int critical { get; set; }
        public int deaths { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime lastChange { get; set; }
        public DateTime lastUpdate { get; set; }
    }
}
