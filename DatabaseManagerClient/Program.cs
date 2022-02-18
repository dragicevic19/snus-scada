using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManagerClient.ServiceReference;

namespace DatabaseManagerClient
{
    public class Program
    {

        static void Main(string[] args)
        {
            DatabaseManagerServiceClient proxy = new DatabaseManagerServiceClient();
            AuthenticationClient authProxy = new AuthenticationClient();
            TagsView tagView = new TagsView(proxy, authProxy);

            RegistrationAndLoginView.GetInstance(proxy, authProxy, tagView).Start();
            

        }
    }
}
