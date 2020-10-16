using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.Model
{
    public class TemperatureHistory
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public double Temp { get; set; }
        public DateTime Date { get; set; }

    }
}
