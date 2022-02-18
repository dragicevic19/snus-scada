using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class DigitalOutput : OutputTag
    {
        const string TYPE = "DigitalOutput";

        public DigitalOutput(string name, string description, string iOAddress, double initValue) : base(name, description, iOAddress, initValue)
        {
        }

        public DigitalOutput()
        {
        }

        public override void WriteToXml(ref XDocument doc)
        {
            XElement tag = doc.Element("root");
            tag.Add(new XElement("tag",
                    new XAttribute("type", TYPE),
                    new XAttribute("name", Name),
                    new XAttribute("description", Description),
                    new XAttribute("ioAddress", IOAddress),
                    new XAttribute("initValue", InitValue)));
        }

        internal static Tag MakeTagFromConfigFile(XElement t)
        {
            string name = (string)t.Attribute("name");
            string desc = (string)t.Attribute("description");
            string ioAddress = (string)t.Attribute("ioAddress");
            double initValue = (double)t.Attribute("initValue");

            return new DigitalOutput(name, desc, ioAddress, initValue);
        }
    }
}
