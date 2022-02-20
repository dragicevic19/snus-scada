using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreWCFService.AlarmModel;
using CoreWCFService.TagDbModel;
using CoreWCFService.TagModel;

namespace CoreWCFService
{
    public class TagProcessing
    {
        const string CONFIG_FILE_PATH = @"../../data/scadaConfig.xml";
        const string ALARM_TXT_PATH = @"../../data/alarmLog.txt";

        private static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();    // key is TagName
        static readonly object tagsLocker = new object();
        private static List<Alarm> alarms = new List<Alarm>();
        static readonly object alarmsLocker= new object();


        public delegate void AlarmHandler(Alarm alarm);
        public delegate void ValueHandler(Tag tag);

        public event AlarmHandler AlarmOccured;
        public event ValueHandler ValueChanged;

        private static TagProcessing Instance = null;

        public static TagProcessing GetInstance()
        {
            if (Instance == null)
            {
                Instance = new TagProcessing();
                LoadScadaConfig();
                StartInputTags();
            }
            return Instance;
        }

        private TagProcessing()
        {
            AlarmOccured += OnAlarmOccured; // ovde ili u nekim metodama koje ce pozivati Trending i AlarmDisplay servisi?
            ValueChanged += OnValueChanged;
        }

        public static void StartInputTags()
        {
            Console.WriteLine("POKRECEM INPUT TAGOVEEEEEEEEEEEEE");
            foreach(KeyValuePair<string, Tag> tagMap in tags)
            {
                if (tagMap.Value is AnalogInput)
                {
                    Thread th = new Thread (() => Instance.StartAnalogInputJob(((AnalogInput)tagMap.Value)));
                    th.Start();
                }
                else if (tagMap.Value is DigitalInput)
                {
                    Thread thh = new Thread(() => Instance.StartDigitalInputJob((DigitalInput)tagMap.Value));
                    thh.Start();
                }
            }         // ne znam sta se desava
        }

        internal void OnAlarmOccured(Alarm alarm)
        {
            AddAlarmToDatabase(alarm);
            AddAlarmToTxt(alarm);
            Console.WriteLine("ALARM OCCURED: " + alarm.TagName + " limit: " + alarm.Limit + ", type: " + alarm.Type);
            // dodaj callback za alarmdisplay
        }


        internal void OnValueChanged(Tag tag)
        {
            AddTagToDatabase(tag);
            Console.WriteLine("VALUE CHANGED: " + tag.Name + ", value: " + tag.Value);
            // plus callback
            // dodaj callback za trending
        }

