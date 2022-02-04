using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public interface IOutputTag : ITag
    {
        double InitValue { get; set; }
    }
}
