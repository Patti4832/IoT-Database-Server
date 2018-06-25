using System;

namespace IoT_Database_Server
{
    class Program
    {
        static void Main(string[] args)     //port in args[0]
        {
            int port = 80; 
            if(args.Length == 1)
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
