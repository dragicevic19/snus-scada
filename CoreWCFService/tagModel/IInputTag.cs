using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public interface IInputTag : ITag
    {
        string Driver { get; set; }
        double ScanTime { get; set; }
        bool OnOffScan { get; set; }
    }
}
