using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RealTimeUnit.ServiceReference;

namespace RealTimeUnit
{
    class Program
    {
        private static CspParameters csp;
        private static RSACryptoServiceProvider rsa;

        public static int id { get; set; }

        static void Main(string[] args)
        {
            RealTimeUnitServiceClient proxy = new RealTimeUnitServiceClient();

            id = proxy.PubInit();
            string pubKeyPath = @"..\..\..\public_key\id" + id.ToString() + ".txt";

            CreateAsmKeys();
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

            Console.WriteLine("Real-Time Unit is now sending data to Real-Time Driver...");
            Random r = new Random();
            while (true)
            {
                double randValue = (r.NextDouble() * (highLimit - lowLimit)) + lowLimit;
                string message = address + "|" + randValue.ToString();
                byte[] signature = SignMessage(message);
                proxy.SendValue(id, message, signature);
                Thread.Sleep(500); // mozda i ne treba al ajd
            }
        }

        private static byte[] SignMessage(string message)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));

                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");

                return formatter.CreateSignature(hashValue);
            }
        }

        private static void CreateAsmKeys()
        {
            csp = new CspParameters();
            rsa = new RSACryptoServiceProvider(csp);
        }

        const string EXPORT_FOLDER = @"..\..\..\public_key\";
        private static void ExportPublicKey()
        {
            string PUBLIC_KEY_FILE = "id" + id.ToString() + ".txt";

            if (!Directory.Exists(EXPORT_FOLDER))
            {
                Directory.CreateDirectory(EXPORT_FOLDER);
            }
            using (StreamWriter writer = new StreamWriter(Path.Combine(EXPORT_FOLDER, PUBLIC_KEY_FILE)))
            {
                writer.WriteLine(rsa.ToXmlString(false));
            }
        }
    }
}
