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

        public abstract void WriteToXml(XDocument doc);
    }
}
