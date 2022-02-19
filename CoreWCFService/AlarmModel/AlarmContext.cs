using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService.AlarmModel
{
    public class AlarmContext : DbContext
    {
        public DbSet<AlarmDTO> Alarms { get; set; }
    }
}
