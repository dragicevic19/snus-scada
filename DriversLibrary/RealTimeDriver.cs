using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriversLibrary
{
    public static class RealTimeDriver
    {
        private static Dictionary<string, double> RtuData = new Dictionary<string, double>();

        public static double ReturnValue(string address)
        {
            try
            {
                return RtuData[address];
            }
            catch (Exception)
            {
                return -1000;
            }
        }

        public static void SetRTUValue(string address, double value)
        {
            RtuData.Add(address, value);
        }
    }
}
