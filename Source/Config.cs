/*-----------------------------------------------------------------------------
    This Class is part of "IoT Database Server"
-----------------------------------------------------------------------------*/

//Status: Incomplete

using System.IO;

namespace IoT_Database_Server
{
    class Config
    {
        public static string db_files = "IoT-DB/VAR/";
        public static string db_file_users = "IoT-DB/users.txt";
        public static string db_file_keys = "IoT-DB/keys.txt";
        public static string db_file_log = "IoT-DB/log.txt";
        public static string config = "config.txt";
        public static bool debug = false;

        public static void LoadConfig(string configfile = "config.txt")
        {
            string[] filecontent = File.ReadAllLines(configfile);
            
        }
    }
}
