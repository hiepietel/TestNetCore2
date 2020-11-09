using ClassLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.DTO
{
    public class DeviceInfoDTO
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DeviceFunction Function { get; set; }
        public bool IsAlive { get; set; }
    }
}
