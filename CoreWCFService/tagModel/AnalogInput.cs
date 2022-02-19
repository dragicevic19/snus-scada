using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class AnalogInput : InputTag
    {
        const string TYPE = "AnalogInput";
        public List<Alarm> Alarms { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Units { get; set; }

        public AnalogInput(string name, string description, string iOAddress, string driver,
            int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units) :
            base(name, description, iOAddress, driver, scanTime, scanOnOff)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
        }

        public AnalogInput()
        {
        }

        public override void WriteToXml(ref XDocument doc)
        {
            XElement tag = doc.Element("root");
            tag.Add(new XElement("tag", 
                    new XAttribute("type" , TYPE),
                    new XAttribute("name", Name),
                    new XAttribute("description", Description),
                    new XAttribute("driver", Driver),
                    new XAttribute("ioAddress", IOAddress),
                    new XAttribute("scanTime", ScanTime),         // TODO: alarmi
                    new XAttribute("scanOnOff", ScanOnOff),
                    new XAttribute("lowLimit", LowLimit),
                    new XAttribute("highLimit", HighLimit),
                    new XAttribute("units", Units)));
        }

        public static Tag MakeTagFromConfigFile(XElement t)
        {
            string name = (string)t.Attribute("name");
            string desc = (string)t.Attribute("description");
            string driver = (string)t.Attribute("driver");
            string ioAddress = (string)t.Attribute("ioAddress");
            int scanTime = (int)t.Attribute("scanTime");
            bool scanOnOff = (bool)t.Attribute("scanOnOff");
            double lowLimit = (double)t.Attribute("lowLimit");
            double highLimit = (double)t.Attribute("highLimit");
            string units = (string)t.Attribute("units");

            return new AnalogInput(name, desc, ioAddress, driver, scanTime, scanOnOff, lowLimit, highLimit, units);
        }

        public override void Start(TagProcessing.AlarmHandler alarmOccured, TagProcessing.ValueHandler valueChanged)
        {
            while (true)
            {
                if (ScanOnOff)
                {
                    double driverValue;
                    if (Driver == "Simulation Driver")
                        driverValue = DriversLibrary.SimulationDriver.ReturnValue(IOAddress);

                    else if (Driver == "RealTime Driver")
                        driverValue = DriversLibrary.RealTimeDriver.ReturnValue(IOAddress);

                    else
                        throw new Exception();

                    if (driverValue > HighLimit) Value = HighLimit;
                    if (driverValue < LowLimit)  Value = LowLimit;
                    else                         Value = driverValue;

                    valueChanged?.Invoke(this);

                    CheckAlarms(Value, alarmOccured);

                    Thread.Sleep(1000 * ScanTime);
                }
            }
        }

        private void CheckAlarms(double value, TagProcessing.AlarmHandler alarmOccured)
        {
            foreach(var alarm in Alarms)
            {
                if ((alarm.Type == AlarmType.LOW && value <= alarm.Limit) || (alarm.Type == AlarmType.HIGH && value >= alarm.Limit))
                {
                    alarmOccured?.Invoke(alarm);
                }
            }
        }
    }
}
