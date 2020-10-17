using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.Model
{
    public class ColorHistory
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Brightness { get; set; }
        public DateTime Date { get; set; }
    }
}
