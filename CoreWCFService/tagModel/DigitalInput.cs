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
        public DigitalInput()
        {
        }

        public DigitalInput(string name, string description, string iOAddress, string driver,
            double scanTime, bool scanOnOff) : base(name, description, iOAddress, driver, scanTime, scanOnOff)
        {
        }

        public override void WriteToXml(XDocument doc)
        {

        }
    }
}
