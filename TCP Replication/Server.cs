using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace server
{
    public class login
    {
        Socket s;

        public login(Socket valueS)
        {
            s = valueS;
        }

        public void check()
        {
            try
            {

                byte[] uname = new byte[100];
                byte[] pword = new byte[100];
                int sizeuser = s.Receive(uname);
                int sizepword = s.Receive(pword);

                if (sizeuser > 0 && sizepword > 0)
                {
                    Console.WriteLine("Recieved");
                    TextReader objTxtReader = new StreamReader("login.txt");
                    String username = objTxtReader.ReadLine();
                    String password = objTxtReader.ReadLine();
                    String test = ASCIIEncoding.ASCII.GetString(uname);
                    test = test.Substring(0, sizeuser);
                    if (username == (ASCIIEncoding.ASCII.GetString(uname)).Substring(0, sizeuser) && password == (ASCIIEncoding.ASCII.GetString(pword)).Substring(0, sizepword))
                    {

                    }
                    else
                    {
                        ASCIIEncoding asen2 = new ASCIIEncoding();
                        s.Send(asen2.GetBytes("0"));
                    }

                }

            }
            catch (Exception e) { }

        }

    }


    public class Program
    {

        static void Main(string[] args)
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse("192.168.10.68");
                TcpListener myList = new TcpListener(ipAd, 8002);
                myList.Start();
                Random randDouble = new Random();
                double randValue;
                Socket s = myList.AcceptSocket();
                login objLogin = new login(s);
                Thread thdLogin = new Thread(new ThreadStart(objLogin.check));
                thdLogin.Start();
                thdLogin.Join();
                while (true)
                {
                    randValue = randDouble.NextDouble();
                    Console.WriteLine(randValue);
                    Thread.Sleep(500);
                    ASCIIEncoding asen = new ASCIIEncoding();
                    String strRandValue = randValue.ToString();
                    byte[] msg = asen.GetBytes(strRandValue);
                    s.Send(msg);
                }
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}