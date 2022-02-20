using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    class Program
    {
        static void Main(string[] args)
        {
            TagProcessing.GetInstance();

            ServiceHost svc = new ServiceHost(typeof(DatabaseManagerService));
            svc.Open();
            ServiceHost svcTrending = new ServiceHost(typeof(TrendingService));
            svcTrending.Open();
            ServiceHost svcAlarm = new ServiceHost(typeof(AlarmDisplayService));
            svcAlarm.Open();

            Console.WriteLine("Service is ready...");
            Console.ReadKey();

            svc.Close();
            svcTrending.Close();
            svcAlarm.Close();
        }
    }
}
