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
        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();
        private static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();    // tagName is key

        const string LOGIN_FAILED_STR = "Login failed";
        const string CONFIG_FILE_PATH = @"../../data/scadaConfig.xml";

        public bool Registration(string username, string password)
        {
            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);

            using (var db = new UserContext())
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                } catch(Exception e)
                {
                    return false;
                }
            }
            Console.WriteLine("User successfully registered: " + username);
            return true;
        }

        public void LoadScadaConfig()
        {
            try
            {
                XElement configElements = XElement.Load(CONFIG_FILE_PATH);
                var tagConfig = configElements.Descendants("tag");
                foreach (var t in tagConfig)
                {
                    Tag tag = Tag.MakeTagFromConfigFile(t);
                    FindValueOfTag(ref tag);
                    tags.Add(tag.Name, tag);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void FindValueOfTag(ref Tag tag)
        {
            List<TagDb> allTags = new List<TagDb>();
            using (var db = new TagContext())
            {
                foreach(var t in db.Tags)
                {
                    if (t.TagName == tag.Name)  // bar se nadam da u bazi pakuje po redosledu upisivanja
                    {
                        allTags.Add(t);
                    }
                }
            }
            if (allTags.Count == 0) return;
            
            TagDb last = allTags.Last();
            tag.Value = last.Value;
        }

        public string Login(string username, string password)
        {
            using (var db = new UserContext())
            {
                foreach( var user in db.Users)
                {
                    if (username == user.Username && ValidateEncryptedData(password, user.EncryptedPassword))
                    {
                        string token = GenerateToken(username);
                        authenticatedUsers.Add(token, user);
                        //if (tags.Values.Count == 0) LoadScadaConfig();
                        Console.WriteLine("User successfully logged in: " + username + ", with token: " + token);

                        return token;
                    }
                }
            }
            return LOGIN_FAILED_STR;
        }

        public bool Logout(string token)
        {
            Console.WriteLine("User trying to logout with token: " + token);
            return authenticatedUsers.Remove(token);
        }


        public bool AddDigitalInputTag(string token, string name, string description, string driver, string ioAddress, double scanTime, bool scanOnOff)
        {
            if (!IsUserAuthenticated(token)) return false;
            try
            {
                Tag tag = new DigitalInput(name, description, ioAddress, driver, scanTime, scanOnOff);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Console.WriteLine("New tag added: " + name);
                    return true;
                }
                else
                {
                    return false;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }



        public bool AddDigitalOutputTag(string token, string name, string description, string ioAddress, double initValue)
        {
            if (!IsUserAuthenticated(token)) return false;
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
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddAnalogInputTag(string token, string name, string description, string driver, string ioAddress, double scanTime, bool scanOnOff, double lowLimit, double highLimit, string units)
        {
            if (!IsUserAuthenticated(token)) return false;
            try
            {
                Tag tag = new AnalogInput(name, description, ioAddress, driver, scanTime, scanOnOff, lowLimit, highLimit, units);
                tags.Add(tag.Name, tag);
                if (AddTagToDatabase(tag) && WriteXmlConfig())
                {
                    Console.WriteLine("New tag added: " + name);
                    return true;
                }
                else
                    return false;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddAnalogOutputTag(string token, string name, string description, string ioAddress, double initValue, double lowLimit, double highLimit, string units)
        {
            if (!IsUserAuthenticated(token)) return false;
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
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

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

        private bool WriteXmlConfig()
        {
            try
            {
                if (!File.Exists(CONFIG_FILE_PATH)) CreateNewXmlFile();

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

        private static void CreateNewXmlFile()  // ovo ne radi ne znam kako
        {
            Console.WriteLine("Usao ovde");
            XmlDocument newDoc = new XmlDocument();
            newDoc.LoadXml("<root></root>");
            XmlTextWriter writer = new XmlTextWriter(CONFIG_FILE_PATH, null);
            writer.Formatting = Formatting.Indented;
            newDoc.Save(CONFIG_FILE_PATH);
        }

        
        public bool RemoveTag(string token, string tagName)
        {
            if (!IsUserAuthenticated(token)) return false;
            try
            {
                if (tags.Remove(tagName) && WriteXmlConfig())
                {
                    Console.WriteLine("Tag removed: " + tagName);
                    return true;
                }
                else return false;

            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ChangeOutputValue(string token, string tagName, double value)
        {
            if (!IsUserAuthenticated(token)) return false;
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
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public double GetOutputValue(string token, string tagName)
        {
            if (!IsUserAuthenticated(token)) return -10000;
            try
            {
                Console.WriteLine("Output value returned: " + tags[tagName].Value);
                return tags[tagName].Value;

            } catch (Exception e)   // povratne vrednosti sredi
            {
                Console.WriteLine(e.Message);
                return -20000;
            }
        }

        public bool TurnScanOff(string token, string tagName)
        {
            if (!IsUserAuthenticated(token)) return false;
            try
            {
                ((InputTag)tags[tagName]).ScanOnOff = false;
                if (WriteXmlConfig())
                {
                    Console.WriteLine("Scan turned OFF for tag: " + tagName);
                    return true;
                }
                else return false;

            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool TurnScanOn(string token, string tagName)
        {
            if (!IsUserAuthenticated(token)) return false;
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

        public string GetStringForPrintingTags(string token, string type="", bool value = false, bool scan = false)
        {
            if (!IsUserAuthenticated(token)) return "Wrong authentication token";

            string retStr = "==================================================================================================================\n";

            retStr += "|      TAG NAME      |  INPUT/OUTPUT  | ANALOG/DIGITAL |               DESCRIPTION               |";
            if (value) retStr += "VALUE|";
            if (scan) retStr += "SCAN ON/OFF|";

            retStr += "\n------------------------------------------------------------------------------------------------------------------\n";

            foreach(KeyValuePair<string, Tag> tag in tags)
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

        private string EncryptData(string password)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);

                return Convert.ToBase64String(salt);
            }

            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);

                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }

            return EncryptValue(password);
        }

        private bool ValidateEncryptedData(string password, string encryptedPassword)
        {
            string[] arrValues = encryptedPassword.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];

            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + password);

            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);

                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        private string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            string randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }

        private bool IsUserAuthenticated(string token)
        {
            return authenticatedUsers.ContainsKey(token);
        }


    }
}
