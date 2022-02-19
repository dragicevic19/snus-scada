using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreWCFService.TagModel
{
    public abstract class InputTag : Tag
    {
        public string Driver { get; set; }
        public int ScanTime { get; set; }
        public bool ScanOnOff { get; set; }

        public InputTag()
        {
        }

        public InputTag(string name, string description, string iOAddress, string driver,
            int scanTime, bool scanOnOff) : base (name, description, iOAddress)
        {
            Driver = driver;
            ScanTime = scanTime;
            ScanOnOff = scanOnOff;
        }

        public abstract override void WriteToXml(ref XDocument doc);

        public abstract void Start(TagProcessing.AlarmHandler alarmOccured, TagProcessing.ValueHandler valueChanged);

    }
}
