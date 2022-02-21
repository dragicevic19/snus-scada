using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    [ServiceContract(CallbackContract = typeof(IAlarmDisplayCallback), SessionMode = SessionMode.Required)]
    public interface IAlarmDisplayService
    {
        [OperationContract(IsOneWay = true)]
        void Init();
    }

    public interface IAlarmDisplayCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnAlarmOccurred(Alarm a, DateTime timeStamp);
    }
}
