using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportManagerClient.ServiceReference;

namespace ReportManagerClient
{
    class Program
    {
        static ReportManagerServiceClient proxy = new ReportManagerServiceClient();

        static void Main(string[] args)
        {

            while (true)
            {
                Console.Clear();
                Console.Write("\n1 - Alarms that occurred over period of time\n2 - Alarms of a certain priority\n3 - Values of tags over period of time\n4 - Last value of all Analog Input tags" +
                    "\n5 - Last value of all Digital Input tags\n6 - All values of a certain tag\n\nX - Exit\n >> ");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "1":
                        AlarmsOverPeriodOfTime();
                        break;
                    case "2":
                        AlarmsOfCertainPriority();
                        break;
                    case "3":
                        ValuesOfTagsOverPeriodOfTime();
                        break;
                    case "4":
                        LastValueOfAllAITags();
                        break;
                    case "5":
                        LastValueOfAllDITags();
                        break;
                    case "6":
                        AllValuesOfCertainTag();
                        break;
                    case "x":
                        return;
                    default:
                        break;

                }
            }
        }

        private static void PrintAlarms(AlarmDTO[] alarms)
        {
            foreach (AlarmDTO alarm in alarms)
            {
                Console.WriteLine($"Alarm for tag: {alarm.TagName}, type: {alarm.Type}, priority: {alarm.Priority}, timestamp: {alarm.TimeStamp}");
            }
        }

        private static void PrintTags(TagDb[] tags)
        {
            foreach (TagDb tag in tags)
            {
                Console.WriteLine($"Tag name: {tag.TagName}, Type: {tag.Type} ,Value: {tag.Value}, timestamp: {tag.TimeStamp}");
            }
        }

        private static void AlarmsOverPeriodOfTime()
        {
            Console.Clear();
            try
            {
                Console.Write("Start DateTime (DD/MM/yyyy HH.mm) >> ");
                DateTime start = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH.mm", CultureInfo.CurrentCulture);
                Console.Write("End DateTime (dd/MM/yyyy HH.mm) >> ");
                DateTime end = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH.mm", CultureInfo.CurrentCulture);

                AlarmDTO[] alarms = proxy.AlarmsOverAPeriodOfTime(start, end);
                PrintAlarms(alarms);

                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            } catch (Exception)
            {
                Console.Write("Invalid input\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void AlarmsOfCertainPriority()
        {
            Console.Clear();
            try
            {
                Console.Write("Enter the priority (1, 2 or 3) >> ");
                int priority = int.Parse(Console.ReadLine());

                if (priority < 1 || priority > 3)
                {
                    Console.WriteLine("Invalid input");
                    return;
                }

                AlarmDTO[] alarms = proxy.AlarmsWithPriority(priority);
                PrintAlarms(alarms);

                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception)
            {
                Console.Write("Invalid input\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void ValuesOfTagsOverPeriodOfTime()
        {
            Console.Clear();
            try
            {
                Console.Write("Start DateTime (DD/MM/yyyy HH.mm) >> ");
                DateTime start = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH.mm", CultureInfo.CurrentCulture);
                Console.Write("End DateTime (dd/MM/yyyy HH.mm) >> ");
                DateTime end = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH.mm", CultureInfo.CurrentCulture);

                TagDb[] tags = proxy.TagsOverAPeriodOfTime(start, end);
                PrintTags(tags);

                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception)
            {
                Console.Write("Invalid input\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void LastValueOfAllAITags()
        {
            try
            {
                TagDb[] tags = proxy.LastValueOfAnalogInputTags();
                PrintTags(tags);
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void LastValueOfAllDITags()
        {
            try
            {
                TagDb[] tags = proxy.LastValueOfDigitalInputTags();
                PrintTags(tags);
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void AllValuesOfCertainTag()
        {
            try
            {
                Console.Write("Enter tag name >> ");
                string tagName = Console.ReadLine();
                TagDb[] tags = proxy.AllValuesOfTagWithTagName(tagName);
                PrintTags(tags);
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception) {
                Console.WriteLine("Error"); 
                Console.Write("\nPress any key to continue..."); 
                Console.ReadKey(); 
            }
        }
    }
}
