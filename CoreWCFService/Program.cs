﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CoreWCFService
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManagerService.LoadScadaConfig();   // da li ovako da pozivam ucitavanje config fajla?
            new TagProcessing().StartInputTags();

            ServiceHost svc = new ServiceHost(typeof(DatabaseManagerService));
            svc.Open();
            Console.WriteLine("Database Manager Service is ready...");

            Console.ReadKey();
        }
    }
}
