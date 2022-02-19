using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CoreWCFService.TagDbModel;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    [ServiceBehavior]
    public class DatabaseManagerService : IDatabaseManagerService, IAuthentication
    {
        TagProcessing TagProcessing = new TagProcessing();
        // UserProcessing UserProcessing = new UserProcessing();
        public static void LoadScadaConfig()
        {
            TagProcessing.LoadScadaConfig();
        }


        public bool Registration(string username, string password)
        {
            return UserProcessing.Registration(username, password);
        }

        public string Login(string username, string password)
        {
            return UserProcessing.Login(username, password);
        }

        public bool Logout(string token)
        {
            return UserProcessing.Logout(token);
        }

        public bool AddDigitalInputTag(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.AddDigitalInputTag(name, description, driver, ioAddress, scanTime, scanOnOff);
        }

        public bool AddDigitalOutputTag(string token, string name, string description, string ioAddress, double initValue)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.AddDigitalOutputTag(name, description, ioAddress, initValue);
        }

        public bool AddAnalogInputTag(string token, string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.AddAnalogInputTag(name, description, driver, ioAddress, scanTime, scanOnOff, lowLimit, highLimit, units);
        }

        public bool AddAnalogOutputTag(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.AddAnalogOutputTag(name, description, ioAddress, initValue, lowLimit, highLimit, units);
        }
        
        public bool RemoveTag(string token, string tagName)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.RemoveTag(tagName);
        }

        public bool ChangeOutputValue(string token, string tagName, double value)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.ChangeOutputValue(tagName, value);
        }

        public double GetOutputValue(string token, string tagName)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return -10000;

            return TagProcessing.GetOutputValue(tagName);
        }

        public bool TurnScanOff(string token, string tagName)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.TurnScanOff(tagName);
        }

        public bool TurnScanOn(string token, string tagName)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return false;

            return TagProcessing.TurnScanOn(tagName);
        }

        public string GetStringForPrintingTags(string token, string type="", bool value = false, bool scan = false)
        {
            if (!UserProcessing.IsUserAuthenticated(token)) return "Wrong authentication token";

            return TagProcessing.PrintTags(type, value, scan);
        }
    }
}
