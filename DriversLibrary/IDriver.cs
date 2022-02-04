using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivers
{
    public interface IDriver
    {
        double ReturnValue(string address);
    }
}
