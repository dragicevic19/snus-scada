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
        public void OnAlarmOccurred(Alarm a, DateTime timeStamp)
        {
            for (int i = 0; i < a.Priority; i++)
            {
                Console.WriteLine("Alarm occurred --> TAG NAME: " + a.TagName + ", type: " + a.Type + ", TIME: " + timeStamp);
            }
            Console.WriteLine("\n");
        }
    }
}