        #region tags
        internal bool AddAnalogInputTag(string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units)
        {
            try
            {
                AnalogInput tag = new AnalogInput(name, description, ioAddress, driver, scanTime, scanOnOff, lowLimit, highLimit, units);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Thread t = new Thread(() => StartAnalogInputJob(tag));
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

        private void StartAnalogInputJob(AnalogInput tag)
        {
            while (true)
            {
                lock (tagsLocker)
                {
                    if (tags.ContainsKey(tag.Name))
                    {
                        if (tag.ScanOnOff)
                        {
                            double oldValue = tag.Value;
                            double newValue;
                            double driverValue;
                            if (tag.Driver == "Simulation Driver")
                                driverValue = DriversLibrary.SimulationDriver.ReturnValue(tag.IOAddress);

                            else if (tag.Driver == "RealTime Driver")
                                driverValue = DriversLibrary.RealTimeDriver.ReturnValue(tag.IOAddress);

                            else
                                throw new Exception();

                            if (driverValue > tag.HighLimit) newValue = tag.HighLimit;
                            else if (driverValue < tag.LowLimit) newValue = tag.LowLimit;
                            else newValue = driverValue;

                            if (oldValue != newValue)
                            {
                                tag.Value = newValue;
                                ValueChanged?.Invoke(tag);
                            }
                            CheckIfAlarmOccured(tag);
                        }
                    }
                    else
                    {
                        Console.WriteLine("USAO U RETURN");
                        return; // kill thread?
                    }
                }
                Thread.Sleep(1000 * tag.ScanTime);
            }
        }

        private void CheckIfAlarmOccured(AnalogInput tag)
        {
            lock (alarmsLocker)
            {
                foreach (var alarm in tag.Alarms)
                {
                    if ((alarm.Type == AlarmType.LOW && tag.Value <= alarm.Limit) || (alarm.Type == AlarmType.HIGH && tag.Value >= alarm.Limit))
                    {
                        AlarmOccured?.Invoke(alarm);
                    }
                }
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
                DigitalInput tag = new DigitalInput(name, description, ioAddress, driver, scanTime, scanOnOff);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Thread tDigi = new Thread(() => StartDigitalInputJob(tag));
                    tDigi.Start();
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

        private void StartDigitalInputJob(DigitalInput tag)
        {
            while (true)
            {
                lock (tagsLocker)
                {
                    if (tags.ContainsKey(tag.Name))
                    {
                        if (tag.ScanOnOff)
                        {
                            double driverValue;
                            if (tag.Driver == "Simulation Driver")
                                driverValue = DriversLibrary.SimulationDriver.ReturnValue(tag.IOAddress);

                            else if (tag.Driver == "RealTime Driver")
                                driverValue = DriversLibrary.RealTimeDriver.ReturnValue(tag.IOAddress);

                            else
                                throw new Exception("error error wrong driver");

                            tag.Value = driverValue;
                            ValueChanged?.Invoke(tag);
                            // odavde sam premestio sleep zbog lock
                        }
                    }
                    else
                    {
                        Console.WriteLine("usao u return digital");
                        return;
                    }
                }
                Thread.Sleep(1000 * tag.ScanTime);
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

        internal bool AddAlarm(string name, string type, int priority, double limit)
        {
            try
            {
                lock (alarmsLocker)
                {
                    AnalogInput ai = (AnalogInput)tags[name];
                    int id = FindNewAlarmId();
                    Alarm alarm = new Alarm(id, (AlarmType)Enum.Parse(typeof(AlarmType), type), priority, limit, name);
                    ai.Alarms.Add(alarm);
                    alarms.Add(alarm);
                    if (WriteXmlConfig())
                    {
                        Console.WriteLine("New alarm added for tag: " + name);
                        return true;
                    }
                    else return false;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private int FindNewAlarmId()
        {
            int newId = 0;

            foreach(Alarm a in alarms)
            {
                if (a.Id > newId)
                    newId = a.Id;
            }

            return ++newId;
        }

        internal bool RemoveAlarm(int id)
        {
            try
            {
                lock (alarmsLocker)
                {
                    foreach (Alarm alarm in alarms)
                    {
                        if (alarm.Id == id)
                        {
                            ((AnalogInput)tags[alarm.TagName]).Alarms.Remove(alarm);

                            if (alarms.Remove(alarm) && WriteXmlConfig())
                            {
                                Console.WriteLine("Alarm deleted!");
                                return true;
                            }
                            else return false;
                        }
                    }
                    return false;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        internal bool RemoveTag(string tagName)
        {
            try
            {
                lock (tagsLocker)
                {
                    if (tags.Remove(tagName) && WriteXmlConfig())   // i zaustavi sve tredove za ovaj tag
                    {
                        Console.WriteLine("Tag removed: " + tagName);
                        return true;
                    }
                    else return false;
                }
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

        #region console_prints
        internal string PrintTags(string ioType, string adType, bool value, bool scan)
        {
            string retStr = "==================================================================================================================\n";

            retStr += "|      TAG NAME      |  INPUT/OUTPUT  | ANALOG/DIGITAL |               DESCRIPTION               |";
            if (value) retStr += "VALUE|";
            if (scan) retStr += "SCAN ON/OFF|";

            retStr += "\n------------------------------------------------------------------------------------------------------------------\n";

            foreach (KeyValuePair<string, Tag> tag in tags)
            {
                if ((ioType == "input" && tag.Value is InputTag) || (ioType == "output" && tag.Value is OutputTag) || ioType == "")
                {
                    if ((adType == "analog" && (tag.Value is AnalogInput || tag.Value is AnalogOutput)) || adType == "")
                    {                                                                                                           // ne proveravam za Digital jer mi ne treba 
                        string IOtype = (tag.Value is InputTag) ? "INPUT" : "OUTPUT";                                           // za sad nigde samo digital tagove da ispisujem
                        string digAnaType = (tag.Value is DigitalInput || tag.Value is DigitalOutput) ? "DIGITAL" : "ANALOG";
                        retStr += String.Format("|{0,-20}|{1,-16}|{2,-16}|{3,-41}|", tag.Value.Name, IOtype, digAnaType, tag.Value.Description);

                        if (value) retStr += String.Format("{0,5}|", tag.Value.Value);

                        if (scan) retStr += String.Format("{0,-11}|", ((InputTag)tag.Value).ScanOnOff);

                        retStr += "\n";
                        retStr += "------------------------------------------------------------------------------------------------------------------\n";
                    }
                }
            }
            retStr += "==================================================================================================================";

            return retStr;
        }

        internal string PrintAlarmsForTag(string tagName)
        {
            string retStr = "===================================================\n";
            retStr +=       "|ID|      TAG NAME      | TYPE | PRIORITY | LIMIT |";
            retStr +=     "\n---------------------------------------------------\n";

            foreach (Alarm alarm in alarms)
            {
                if (alarm.TagName == tagName)
                {                                                                                                           
                    retStr += String.Format("|{0,-2}|{1,-20}|{2,-6}|{3,-10}|{4,7}|", alarm.Id, alarm.TagName, alarm.Type, alarm.Priority, alarm.Limit);
                    retStr += "\n";
                    retStr += "---------------------------------------------------\n";
                }
            }
            retStr += "===================================================\n";

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
                doc.Descendants("tag").Remove();
                doc.Descendants("alarm").Remove();
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
                Console.WriteLine("LOAD SCADA CONFIGGGGG");
                XElement configElements = XElement.Load(CONFIG_FILE_PATH);
                var tagConfig = configElements.Descendants("tag");
                LoadTags(tagConfig);
                var alarmConfig = configElements.Descendants("alarm");
                LoadAlarms(alarmConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void LoadAlarms(IEnumerable<XElement> alarmConfig)
        {
            foreach (var a in alarmConfig)
            {
                Alarm alarm = Alarm.MakeAlarmFromConfigFile(a);
                ((AnalogInput)tags[alarm.TagName]).Alarms.Add(alarm);
                alarms.Add(alarm);
            }
        }

        private static void LoadTags(IEnumerable<XElement> tagConfig)
        {
            foreach (var t in tagConfig)
            {
                Tag tag = Tag.MakeTagFromConfigFile(t);
                if (tag is InputTag) FindValueOfTag(ref tag);
                tags.Add(tag.Name, tag);
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

        #region database
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

        private bool AddAlarmToDatabase(Alarm alarm)
        {
            using (var db = new AlarmContext())
            {
                try
                {
                    db.Alarms.Add(new AlarmDTO(alarm.TagName, alarm.Type, DateTime.Now));
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

        private void AddAlarmToTxt(Alarm alarm)
        {
            try
            {
                string stringToWrite = "";
                File.AppendAllText(ALARM_TXT_PATH, alarm.Id + "|" + alarm.TagName + "|" + alarm.Type + "|" + DateTime.Now + Environment.NewLine);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion
    }
}
