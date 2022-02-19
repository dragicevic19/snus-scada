using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerClient
{
    public class DigitalOutputBuilder : OutputTagBuilder, ITagBuilder
    {
        public bool Error { get; set; }
        public override void Build()
        {
            try
            {
                Error = false;
                base.Build();
            }
            catch (Exception)
            {
                Error = true;
                Console.WriteLine("Invalid input!");

            }
        }
    }
}
