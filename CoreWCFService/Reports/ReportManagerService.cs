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
    [ServiceBehavior]
    public class ReportManagerService : IReportManagerService
    {
        public AlarmDTO[] AlarmsOverAPeriodOfTime(DateTime start, DateTime end)
        {
            return TagProcessing.GetInstance().GetAlarmsOverAPeriodOfTime(start, end);
        }

        public AlarmDTO[] AlarmsWithPriority(int priority)
        {
            return TagProcessing.GetInstance().GetAlarmsWithCertainPriority(priority);
        }

        public TagDb[] AllValuesOfTagWithTagName(string tagName)
        {
            return TagProcessing.GetInstance().GetTagsWithCertainTagName(tagName);
        }

        public TagDb[] LastValueOfAnalogInputTags()
        {
            return TagProcessing.GetInstance().GetLastValueOfAITags();
        }

        public TagDb[] LastValueOfDigitalInputTags()
        {
            return TagProcessing.GetInstance().GetLastValueOfDITags();
        }

        public TagDb[] TagsOverAPeriodOfTime(DateTime start, DateTime end)
        {
            return TagProcessing.GetInstance().GetTagsOverAPeriodOfTime(start, end);
        }
    }
}
