using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerClient
{
    public class OutputTagBuilder : TagBuilder, ITagBuilder
    {
        public double InitValue { get; set; }

        public override void Build()
        {
            base.Build();
            Console.Write("Initial value: ");
            InitValue = double.Parse(Console.ReadLine());
        }
    }
}
