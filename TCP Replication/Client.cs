using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");
                String username = null;
                String password = null;

                tcpclnt.Connect("192.168.10.68", 8002);
                Console.WriteLine("Connected");
                Console.WriteLine("Enter Username");
                username = Console.ReadLine();
                Console.WriteLine("Enter Password");
                // password =  Console.ReadLine();
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Substring(0, (password.Length - 1));
                            Console.Write("\b \b");
                        }
                    }
                } while (key.Key != ConsoleKey.Enter);

                Console.Write("\n");
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding uname = new ASCIIEncoding();
                byte[] byteUsername = uname.GetBytes(username);
                ASCIIEncoding pword = new ASCIIEncoding();
                byte[] bytePword = uname.GetBytes(password);
                stm.Write(byteUsername, 0, byteUsername.Length);
                stm.Write(bytePword, 0, bytePword.Length);

                byte[] byteMsg = new byte[100];
                int length = stm.Read(byteMsg, 0, 100);
                while (length > 1)
                {
                    for (int i = 0; i < length; i++)
                        Console.Write(Convert.ToChar(byteMsg[i]));
                    Console.WriteLine("\n");
                    length = stm.Read(byteMsg, 0, 100);
                }

                if (length == 1)
                {
                    Console.WriteLine("Invalid username or password");
                }
                tcpclnt.Close();
                String str1 = Console.ReadLine();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
