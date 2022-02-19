using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagerClient.TagBuilders
{
    public class AlarmBuilder
    {
        public string Type { get; set; }
        public int Priority { get; set; }
        public double Limit { get; set; }
        public string TagName { get; set; }
        public bool Error { get; set; }

        public AlarmBuilder() { }

        internal void Build(string tagName)
        {
            try
            {
                Error = false;
                TagName = tagName;
                Console.Clear();
                ChooseType();
                ChoosePriority();
                Console.Write("Limit >> ");
                Limit = double.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Error = true;
                Console.WriteLine("Invalid input");
            }
        }

        private void ChoosePriority()
        {
            while (true)
            {
                Console.Write("Priority (1, 2 or 3) >> ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Priority = 1;
                        return;
                    case "2":
                        Priority = 2;
                        return;
                    case "3":
                        Priority = 3;
                        return;
                    default:
                        Console.WriteLine("Invalid input. Try again");
                        break;
                }
            }
        }

        private void ChooseType()
        {
            Console.WriteLine("Alarm type:\n\t1 - LOW\n\t2 - HIGH");
            while (true)
            {
                Console.Write(">> ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Type = "LOW";
                        return;
                    case "2":
                        Type = "HIGH";
                        return;
                    default:
                        Console.WriteLine("Invalid input. Try again");
                        break;
                }
            }
        }
    }
}
