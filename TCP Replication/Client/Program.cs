using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    class Program
    {
        //Memeriksa apakah ada pesan yang masuk
        public class CheckMessage
        {

            public void Check(TcpClient tcpClient)
            {
                StreamReader streamReader = new StreamReader(tcpClient.GetStream());

                //Pemeriksaan dilakukan secara - terus menerus
                //Ketika program dijalankan
                while (true)
                {
                    try
                    {
                        string message = streamReader.ReadLine();
                        Console.WriteLine(message);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        break;
                    }
                }
            }
        }

        //Mencatat pesan yang masuk pada fungsi main
        static void Main(string[] args)
        {
            CheckMessage read = new CheckMessage();
            
            //Ketika ada data yang masuk
            //Maka data dibaca, dan ditampilkan
            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 1000);
                Console.WriteLine("Connect to The Server.");
                Console.WriteLine("1.Login\n2.Play\n");
                Console.WriteLine("Type here : ");

                Thread thread = new Thread(() => read.Check(tcpClient));
                thread.Start();

                StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream());
                string word = Console.ReadLine();

                while (true)
                {
                    if (tcpClient.Connected)
                    {
                       
                        Console.WriteLine(word);

                        string username = Console.ReadLine();
                        string code = Console.ReadLine();
                        string input = Console.ReadLine();

                        if (word == "login")
                        {                           
                            Console.WriteLine("Enter Username : ");
                            streamWriter.WriteLine(username + " : " + input);
                            streamWriter.Flush();
                        }

                        else if (word == "play")
                        {
                            Console.WriteLine("Enter Code : ");
                            streamWriter.WriteLine(code + " : " + input);
                            streamWriter.Flush();
                        }
                        else
                        {
                            Console.WriteLine("You wrote the wrong word!!!");
                            streamWriter.Flush();
                        }
                        
                    }
                }

            }

            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.ReadKey();

        }
    }
}