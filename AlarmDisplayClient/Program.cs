using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlarmDisplayClient.ServiceReference;

namespace AlarmDisplayClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AlarmDisplayServiceClient proxy = new AlarmDisplayServiceClient(new InstanceContext(new AlarmDisplayCallback()));

            proxy.Init();
            Console.WriteLine("Alarm Display App is active...\nWait for value change...\n\n");
            Console.ReadKey();
        }
    }
}
