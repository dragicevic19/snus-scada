using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    public interface ITag
    {
        string Name { get; set; }
        string Description { get; set; }
        string IOAddress { get; set; }

    }
}
