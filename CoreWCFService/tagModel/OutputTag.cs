using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreWCFService.TagModel
{
    public abstract class OutputTag : Tag
    {
        public double InitValue { get; set; }

        public OutputTag(string name, string description, string iOAddress, double initValue) : base(name, description, iOAddress)
        {
            InitValue = initValue;
        }

        public OutputTag()
        {
        }

        public abstract override void WriteToXml(ref XDocument doc);

    }
}
