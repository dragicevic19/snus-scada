using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService.RTU
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IRealTimeUnitService
    {
        [OperationContract(IsInitiating = true)]
        int PubInit();

        [OperationContract(IsInitiating = false)]
        void ExportPublicKey(int pubId, string keyPath);

        [OperationContract(IsInitiating = false)]
        void SendValue(int pubId, string message, byte[] signature);
    }
}
