using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class AnalogOutput : OutputTag
    {
        public double LowLimit {get; set;}
        public double HighLimit {get; set;}
        public string Units {get; set;}

        public AnalogOutput() { }

        public AnalogOutput(string name, string description, string iOAddress, double initValue, double lowLimit, double highLimit,
            string units) : base(name, description, iOAddress, initValue)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
        }

        public override void WriteToXml(XDocument doc)
        {

        }
    }
}
