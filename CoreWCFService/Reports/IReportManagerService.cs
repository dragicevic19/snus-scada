using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CoreWCFService.AlarmModel;
using CoreWCFService.TagDbModel;

namespace CoreWCFService.Reports
{
    [ServiceContract]
    public interface IReportManagerService
    {
        [OperationContract(IsOneWay = false)]
        AlarmDTO[] AlarmsOverAPeriodOfTime(DateTime start, DateTime end);

        [OperationContract]
        AlarmDTO[] AlarmsWithPriority(int priority);

        [OperationContract]
        TagDb[] TagsOverAPeriodOfTime(DateTime start, DateTime end);

        [OperationContract]
        TagDb[] LastValueOfAnalogInputTags();

        [OperationContract]
        TagDb[] LastValueOfDigitalInputTags();

        [OperationContract]
        TagDb[] AllValuesOfTagWithTagName(string tagName);
    }
}
