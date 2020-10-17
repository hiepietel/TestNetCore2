using ClassLibrary.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        public DbSet<Device> Device { get; set; }
        public DbSet<TemperatureHistory> TemperatureHistory { get; set; }
        public DbSet<ColorHistory> ColorHistory { get; set; }
        /*
  Insert into Device values ('192.168.0.102', 'TEMP', 'Temperature Device', 1)
  Insert into Device values ('192.168.0.189', 'RGB', 'RGB Color Device', 0)
  Insert into Device values ('192.168.0.102', 'LCD', 'LCD Device', 2)
         */
    }
}
