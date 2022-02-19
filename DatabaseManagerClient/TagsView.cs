using System;
using DatabaseManagerClient.ServiceReference;

namespace DatabaseManagerClient
{
    public class TagsView
    {
        private DatabaseManagerServiceClient Proxy = null;
        private AuthenticationClient AuthProxy = null;
        public string Token { get; set; }

        public TagsView(DatabaseManagerServiceClient proxy, AuthenticationClient authProxy)
        {
            Proxy = proxy;
            AuthProxy = authProxy;
        }

        public void Start(string token)
        {
            Token = token;

            while (true)
            {
                System.Console.Clear();

                Console.WriteLine("\n1 - Add ANALOG tag\n2 - Add DIGITAL tag\n3 - Add alarm\n4 - Remove tag\n5 - Change output value\n6 - Show values of output tags" +
                    "\n7 - Turn scan on\n8 - Turn scan off\n9 - Log out\n");
                Console.Write(">> ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddAnalogTag();
                        break;
                    case "2":
                        AddDigitalTag();
                        break;
                    case "3":
                        AddAlarm();
                    case "4":
                        RemoveTag();
                        break;
                    case "5":
                        ChangeOutputValue();
                        break;
                    case "6":
                        ShowValuesOfOutputTags();
                        break;
                    case "7":
                        TurnScanOn();
                        break;
                    case "8":
                        TurnScanOff();
                        break;
                    case "9":
                        if (AuthProxy.Logout(Token))
                        {
                            Console.WriteLine("Successfully logged out");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            return;
                        } else
                        {
                            Console.WriteLine("An error occurred while logging out");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input!\n");
                        break;
                }
            }
        }

        private void AddAlarm()
        {
            System.Console.Clear();
            try
            {
                Console.WriteLine(Proxy.GetStringForPrintingTags(Token, type: "input", value: false, scan: true));
                Console.Write("Enter name of the tag you want to change: ");
                string tagName = Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void AddAnalogTag()
        {
            System.Console.Clear();

            Console.WriteLine("\n1 - Add analog INPUT tag\n2 - Add analog OUTPUT tag\nX - Tag Menu (Back)");
            Console.Write(">> ");
            string input = Console.ReadLine();
            switch (input.ToUpper())
            {
                case "1":
                    AddAnalogInputTag();
                    break;

                case "2":
                    AddAnalogOutput();
                    break;

                case "X":
                    return;
                default:
                    Console.WriteLine("Invalid input!\n");
                    break;
            }
        }
        private void AddAnalogInputTag()
        {
            AnalogInputBuilder tag = new AnalogInputBuilder();
            tag.Build();
            if (!tag.Error)
            {
                Console.WriteLine("Adding analog input tag...");
                if (Proxy.AddAnalogInputTag(Token, tag.Name, tag.Description, tag.Driver, tag.IOAddress, tag.ScanTime,
                    tag.ScanOnOff, tag.LowLimit, tag.HighLimit, tag.Units))
                {
                    Console.WriteLine("Analog input tag added successfully");
                }
                else
                {
                    Console.WriteLine("An error occurred while adding the tag");
                }
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        private void AddAnalogOutput()
        {
            AnalogOutputBuilder tag = new AnalogOutputBuilder();
            tag.Build();
            if (!tag.Error)
            {
                Console.WriteLine("Adding analog output tag...");
                if (Proxy.AddAnalogOutputTag(Token, tag.Name, tag.Description, tag.IOAddress, tag.InitValue,
                    tag.LowLimit, tag.HighLimit, tag.Units))
                {
                    Console.WriteLine("Analog output tag added successfully");
                }
                else
                {
                    Console.WriteLine("An error occurred while adding the tag");
                }
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }


        private void AddDigitalTag()
        {
            System.Console.Clear();

            Console.WriteLine("\n1 - Add digital INPUT tag\n2 - Add digital OUTPUT tag\nX - Tag Menu (Back)");
            Console.Write(">> ");
            string input = Console.ReadLine();
            switch (input.ToUpper())
            {
                case "1":
                    AddDigitalInput();
                    break;

                case "2":
                    AddDigitalOutput();
                    break;

                case "X":
                    return;
                default:
                    Console.WriteLine("Invalid input!\n");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
        private void AddDigitalInput()
        {
            DigitalInputBuilder tag = new DigitalInputBuilder();
            tag.Build();
            if (!tag.Error)
            {
                Console.WriteLine("Adding digital input tag...");

                if (Proxy.AddDigitalInputTag(Token, tag.Name, tag.Description, tag.Driver, tag.IOAddress, tag.ScanTime, tag.ScanOnOff))
                {
                    Console.WriteLine("Digital input tag added successfully");
                }
                else
                {
                    Console.WriteLine("An error occurred while adding the tag");
                }
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        private void AddDigitalOutput()
        {
            DigitalOutputBuilder tag = new DigitalOutputBuilder();
            tag.Build();
            if (!tag.Error)
            {
                Console.WriteLine("Adding digital output tag...");

                if (Proxy.AddDigitalOutputTag(Token, tag.Name, tag.Description, tag.IOAddress, tag.InitValue))
                {
                    Console.WriteLine("Digital output tag added successfully");
                }
                else
                {
                    Console.WriteLine("An error occurred while adding the tag");
                }
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }


        private void RemoveTag()
        {
            System.Console.Clear();

            Console.WriteLine(Proxy.GetStringForPrintingTags(Token, type:"", value:false, scan:false));
            Console.Write("Enter name of the tag you want to remove: ");
            string tagName = Console.ReadLine();

            if (Proxy.RemoveTag(Token, tagName))
            {
                Console.WriteLine("Tag removed successfully");
            }
            else
            {
                Console.WriteLine("An error occurred while removing the tag");
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void ChangeOutputValue()
        {
            System.Console.Clear();
            try
            {
                Console.WriteLine(Proxy.GetStringForPrintingTags(Token, type: "output", value: true, scan: false));
                Console.Write("Enter name of the tag you want to change: ");
                string tagName = Console.ReadLine();

                Console.Write("Enter new value: ");
                double value = double.Parse(Console.ReadLine());

                if (Proxy.ChangeOutputValue(Token, tagName, value))
                {
                    Console.WriteLine("Tag value changed successfully");
                }
                else
                {
                    Console.WriteLine("An error occurred while changing value of the tag");
                }
                Console.Write("Press any key to continue...");
                Console.ReadKey();

            } catch (Exception e)
            {
                Console.WriteLine("Invalid input");
                Console.Write("Press any key to continue...");

            }

        }

        private void ShowValuesOfOutputTags()
        {
            System.Console.Clear();
            Console.WriteLine(Proxy.GetStringForPrintingTags(Token, type: "output", value: true, scan: false));
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void TurnScanOn()
        { 
            System.Console.Clear();
            Console.WriteLine(Proxy.GetStringForPrintingTags(Token, type: "input", value: false, scan: true));
            Console.Write("Enter name of the tag for which you want to turn scan ON: ");
            string tagName = Console.ReadLine();

            if (Proxy.TurnScanOn(Token, tagName))
            {
                Console.WriteLine("Tag scan: ON");
            }
            else
            {
                Console.WriteLine("An error occurred while setting scan ON");
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void TurnScanOff()
        {
            System.Console.Clear();
            Console.WriteLine(Proxy.GetStringForPrintingTags(Token, type: "input", value: false, scan: true));
            Console.Write("Enter name of the tag for which you want to turn scan OFF: ");
            string tagName = Console.ReadLine();

            if (Proxy.TurnScanOff(Token, tagName))
            {
                Console.WriteLine("Tag scan: OFF");
            }
            else
            {
                Console.WriteLine("An error occurred while setting scan OFF");
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

    }
}