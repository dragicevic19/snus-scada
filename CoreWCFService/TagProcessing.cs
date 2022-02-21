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

        private static Dictionary<string, Thread> tagThreads = new Dictionary<string, Thread>();
        static readonly object threadLocker = new object();

        private static List<Alarm> alarms = new List<Alarm>();
        static readonly object alarmsLocker = new object();

        public delegate void AlarmHandler(Alarm alarm, DateTime timeStamp);
        public delegate void ValueHandler(TagDb tag);

        public event AlarmHandler AlarmOccurred;
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
            AlarmOccurred += OnAlarmOccurred;
            ValueChanged += OnValueChanged;
        }

        public static void StartInputTags()
        {
            Console.WriteLine("Starting input tags...");
            foreach (KeyValuePair<string, Tag> tagMap in tags)
            {
                if (tagMap.Value is AnalogInput)
                {
                    Thread th = new Thread(() => Instance.StartAnalogInputJob(((AnalogInput)tagMap.Value)));
                    th.Start();
                    lock (threadLocker)
                    {
                        tagThreads.Add(tagMap.Value.Name, th);
                    }
                }
                else if (tagMap.Value is DigitalInput)
                {
                    Thread thh = new Thread(() => Instance.StartDigitalInputJob((DigitalInput)tagMap.Value));
                    thh.Start();
                    lock (threadLocker)
                    {
                        tagThreads.Add(tagMap.Value.Name, thh);
                    }
                }
            }
        }

        #region eventHandlers

        internal void AddProxyForTrending(ITrendingCallback proxy)
        {
            ValueChanged += proxy.OnValueChanged;
        }
        internal void AddProxyForAlarmDisplay(IAlarmDisplayCallback proxy)
        {
            AlarmOccurred += proxy.OnAlarmOccurred;
        }

        internal void OnAlarmOccurred(Alarm alarm, DateTime timestamp)
        {
            AddAlarmToDatabase(alarm);
            AddAlarmToTxt(alarm);
        }
        internal void OnValueChanged(TagDb tag)
        {
            AddTagToDatabase(tag);
        }

        #endregion

        #region tags
        internal bool AddAnalogInputTag(string name, string description, string driver, string ioAddress, int scanTime, bool scanOnOff, double lowLimit, double highLimit, string units)
        {
            try
            {
                AnalogInput tag = new AnalogInput(name, description, ioAddress, driver, scanTime, scanOnOff, lowLimit, highLimit, units);
                lock (tagsLocker)
                {
                    tags.Add(tag.Name, tag);
                }
                if (AddTagToDatabase(new TagDb(tag.Name, tag.GetType().Name, tag.Value, DateTime.Now)) && WriteXmlConfig())
                {
                    Thread t = new Thread(() => StartAnalogInputJob(tag));
                    t.Start();
                    lock (threadLocker)
                    {
                        tagThreads.Add(tag.Name, t);
                    }
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

                    if (driverValue == -1000) continue;

                    if (driverValue > tag.HighLimit) 
                        newValue = tag.HighLimit;

                    else if (driverValue < tag.LowLimit) 
                        newValue = tag.LowLimit;

                    else newValue = driverValue;

                    if (oldValue != newValue)
                    {
                        lock (tagsLocker)
                        {
                            tag.Value = newValue;
                        }
                        ValueChanged?.Invoke(new TagDb(tag.Name, tag.GetType().Name, tag.Value, DateTime.Now));
                    }
                    CheckIfAlarmOccurred(tag);
                    Thread.Sleep(1000 * tag.ScanTime);
                }
            }
        }

        private void CheckIfAlarmOccurred(AnalogInput tag)
        {
            foreach (var alarm in tag.Alarms)
            {
                if ((alarm.Type == AlarmType.LOW && tag.Value <= alarm.Limit) || (alarm.Type == AlarmType.HIGH && tag.Value >= alarm.Limit))
                {
                    AlarmOccurred?.Invoke(alarm, DateTime.Now);
                }
            }
        }

        internal bool AddAnalogOutputTag(string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units)
        {
            try
            {
                Tag tag = new AnalogOutput(name, description, ioAddress, initValue, lowLimit, highLimit, units);
                lock (tagsLocker)
                {
                    tags.Add(tag.Name, tag);
                }
                if (AddTagToDatabase(new TagDb(tag.Name, tag.GetType().Name, tag.Value, DateTime.Now)) && WriteXmlConfig())
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
                lock (tagsLocker)
                {
                    tags.Add(tag.Name, tag);
                }
                if (AddTagToDatabase(new TagDb(tag.Name, tag.GetType().Name, tag.Value, DateTime.Now)) && WriteXmlConfig())
                {
                    Thread tDigi = new Thread(() => StartDigitalInputJob(tag));
                    tDigi.Start();
                    lock (threadLocker)
                    {
                        tagThreads.Add(tag.Name, tDigi);
                    }
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
                if (tag.ScanOnOff)
                {
                    double driverValue;
                    if (tag.Driver == "Simulation Driver")
                        driverValue = DriversLibrary.SimulationDriver.ReturnValue(tag.IOAddress);

                    else if (tag.Driver == "RealTime Driver")
                        driverValue = DriversLibrary.RealTimeDriver.ReturnValue(tag.IOAddress);

                    else
                        throw new Exception("error error wrong driver");

                    if (driverValue == -1000) continue;
                    lock (tagsLocker)
                    {
                        tag.Value = driverValue;
                    }
                    ValueChanged?.Invoke(new TagDb(tag.Name, tag.GetType().Name, tag.Value, DateTime.Now));
                    Thread.Sleep(1000 * tag.ScanTime);
                }
            }
        }

        internal bool AddDigitalOutputTag(string name, string description, string ioAddress, double initValue)
        {
            try
            {
                Tag tag = new DigitalOutput(name, description, ioAddress, initValue);
                lock (tagsLocker)
                {
                    tags.Add(tag.Name, tag);
                }
                if (AddTagToDatabase(new TagDb(tag.Name, tag.GetType().Name, tag.Value, DateTime.Now)) && WriteXmlConfig())
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
                AnalogInput ai = (AnalogInput)tags[name];
                int id = FindNewAlarmId();
                Alarm alarm = new Alarm(id, (AlarmType)Enum.Parse(typeof(AlarmType), type), priority, limit, name);
                ai.Alarms.Add(alarm);
                lock (alarmsLocker)
                {
                    alarms.Add(alarm);
                }
                if (WriteXmlConfig())
                {
                    Console.WriteLine("New alarm added for tag: " + name);
                    return true;
                }
                else return false;
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
                foreach (Alarm alarm in alarms)
                {
                    if (alarm.Id == id)
                    {
                        lock (tagsLocker)
                        {
                            ((AnalogInput)tags[alarm.TagName]).Alarms.Remove(alarm);
                        }
                        lock (alarmsLocker)
                        {
                            if (!alarms.Remove(alarm)) return false;
                        }
                        if (WriteXmlConfig())
                        {
                            Console.WriteLine("Alarm deleted!");
                            return true;
                        }
                        else return false;
                    }
                }
                return false;
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
                if (tags[tagName] is InputTag)
                {
                    lock (threadLocker)
                    {
                        tagThreads[tagName].Abort();
                        tagThreads.Remove(tagName);
                    }
                }

                lock (tagsLocker)
                {
                    if (!tags.Remove(tagName)) return false;
                }
                
                if (WriteXmlConfig())
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
                lock (tagsLocker)
                {
                    ((InputTag)tags[tagName]).ScanOnOff = true;
                }
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
                lock (tagsLocker)
                {
                    ((InputTag)tags[tagName]).ScanOnOff = false;
                }
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -20000;
            }
        }

        internal bool ChangeOutputValue(string tagName, double value)
        {
            try
            {
                lock (tagsLocker)
                {
                    tags[tagName].Value = value;
                }

                if (AddTagToDatabase(new TagDb(tags[tagName].Name, tags[tagName].Value.GetType().Name, tags[tagName].Value, DateTime.Now)))
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
                Console.WriteLine("Loading SCADA configuration...");
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
                lock (tagsLocker)
                {
                    ((AnalogInput)tags[alarm.TagName]).Alarms.Add(alarm);
                }
                lock (alarmsLocker)
                {
                    alarms.Add(alarm);
                }
            }
        }

        private static void LoadTags(IEnumerable<XElement> tagConfig)
        {
            foreach (var t in tagConfig)
            {
                Tag tag = Tag.MakeTagFromConfigFile(t);
                if (tag is InputTag) FindValueOfTag(ref tag);
                lock (tagsLocker) {
                    tags.Add(tag.Name, tag);
                }
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

            TagDb last = allTags.Last();
            lock (tagsLocker)
            {
                tag.Value = last.Value;
            }
        }

        #endregion

        #region database
        private bool AddTagToDatabase(TagDb tag)
        {
            using (var db = new TagContext())
            {
                try
                {
                    db.Tags.Add(tag);
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
                    db.Alarms.Add(new AlarmDTO(alarm.TagName, alarm.Type, DateTime.Now, alarm.Priority));
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
                File.AppendAllText(ALARM_TXT_PATH, alarm.Id + "|" + alarm.TagName + "|" + alarm.Type + "|" + alarm.Priority + "|" + DateTime.Now + Environment.NewLine);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region reports
        internal AlarmDTO[] GetAlarmsOverAPeriodOfTime(DateTime start,  DateTime end)
        {
            try
            {
                using (var db = new AlarmContext())
                {
                    var query =
                        from alarm in db.Alarms
                        where alarm.TimeStamp >= start && alarm.TimeStamp <= end
                        select alarm;

                    return query.OrderByDescending(a => a.Priority).ThenByDescending(a => a.TimeStamp).ToArray();
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        internal AlarmDTO[] GetAlarmsWithCertainPriority(int priority)
        {
            try
            {
                using (var db = new AlarmContext())
                {
                    var query =
                        from alarm in db.Alarms
                        where alarm.Priority == priority
                        select alarm;

                    return query.OrderByDescending(a => a.TimeStamp).ToArray();
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
}

        internal TagDb[] GetTagsOverAPeriodOfTime(DateTime start, DateTime end)
        {
            try
            {
                using (var db = new TagContext())
                {
                    var query =
                        from tag in db.Tags
                        where tag.TimeStamp >= start && tag.TimeStamp <= end
                        select tag;

                    return query.OrderByDescending(t => t.TimeStamp).ToArray();
                }
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        internal TagDb[] GetTagsWithCertainTagName(string tagName)
        {
            try
            {
                using (var db = new TagContext())
                {
                    var query =
                        from tag in db.Tags
                        where tag.TagName == tagName
                        select tag;

                    return query.OrderByDescending(t => t.TimeStamp).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        internal TagDb[] GetLastValueOfDITags()
        {
            Dictionary<string, TagDb> ret = new Dictionary<string, TagDb>();
            
            try
            {
                using (var db = new TagContext())
                {
                    var query =
                        from tag in db.Tags
                        where tag.Type == typeof(DigitalInput).Name
                        select tag;

                    foreach (var q in query.OrderByDescending(m => m.TagName).ThenBy(m => m.TimeStamp))
                    {
                        ret[q.TagName] = q;
                    }

                    return ret.Values.OrderByDescending(m => m.TimeStamp).ToArray();
                }
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        internal TagDb[] GetLastValueOfAITags()
        {
            try
            {
                Dictionary<string, TagDb> ret = new Dictionary<string, TagDb>();
                using (var db = new TagContext())
                {
                    var query =
                        from tag in db.Tags
                        where tag.Type == typeof(AnalogInput).Name
                        select tag;

                    foreach (var q in query.OrderByDescending(m => m.TagName).ThenBy(m => m.TimeStamp))
                    {
                        ret[q.TagName] = q;
                    }

                    return ret.Values.OrderByDescending(m => m.TimeStamp).ToArray();
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion
    }
}
