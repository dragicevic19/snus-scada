using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CoreWCFService.AlarmModel;
using CoreWCFService.Reports;
using CoreWCFService.RTU;
using CoreWCFService.TagModel;

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
            ServiceHost svcRTU = new ServiceHost(typeof(RealTimeUnitService));
            svcRTU.Open();
            ServiceHost svcReport = new ServiceHost(typeof(ReportManagerService));
            svcReport.Open();

            Console.WriteLine("Service is ready...");
            Console.ReadKey();

            svc.Close();
            svcTrending.Close();
            svcAlarm.Close();
            svcRTU.Close();
            svcReport.Close();
        }
    }
}
