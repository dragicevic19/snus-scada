using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    [ServiceBehavior]
    public class DatabaseManagerService : IDatabaseManagerService
    {
        public void RegisterUser()
        {
            throw new NotImplementedException();
        }

        public void Login()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void AddTag()
        {
            throw new NotImplementedException();
        }

        public void RemoveTag()
        {
            throw new NotImplementedException();
        }

        public void ChangeOutputValue()
        {
            throw new NotImplementedException();
        }

        public double GetOutputValue()
        {
            throw new NotImplementedException();
        }

        public bool TurnScanOff()
        {
            throw new NotImplementedException();
        }

        public bool TurnScanOn()
        {
            throw new NotImplementedException();
        }
    }
}
