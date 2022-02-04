using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    [ServiceContract]
    public interface IDatabaseManagerService
    {
        [OperationContract]
        void RegisterUser();

        [OperationContract]
        void Login();

        [OperationContract]
        void Logout();

        [OperationContract]
        void AddTag();

        [OperationContract]
        void RemoveTag();

        [OperationContract]
        void ChangeOutputValue();

        [OperationContract]
        double GetOutputValue();

        [OperationContract]
        bool TurnScanOn();

        [OperationContract]
        bool TurnScanOff();



    }
}
