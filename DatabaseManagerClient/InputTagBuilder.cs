using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerClient
{
    public class InputTagBuilder : TagBuilder, ITagBuilder
    {
        public string Driver { get; set; }
        public double ScanTime { get; set; }
        public bool ScanOnOff { get; set; }

        public override void Build()
        {
            base.Build();

            Driver = ChooseDriver();

            Console.Write("Scan time >> ");
            ScanTime = double.Parse(Console.ReadLine());

            Console.Write("Type ON or OFF to turn the scan on or off >> ");
            string scanOnOffInput = Console.ReadLine();
            ScanOnOff = (scanOnOffInput.ToUpper().Trim() == "ON");
        }

        private string ChooseDriver()
        {
            Console.WriteLine("Drivers:\n\t1 - Simulation Driver\n\t2 - Real-Time Driver");

            while (true)
            {
                Console.Write("Pick the driver >> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return "Simulation Driver";
                    case "2":
                        return "Real-Time Driver";
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }
        }
    }
}
