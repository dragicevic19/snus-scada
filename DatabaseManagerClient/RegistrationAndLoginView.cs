using System;
using DatabaseManagerClient.ServiceReference;

namespace DatabaseManagerClient
{
    public class RegistrationAndLoginView
    {
        private static RegistrationAndLoginView Instance = null;
        private static DatabaseManagerServiceClient Proxy = null;
        private static AuthenticationClient AuthProxy = null;
        private static TagsView TagsView = null;

        const string LOGIN_FAILED_STR = "Login failed";

        public static RegistrationAndLoginView GetInstance(DatabaseManagerServiceClient newProxy, AuthenticationClient authProxy, TagsView tagsView)
        {
            Proxy = newProxy;
            AuthProxy = authProxy;
            TagsView = tagsView;

            if (Instance == null)
            {
                Instance = new RegistrationAndLoginView();
            }
            return Instance;
        }

        private RegistrationAndLoginView() { }

        public void Start()
        {
            while (true)
            {
                System.Console.Clear();
                Console.WriteLine("\n1 - Registration\n2 - Login\n3 - Exit");
                Console.Write(">> ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        StartRegistration();
                        break;
                    case "2":
                        StartLogin();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid input!\n");
                        break;
                }
            }
        }

        public void StartLogin() 
        {
            System.Console.Clear();
            Console.Write("Username >> ");
            string username = Console.ReadLine();
            Console.Write("Password >> ");
            string password = Console.ReadLine();

            if (ValidInput(username, password))
            {
                string token = AuthProxy.Login(username, password);
                if (token != LOGIN_FAILED_STR)
                {
                    TagsView.Start(token);
                }
                else
                {
                    Console.WriteLine(LOGIN_FAILED_STR);
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void StartRegistration()
        {
            System.Console.Clear();
            Console.Write("Username >> ");
            string username = Console.ReadLine();
            Console.Write("Password >> ");
            string password = Console.ReadLine();

            if (ValidInput(username, password))
            {
                Console.WriteLine("New user registration...");
                if (AuthProxy.Registration(username, password))
                {
                    Console.WriteLine("Registration successfully completed");
                }
                else
                {
                    Console.WriteLine("There are already user with username");
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private bool ValidInput(string username, string password)
        {
            if (username.Length == 0 || password.Length == 0)   // za sad neka bude samo ovo
                return false;

            return true;
        }
    }
}