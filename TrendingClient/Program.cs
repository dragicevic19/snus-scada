using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TrendingClient.ServiceReference;

namespace TrendingClient
{
    class Program
    {
        
        static void Main(string[] args)
        {
            TrendingServiceClient proxy = new TrendingServiceClient(new InstanceContext(new TrendingCallback()));

            proxy.Init();
            Console.WriteLine("Trending App is active...\nWait for value change...\n\n");
            Console.ReadKey();
        }
    }
}
