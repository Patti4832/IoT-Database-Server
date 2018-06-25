/*-----------------------------------------------------------------------------
    This Class is part of "IoT Database Server"
-----------------------------------------------------------------------------*/

//Status: Finished

using System;
using System.IO;

namespace IoT_Database_Server
{
    class Security
    {
        private static string[] users, keys;
        public static void Init()
        {
            string content = File.ReadAllText(Config.db_file_users);
            int counter = 0;
            string[] lines = content.Replace("\r", "").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            users = new string[lines.Length];
            foreach (string line in lines)
            {
                users[counter] = line;
                Log.Info("Loaded user '" + line + "'");

                if (!File.Exists(Config.db_files + line + ".var"))
                {
                    var f = File.Create(Config.db_files + line + ".var");
                    f.Close();
                }

                counter++;
            }

            content = File.ReadAllText(Config.db_file_keys);
            counter = 0;
            lines = content.Replace("\r", "").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            keys = new string[lines.Length];
            foreach (string line in lines)
            {
                keys[counter] = line;
                counter++;
            }

            Log.Info("Security init complete.");
        }
        public static string GetVarfile(string user = "", string key = "")
        {
            string varfile = "anonymous.var";
            bool isRegistred = false;
            bool pwRight = false;

            if(user != "" && key != "")
            {
                for(int i = 0; i < users.Length; i++)
                {
                    if(users[i] == user)
                    {
                        isRegistred = true;
                        if (keys[i] == key)
                        {
                            varfile = user + ".var";
                            pwRight = true;
                        }
                    }
                }
            }
            else
            {
                isRegistred = true;
            }

            
            if (!isRegistred)
            {
                varfile = "";
                Log.Warning("User '" + user + "' not registred!");
            }
            else if (!pwRight && user != "")
            {
                varfile = "";
                Log.Warning("User '" + user + "' used wrong key!");
            }

            return varfile;
        }
        public static bool VariableAllowed(string v)
        {
            bool ret = true;

            if (v.Contains("=") || v.Contains("%") || v.Contains(" "))
                ret = false;

            return ret;
        }
    }
}
