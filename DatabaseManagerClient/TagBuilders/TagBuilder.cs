using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerClient
{
    public class TagBuilder : ITagBuilder
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }
        public double Value { get; set; }

        public virtual void Build()
        {
            Console.Write("Tag name: ");
            Name = Console.ReadLine();

            Console.Write("Description: ");
            Description = Console.ReadLine();

            Console.Write("I/O Address >> ");
            IOAddress = Console.ReadLine();
        }
    }
}
