using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmDisplayClient.ServiceReference;

namespace AlarmDisplayClient
{
    public class AlarmDisplayCallback : IAlarmDisplayServiceCallback
    {
        public void OnAlarmOccured(Alarm a, DateTime timeStamp)
        {
            for (int i = 0; i < a.Priority; i++)
            {
                Console.WriteLine("Alarm occured --> TAG NAME: " + a.TagName + ", type: " + a.Type + ", TIME: " + timeStamp);
            }
            Console.WriteLine("\n");
        }
    }
}
