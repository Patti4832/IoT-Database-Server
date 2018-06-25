/*-----------------------------------------------------------------------------
    This Class is part of "IoT Database Server"
-----------------------------------------------------------------------------*/

//Status: Finished

using System;

namespace IoT_Database_Server
{
    class Log
    {
        private static DateTime dt;

        public static void Info(string text)
        {
            Write("[INFO] " + text);
        }
        public static void Warning(string text)
        {
            Write("[WARNING] " + text);
        }
        public static void Error(string text)
        {
            Write("[ERROR] " + text);
        }
        public static void Debug(string text)
        {
            if (Config.debug)
            {
                Write("[DEBUG] " + text);
            }
        }
        private static void Write(string text)
        {
            dt = DateTime.Now;
            Console.WriteLine("[" + dt.ToShortDateString() + " - " + dt.ToLongTimeString() + "] " + text);
        }
    }
}
