using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CoreWCFService
{
    public enum AlarmType
    {
        LOW,
        HIGH
    }

    public class Alarm
    {
        public AlarmType Type { get; set; }
        public int Priority { get; set; }
        public double Limit { get; set; }
        public string TagName { get; set; }

        public Alarm(AlarmType type, int priority, double limit, string tagName)
        {
            Type = type;
            Priority = priority;
            Limit = limit;
            TagName = tagName;
        }

        public Alarm() { }

        internal void WriteToXml(ref XElement element)
        {
            element.Add(
                new XElement("alarm",
                new XAttribute("type", Type),
                new XAttribute("priority", Priority),
                new XAttribute("limit", Limit),
                new XAttribute("tagName", TagName)));
        }

        public static Alarm MakeAlarmFromConfigFile(XElement t)
        {
            AlarmType type = (AlarmType)Enum.Parse(typeof(AlarmType), (string)t.Attribute("type"));
            int priority = (int)t.Attribute("priority");
            double limit = (double)t.Attribute("limit");
            string tagName = (string)t.Attribute("tagName");

            return new Alarm(type, priority, limit, tagName);
        }
    }
}