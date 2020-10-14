using ClassLibrary.Enum;
using System;

namespace ClassLibrary.Model
{
    public class Device
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DeviceFunction Function { get; set; }
    }
}
