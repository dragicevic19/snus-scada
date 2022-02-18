using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService.TagDbModel
{
    public class TagContext : DbContext
    {
        public DbSet<TagDb> Tags { get; set; }
    }
}
