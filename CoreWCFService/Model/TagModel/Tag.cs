using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreWCFService.TagModel
{
    public abstract class Tag : ITag
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }
        public double Value { get; set; }

        public Tag(string name, string description, string iOAddress)
        {
            Name = name;
            Description = description;
            IOAddress = iOAddress;
            Value = 0; // ??
        }

        public Tag()
        {
        }

        public abstract void WriteToXml(ref XDocument doc);

        public static Tag MakeTagFromConfigFile(XElement t)
        {
            string type = (string)t.Attribute("type");
            switch (type)
            {
                case "AnalogInput":
                    return AnalogInput.MakeTagFromConfigFile(t);
                case "AnalogOutput":
                    return AnalogOutput.MakeTagFromConfigFile(t);
                case "DigitalInput":
                    return DigitalInput.MakeTagFromConfigFile(t);
                case "DigitalOutput":
                    return DigitalOutput.MakeTagFromConfigFile(t);
                default:
                    return null;
            }
        }
    }
}
