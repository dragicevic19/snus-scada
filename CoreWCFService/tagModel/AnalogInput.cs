using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class AnalogInput : InputTag
    {
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Units { get; set; }

        public AnalogInput(string name, string description, string iOAddress, string driver,
            double scanTime, bool scanOnOff, double lowLimit, double highLimit, string units) :
            base(name, description, iOAddress, driver, scanTime, scanOnOff)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
        }

        public AnalogInput()
        {
        }

        public override void WriteToXml(XDocument doc)
        {
            XElement tag = doc.Element("tag");
            tag.Add(new XElement("type", "AnalogInput"),
                    new XElement("name", Name),
                    new XElement("description", Description),
                    new XElement("driver", Driver),
                    new XElement("ioAddress", IOAddress),
                    new XElement("scanTime", ScanTime),         // TODO: alarmi
                    new XElement("scanOnOff", ScanOnOff),
                    new XElement("lowLimit", LowLimit),
                    new XElement("highLimit", HighLimit),
                    new XElement("units", Units));
        }

    }
}
