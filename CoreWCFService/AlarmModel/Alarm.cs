using System.ComponentModel.DataAnnotations;

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
    }
}