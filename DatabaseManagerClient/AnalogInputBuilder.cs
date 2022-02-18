using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerClient
{
    public class AnalogInputBuilder : InputTagBuilder, ITagBuilder
    {
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Units { get; set; }
        public bool Error { get; set; }

        // alarme sam ostavio za posle

        public override void Build()
        {
            try
            {
                Error = false;
                base.Build();
                Console.Write("Low limit >> ");
                LowLimit = double.Parse(Console.ReadLine());

                Console.Write("High limit >> ");
                HighLimit = double.Parse(Console.ReadLine());

                Console.Write("Units >> ");
                Units = Console.ReadLine();
            }
            catch (Exception)
            {
                Error = true;
                Console.WriteLine("Invalid input!");
            }
        }
    }
}
