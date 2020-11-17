using ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.Model
{
    public class DeviceHistory
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public bool IsAlive { get; set; }
        public DateTime Date { get; set; }
    }
}
