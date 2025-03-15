using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestChat
{
    static class BaseDataAccount
    {
        static private List<Account> listAccounts { get; set; }

        static BaseDataAccount()
        {
            listAccounts = new List<Account>();
        }

        static public void AddAccount(Account addAccount)
        {
            listAccounts.Add(addAccount);
        }

        static public void WriteListAccounts()
        {
            foreach (Account accounts in listAccounts)
            {
                Console.WriteLine($"{accounts.nameUser}");
            }
        }

        static public Account CheckedOnEnter(string nameUser, string passwordUser)
        {
            for (int i = 0; i < listAccounts.Count; i++)
            {
                if (nameUser == listAccounts[i].nameUser)
                {
                    if (passwordUser == listAccounts[i].GetPassword())
                    {
                        Console.WriteLine("You welcome!");
                        return listAccounts[i];
                    }
                }
            }
            Console.WriteLine("Password or name not correct");
            return null;
        }

        static public bool CheckNameAccount(string nameAccount)
        {
            foreach (Account name in listAccounts)
            {
                if (name.nameUser == nameAccount)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }

        static public Account GetAccount(string nameAccount)
        {
            foreach (Account name in listAccounts)
            {
                if (name.nameUser == nameAccount)
                {
                    return name;
                }
                else
                {
                    continue;
                }
            }
            return null;
        }
    }

    class Account
    {
        public string nameUser { get; set; }

        private string passwordUser { get; set; }

        public Mail mailUser { get; set; }

        private Account(string name, string password)
        {
            nameUser = name;
            passwordUser = password;

            mailUser = new Mail();
            
            BaseDataAccount.AddAccount(this);
        }

        public static Account CreateAccount()
        {
            Console.Write("Create your name: ");
            string name = Console.ReadLine();

            Console.Write("Create your password: ");
            string password = Console.ReadLine();

            return new Account(name, password);
        }

        public string GetPassword()
        {
            return passwordUser;
        }
    }

    class Mail
    {
        private List<string> message { get; set; }
        private List<bool> messageRead { get; set; }

        public Mail()
        {
            message = new List<string>();
            messageRead = new List<bool>();
        }

        private void SendMessage(Account sendToUser, string sendMessage)
        {
            sendToUser.mailUser.AddMessage(sendToUser, sendMessage);
        }

        private void AddMessage(Account user, string message)
        {
            user.mailUser.message.Add(message);
            user.mailUser.messageRead.Add(false);
        }

        public string CheckUnreadMessage()
        {
            int unreadMessage = 0;

            foreach (bool list in messageRead)
            {
                if (list)
                {
                    continue;
                }
                else
                {
                    unreadMessage++;
                }
            }
            if (unreadMessage > 0)
            {
                return $"You have unread {unreadMessage} messages!";
            }
            else
            {
                return "You haven't unread message!";
            }
        }

        public void ReadingUnreadMessage()
        {
            int indexReadingMessage = 0;
            for (int i = 0; i < messageRead.Count; i++)
            {
                if (messageRead[i])
                {
                    continue;
                }
                else
                {
                    Console.WriteLine($"{i}. {message[i]}");
                    indexReadingMessage++;
                    continue;
                }
            }
            if (indexReadingMessage == 0)
            {
                Console.WriteLine("You haven't unread message!");
            }
        }

        public void ReadingAll()
        {
            for (int i = 0; i < messageRead.Count; i++)
            {
                if (!messageRead[i])
                {
                    messageRead[i] = !messageRead[i];
                }
            }
            Console.Clear();
            Console.WriteLine("All message is reading!");
            Console.ReadKey(true);
        }

        public void SendMessage(Account sender)
        {
            Console.Write("Write name for who send message: ");
            string name = Console.ReadLine();

            Console.Write("Write your message: ");
            string message = Console.ReadLine();
            
            if (BaseDataAccount.CheckNameAccount(name))
            {
                sender.mailUser.SendMessage(BaseDataAccount.GetAccount(name), message);
            }
        }



        public void MethodForTest()
        {
            for (int i = 0; i < messageRead.Count; i++)
            {
                Console.WriteLine(messageRead[i]);
            }
        }
    }

    class App
    {
        public void Start()
        {
            Console.WriteLine("Application Start!\n");
            StartApp();
        }

        private static void StartApp()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("What do you want?" +
                "\n1. Enter in account" +
                "\n2. Create a new account" +
                "\n3. Exit program");

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.Write("Write your Login: ");
                        string name = Console.ReadLine();

                        Console.Write("Write your password: ");
                        string password = Console.ReadLine();

                        Account account = BaseDataAccount.CheckedOnEnter(name, password);
                        if (account != null)
                        {
                            ChangeAccount(account);
                        }
                        break;
                    case ConsoleKey.D2:
                        Account newAccount = Account.CreateAccount();
                        ChangeAccount(newAccount);
                        break;
                    case ConsoleKey.D3:
                        isRunning = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ChangeAccount(Account account)
        {
            bool isLoggedIn = true;

            while (isLoggedIn)
            {
                Console.Clear();

                string countReadingMessage = account.mailUser.CheckUnreadMessage();

                Console.WriteLine("You have ... action" +
                    $"\n1. Your Mail: {countReadingMessage}" +
                    $"\n2. Exit from account" +
                    $"\n3. Send message");

                account.mailUser.MethodForTest();

                ConsoleKeyInfo keyInAccount = Console.ReadKey(true);

                switch (keyInAccount.Key)
                {
                    case ConsoleKey.D1:
                        account.mailUser.ReadingUnreadMessage();

                        Console.WriteLine("\n1. Reading All (n)" +
                            "\n2. Back (esc)");
                        ConsoleKeyInfo keyInAction = Console.ReadKey(true);

                        switch (keyInAction.Key)
                        {
                            case ConsoleKey.Escape:
                                continue;
                            case ConsoleKey.N:
                                account.mailUser.ReadingAll();
                                break;
                            default:
                                break;
                        }
                        break;
                    case ConsoleKey.D2:
                        isLoggedIn = false;
                        break;
                    case ConsoleKey.D3:
                        account.mailUser.SendMessage(account);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*App app = new App();

            app.Start();*/

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i + 1);
            }
        }
    }
}
