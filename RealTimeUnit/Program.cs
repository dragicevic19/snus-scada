using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RealTimeUnit.ServiceReference;

namespace RealTimeUnit
{
    class Program
    {
        public static string ContainerName { get; set; }
        public static CspParameters csp = new CspParameters();
        public static RSACryptoServiceProvider rsa;
        const string KEY_STORE_NAME = "MyKeyStore";
        public static int id { get; set; }

        static void Main(string[] args)
        {
            RealTimeUnitServiceClient proxy = new RealTimeUnitServiceClient();

            id = proxy.PubInit();
            string pubKeyPath = @"..\..\..\public_key\id" + id.ToString() + ".txt";
            CreateAsmKeys(out string containterName, true);
            ExportPublicKey();
            proxy.ExportPublicKey(id, pubKeyPath);

            string address;
            double highLimit, lowLimit;
            while (true)
            {
                try
                {
                    Console.Write("Enter Real-Time Driver address >> ");
                    address = Console.ReadLine();

                    Console.Write("Enter upper limit value >> ");
                    highLimit = double.Parse(Console.ReadLine());

                    Console.Write("Enter lower limit value >> ");
                    lowLimit = double.Parse(Console.ReadLine());

                    break;
                } catch (Exception)
                {
                    Console.WriteLine("Invalid input!");
                }
            }
            Random r = new Random();
            while (true)
            {
                double randValue = (r.NextDouble() * (highLimit - lowLimit)) + lowLimit;
                string message = address + "|" + randValue.ToString();
                byte[] signature = SignMessage(message, out byte[] hashValue);
                proxy.SendValue(id, message, signature);
            }
        }

        private static byte[] SignMessage(string message, out byte[] hashValue)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                CspParameters csp = new CspParameters();
                csp.KeyContainerName = ContainerName;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }

        private static void CreateAsmKeys(out string containerName, bool useMachineKeyStore)
        {
            csp.KeyContainerName = KEY_STORE_NAME;
            if (useMachineKeyStore)
                csp.Flags = CspProviderFlags.UseMachineKeyStore;
            rsa = new RSACryptoServiceProvider(csp);
            rsa.PersistKeyInCsp = true;
            CspKeyContainerInfo info = new CspKeyContainerInfo(csp);
            Console.WriteLine($"The key container name: {info.KeyContainerName}");
            containerName = info.KeyContainerName;
            Console.WriteLine($"Unique key container name: {info.UniqueKeyContainerName}");
        }

        const string EXPORT_FOLDER = @"..\..\..\public_key\";
        private static void ExportPublicKey()
        {
            string PUBLIC_KEY_FILE = "id" + id.ToString() + ".txt";

            if (!Directory.Exists(EXPORT_FOLDER))
            {
                Directory.CreateDirectory(EXPORT_FOLDER);
            }
            using (StreamWriter writer = new StreamWriter(Path.Combine(EXPORT_FOLDER,
            PUBLIC_KEY_FILE)))
            {
                writer.WriteLine(rsa.ToXmlString(false));
            }
        }
    }
}
