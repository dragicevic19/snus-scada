using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public class AnalogInput : IAnalogTag, IInputTag
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }
        public string Driver { get; set; }
        public double ScanTime { get; set; }
        public bool OnOffScan { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Units { get; set; }
    }
}
