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

        const string TYPE = "AnalogOutput";

        public AnalogOutput() { }

        public AnalogOutput(string name, string description, string iOAddress, double initValue, double lowLimit, double highLimit,
            string units) : base(name, description, iOAddress, initValue)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
        }

        public override void WriteToXml(ref XDocument doc)
        {
            XElement tag = doc.Element("root");
            tag.Add(new XElement("tag",
                    new XAttribute("type", TYPE),
                    new XAttribute("name", Name),
                    new XAttribute("description", Description),
                    new XAttribute("ioAddress", IOAddress),
                    new XAttribute("initValue", InitValue),
                    new XAttribute("lowLimit", LowLimit),
                    new XAttribute("highLimit", HighLimit),
                    new XAttribute("units", Units)));
        }

        internal static Tag MakeTagFromConfigFile(XElement t)
        {
            string name = (string) t.Attribute("name");
            string desc = (string)t.Attribute("description");
            string ioAddress = (string)t.Attribute("ioAddress");
            double initValue = (double)t.Attribute("initValue");
            double lowLimit = (double)t.Attribute("lowLimit");
            double highLimit = (double)t.Attribute("highLimit");
            string units = (string)t.Attribute("units");

            return new AnalogOutput(name, desc, ioAddress, initValue, lowLimit, highLimit, units);
        }
    }
}
