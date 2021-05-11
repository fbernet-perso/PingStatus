using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace PingStatus
{
    class Program
    {
        static string outputFile = @".\ping.txt";

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                outputFile = args[0];
            }
            int i = 1;
            using (Ping myPing = new Ping())
            {
                while (true)
                {
                    i++;
                    try
                    {
                        PingReply reply = myPing.Send("1.1.1.1", 1000);
                        if (reply?.Status != IPStatus.Success)
                        {
                            LogError(FormatOutput(reply?.Status.ToString()));
                            i = 1;
                        }
                        else
                        {
                            if (i >= 50)
                            {
                                Console.WriteLine(FormatOutput("OK"));
                                i = 1;
                            }
                            else
                            {
                                Console.Write(".");
                            }
                            Thread.Sleep(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        i = 1;
                        LogError(FormatOutput(ex.ToString()));
                    }
                }
            }
        }

        private static void LogError(string errorMessage)
        {
            Console.Error.WriteLine(errorMessage);
            File.AppendAllLines(outputFile, new List<string> { errorMessage });
        }

        private static string FormatOutput(string message)
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + message;
        }
    }
}
