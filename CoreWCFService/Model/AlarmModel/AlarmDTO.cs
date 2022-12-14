using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService.AlarmModel
{
    public class AlarmDTO
    {
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }
        public AlarmType Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Priority { get; set; }

        public AlarmDTO(string tagName, AlarmType type, DateTime timeStamp, int priority)
        {
            TagName = tagName;
            Type = type;
            TimeStamp = timeStamp;
            Priority = priority;
        }

        public AlarmDTO() { }
    }
}
