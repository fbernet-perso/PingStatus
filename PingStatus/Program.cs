using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace PingStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1;
            using (Ping myPing = new Ping())
            {
                while (true)
                {
                    try
                    {
                        PingReply reply = myPing.Send("1.1.1.1", 1000);
                        if (reply?.Status != IPStatus.Success)
                        {
                            string errorMessage = DateTime.Now.ToLongTimeString() + "\t" + reply?.Status;
                            Console.Error.WriteLine(errorMessage);
                            if (args.Length > 0)
                            {
                                File.AppendAllLines(args[0], new List<string> { errorMessage });
                            }
                        }
                        else
                        {
                            if (i == 50)
                            {
                                Console.WriteLine(DateTime.Now.ToLongTimeString() + "\tOK");
                                i = 0;
                            }
                            else
                            {
                                Console.Write(".");
                            }
                            Thread.Sleep(500);
                        }
                        i++;
                    }
                    catch (Exception ex)
                    {
                        i = 0;
                        string errorMessage = DateTime.Now.ToLongTimeString() + "\t" + ex.ToString();
                        Console.Error.WriteLine(errorMessage);
                        if (args.Length > 0)
                        {
                            File.AppendAllLines(args[0], new List<string> { errorMessage });
                        }
                    }
                }
            }
        }
    }
}
