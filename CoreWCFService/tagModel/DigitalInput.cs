using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class DigitalInput : InputTag
    {
        const string TYPE = "DigitalInput";
        public DigitalInput()
        {
        }

        public DigitalInput(string name, string description, string iOAddress, string driver,
            double scanTime, bool scanOnOff) : base(name, description, iOAddress, driver, scanTime, scanOnOff)
        {
        }

        public override void WriteToXml(ref XDocument doc)
        {
            XElement tag = doc.Element("root");
            tag.Add(new XElement("tag",
                    new XAttribute("type", TYPE),
                    new XAttribute("name", Name),
                    new XAttribute("description", Description),
                    new XAttribute("driver", Driver),
                    new XAttribute("ioAddress", IOAddress),
                    new XAttribute("scanTime", ScanTime),
                    new XAttribute("scanOnOff", ScanOnOff)));
        }

        internal static Tag MakeTagFromConfigFile(XElement t)
        {
            string name = (string)t.Attribute("name");
            string desc = (string)t.Attribute("description");
            string driver = (string)t.Attribute("driver");
            string ioAddress = (string)t.Attribute("ioAddress");
            double scanTime = (double)t.Attribute("scanTime");
            bool scanOnOff = (bool)t.Attribute("scanOnOff");

            return new DigitalInput(name, desc, ioAddress, driver, scanTime, scanOnOff);
        }
    }
}
