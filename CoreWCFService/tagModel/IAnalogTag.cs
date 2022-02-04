using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public interface IAnalogTag : ITag
    {
        double LowLimit { get; set; }
        double HighLimit { get; set; }
        string Units { get; set; }

    }
}
