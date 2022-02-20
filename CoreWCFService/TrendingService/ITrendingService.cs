using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CoreWCFService.TagDbModel;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    [ServiceContract(CallbackContract = typeof(ITrendingCallback))]
    public interface ITrendingService
    {
        [OperationContract(IsOneWay = true)]
        void Init();
    }

    public interface ITrendingCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnValueChanged(TagDb tag);
    }
}
