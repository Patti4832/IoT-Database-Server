using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Database_Server
{
    class Program
    {
        static void Main(string[] args)     //port in args[0]
        {
            Updater.Updater u = new Updater.Updater();
            u.updateURL = "https://raw.githubusercontent.com/Patti4832/IoT-Database-Server/master/Update/";
            u.files = 1;
            u.fileNames = new string[] { "IoT Database Server.exe" };
            u.versions = new int[] { 2 };
            u.CheckForUpdate();
            if (u.updateable)
                u.InstallUpdate();
            else
            {
                int port = 80;
                if (args.Length == 1)
                {
                    try
                    {
                        port = int.Parse(args[0]);
                    }
                    catch { }
                }

                Log.Info("Starting IoT Databse Server ...");

                Database.Init();

                Config.debug = true;
                Server s1 = new Server(port);
                s1.Start();

                Console.ReadLine();
            }
        }
    }
}
