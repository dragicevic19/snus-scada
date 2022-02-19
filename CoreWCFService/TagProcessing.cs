﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.TagDbModel;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class TagProcessing
    {
        private static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();    // key is TagName
        const string CONFIG_FILE_PATH = @"../../data/scadaConfig.xml";

        public delegate void AlarmHandler(Alarm alarm);
        public delegate void ValueHandler(Tag tag);

        public event AlarmHandler AlarmOccured;
        public event ValueHandler ValueChanged;

        public TagProcessing()
        {
            AlarmOccured += OnAlarmOccured; // ovde ili u nekim metodama koje ce pozivati Trending i AlarmDisplay servisi?
            ValueChanged += OnValueChanged;
        }

        internal void OnAlarmOccured(Alarm alarm)
        {
            // callbackproxy.AlarmOccured(mozda str?)
        }

        internal void OnValueChanged(Tag tag)
        {

        }

        #region tags
        internal bool AddAnalogInputTag(string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units)
        {
            try
            {
                InputTag tag = new AnalogInput(name, description, ioAddress, driver, scanTime, scanOnOff, lowLimit, highLimit, units);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Thread t = new Thread(() => tag.Start(AlarmOccured, ValueChanged));
                    t.Start();
                    Console.WriteLine("New tag added: " + name);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal bool AddAnalogOutputTag(string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units)
        {
            try
            {
                Tag tag = new AnalogOutput(name, description, ioAddress, initValue, lowLimit, highLimit, units);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Console.WriteLine("New tag added: " + name);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal bool AddDigitalInputTag(string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff)
        {
            try
            {
                InputTag tag = new DigitalInput(name, description, ioAddress, driver, scanTime, scanOnOff);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Thread t = new Thread(() => tag.Start(AlarmOccured, ValueChanged));
                    t.Start();
                    Console.WriteLine("New tag added: " + name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal bool AddDigitalOutputTag(string name, string description, string ioAddress, double initValue)
        {
            try
            {
                Tag tag = new DigitalOutput(name, description, ioAddress, initValue);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Console.WriteLine("New tag added: " + name);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal bool RemoveTag(string tagName)
        {
            try
            {
                if (tags.Remove(tagName) && WriteXmlConfig())
                {
                    Console.WriteLine("Tag removed: " + tagName);
                    return true;
                }
                else return false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal bool TurnScanOn(string tagName)
        {
            try
            {
                ((InputTag)tags[tagName]).ScanOnOff = true;
                if (WriteXmlConfig())
                {
                    Console.WriteLine("Scan turned ON for tag: " + tagName);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal bool TurnScanOff(string tagName)
        {
            try
            {
                ((InputTag)tags[tagName]).ScanOnOff = false;
                if (WriteXmlConfig())
                {
                    Console.WriteLine("Scan turned OFF for tag: " + tagName);
                    return true;
                }
                else return false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal double GetOutputValue(string tagName)
        {
            try
            {
                Console.WriteLine("Output value returned: " + tags[tagName].Value);
                return tags[tagName].Value;
            }
            catch (Exception e)   // povratne vrednosti sredi
            {
                Console.WriteLine(e.Message);
                return -20000;
            }
        }

        internal bool ChangeOutputValue(string tagName, double value)
        {
            try
            {
                tags[tagName].Value = value;
                if (AddTagToDatabase(tags[tagName]))
                {
                    Console.WriteLine("Value changed on tag: " + tagName + ", to a new value: " + value);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region console_print_tags
        internal string PrintTags(string type, bool value, bool scan)
        {
            string retStr = "==================================================================================================================\n";

            retStr += "|      TAG NAME      |  INPUT/OUTPUT  | ANALOG/DIGITAL |               DESCRIPTION               |";
            if (value) retStr += "VALUE|";
            if (scan) retStr += "SCAN ON/OFF|";

            retStr += "\n------------------------------------------------------------------------------------------------------------------\n";

            foreach (KeyValuePair<string, Tag> tag in tags)
            {
                if ((type == "input" && tag.Value is InputTag) || (type == "output" && tag.Value is OutputTag) || type == "")
                {
                    string IOtype = (tag.Value is InputTag) ? "INPUT" : "OUTPUT";
                    string digAnaType = (tag.Value is DigitalInput || tag.Value is DigitalOutput) ? "DIGITAL" : "ANALOG";
                    retStr += String.Format("|{0,-20}|{1,-16}|{2,-16}|{3,-41}|", tag.Value.Name, IOtype, digAnaType, tag.Value.Description);

                    if (value) retStr += String.Format("{0,5}|", tag.Value.Value);

                    if (scan) retStr += String.Format("{0,-11}|", ((InputTag)tag.Value).ScanOnOff);

                    retStr += "\n";
                    retStr += "------------------------------------------------------------------------------------------------------------------\n";

                }
            }
            retStr += "==================================================================================================================";

            return retStr;
        }

        #endregion

        #region write_config
        private bool WriteXmlConfig()
        {
            try
            {
                //if (!File.Exists(CONFIG_FILE_PATH)) CreateNewXmlFile();
                XDocument doc = XDocument.Load(CONFIG_FILE_PATH);
                doc.Descendants("tag").Remove();                    // brisem sve pa ponovo sve upisujem zbog izmena?
                                                                    // ili je mozda bolje da trazim da li postoji pa onda menjam?
                foreach (KeyValuePair<string, Tag> tag in tags)
                {
                    tag.Value.WriteToXml(ref doc);
                }
                doc.Save(CONFIG_FILE_PATH);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        #region load_config
        internal static void LoadScadaConfig()
        {
            try
            {
                XElement configElements = XElement.Load(CONFIG_FILE_PATH);
                var tagConfig = configElements.Descendants("tag");
                foreach (var t in tagConfig)
                {
                    Tag tag = Tag.MakeTagFromConfigFile(t);
                    if (tag is InputTag) FindValueOfTag(ref tag);
                    tags.Add(tag.Name, tag);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void FindValueOfTag(ref Tag tag)
        {
            List<TagDb> allTags = new List<TagDb>();
            using (var db = new TagContext())
            {
                foreach (var t in db.Tags)
                {
                    if (t.TagName == tag.Name)
                    {
                        allTags.Add(t);
                    }
                }
            }
            if (allTags.Count == 0) return;

            TagDb last = allTags.Last(); // bar se nadam da u bazi pakuje po redosledu upisivanja
            tag.Value = last.Value;
        }

        #endregion

        #region tag_database
        private bool AddTagToDatabase(Tag tag)
        {
            using (var db = new TagContext())
            {
                try
                {
                    db.Tags.Add(new TagDb(tag.Name, tag.Value, DateTime.Now));
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }
        #endregion
    }
}
