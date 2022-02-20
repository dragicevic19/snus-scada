using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    [ServiceBehavior]
    public class AlarmDisplayService : IAlarmDisplayService
    {
        public void Init()
        {
            try
            {
                IAlarmDisplayCallback proxy = OperationContext.Current.GetCallbackChannel<IAlarmDisplayCallback>();
                TagProcessing.GetInstance().AddProxyForAlarmDisplay(proxy);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
