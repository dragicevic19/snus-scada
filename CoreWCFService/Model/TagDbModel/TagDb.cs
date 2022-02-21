using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService.TagDbModel
{
    public class TagDb
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public string TagName { get; set; }

        public string Type { get; set; }

        public double Value { get; set; }

        public DateTime TimeStamp { get; set; }

        public TagDb() { }

        public TagDb(string tagName, string type, double value, DateTime timeStamp)
        {
            TagName = tagName;
            Type = type;
            Value = value;
            TimeStamp = timeStamp;
        }
    }
}
