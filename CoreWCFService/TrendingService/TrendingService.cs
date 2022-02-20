using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public class TrendingService : ITrendingService
    {
        public void Init()
        {
            try
            {
                ITrendingCallback proxy = OperationContext.Current.GetCallbackChannel<ITrendingCallback>();
                TagProcessing.GetInstance().AddProxyForTrending(proxy);
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
