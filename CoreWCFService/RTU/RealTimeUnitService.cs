using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService.RTU
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RealTimeUnitService : IRealTimeUnitService
    {
        private static CspParameters csp;
        private static RSACryptoServiceProvider rsa;

        private Dictionary<int, string> PubKeys = new Dictionary<int, string>();
        static readonly object pubKeysLocker = new object();

        static int pubId = 0;
        static readonly object pubIdlocker = new object();

        static readonly object rtuLocker = new object();

        public void ExportPublicKey(int pubId, string keyPath)
        {
            lock (pubKeysLocker)
            {
                PubKeys[pubId] = keyPath;
            }
        }

        public int PubInit()
        {
            lock (pubIdlocker)
            {
                return pubId++;
            }
        }

        public void SendValue(int pubId, string message, byte[] signature)
        {
            try
            {
                string path = PubKeys[pubId];
                ImportPublicKey(path);

                if (VerifySignedMessage(message, signature))
                {
                    string[] rtuMessage = message.Split('|');
                    string address = rtuMessage[0];
                    double value = double.Parse(rtuMessage[1]);
                    lock (rtuLocker)
                    {
                        DriversLibrary.RealTimeDriver.SetRTUValue(address, value);
                    }
                }
                else
                {
                    Console.WriteLine("Message is not valid");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Something happened while trying to read message");
            }
        }

        private static void ImportPublicKey(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    csp = new CspParameters();
                    rsa = new RSACryptoServiceProvider(csp);
                    string publicKeyText = reader.ReadToEnd();
                    rsa.FromXmlString(publicKeyText);
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }
        }

        private static bool VerifySignedMessage(string message, byte[] signature)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA256");
                return deformatter.VerifySignature(hashValue, signature);
            }
        }
    }
}
