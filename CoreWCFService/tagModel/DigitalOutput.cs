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
        public DigitalOutput(string name, string description, string iOAddress, double initValue) : base(name, description, iOAddress, initValue)
        {
        }

        public DigitalOutput()
        {
        }

        public override void WriteToXml(XDocument doc)
        {

        }
    }
}
