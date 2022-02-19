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
        bool AddDigitalInputTag(string token, string name, string description, string driver, string ioAddress, double scanTime, bool scanOnOff);

        [OperationContract]
        bool AddDigitalOutputTag(string token, string name, string description, string ioAddress, double initValue);

        [OperationContract] // ovde nisam stavio alarme
        bool AddAnalogInputTag(string token, string name, string description, string driver, string ioAddress, double scanTime, bool scanOnOff, double lowLimit, double highLimit, string units);

        [OperationContract]
        bool AddAnalogOutputTag(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units);

        [OperationContract]
        bool RemoveTag(string token, string tagName);

        [OperationContract]
        bool ChangeOutputValue(string token, string tagName, double value);

        [OperationContract]
        double GetOutputValue(string token, string tagName);

        [OperationContract]
        bool TurnScanOn(string token, string tagName);

        [OperationContract]
        bool TurnScanOff(string token, string tagName);

        [OperationContract]
        string GetStringForPrintingTags(string token, string type="", bool value=false, bool scan=false);

    }
}
