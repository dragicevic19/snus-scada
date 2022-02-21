using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TrendingClient.ServiceReference;

namespace TrendingClient
{
    public class TrendingCallback : ITrendingServiceCallback
    {
        public void OnValueChanged(TagDb tag)
        {
            Console.WriteLine($"Value changed --> TAG NAME: {tag.TagName}, type: {tag.Type}, value: {tag.Value}, time: {tag.TimeStamp}");
        }
    }
}
