/*-----------------------------------------------------------------------------
    This Class is part of "IoT Database Server"
-----------------------------------------------------------------------------*/

//Status: Finished

using System.IO;

namespace IoT_Database_Server
{
    class Database
    {
        public static void Init()
        {
            if (File.Exists(Config.config))
            {
                Config.LoadConfig();
            }

            if (!Directory.Exists(Config.db_files))
            {
                Directory.CreateDirectory(Config.db_files);
            }
            if(!File.Exists(Config.db_files + "anonymous.var"))
            {
                var f = File.Create(Config.db_files + "anonymous.var");
                f.Close();
            }
            if (!File.Exists(Config.db_file_users) || !File.Exists(Config.db_file_keys))
            {
                Log.Warning("UserFile or KeyFile not found!");
            }
            else
            {
                Security.Init();
            }
        }
        public static string Process(string[] args) //user, key, action, variable, content
        {
            string ret = "error";

            string
                u = args[0],
                k = args[1],
                a = args[2],
                v = args[3],
                c = args[4];

            if (a == "" || !( a == "r" || a == "n" || a == "e" || a == "d" || a == "l"))
            {
                Log.Warning("No action selected!");
            }
            else if (!Security.VariableAllowed(v) || !Security.VariableAllowed(c))
            {
                Log.Warning("Variable or content not allowed!");
            }
            else
            {
                string varfile = "";

                if (u == "" && k == "")  //anonymous login (readonly)
                {
                    Log.Info("Anonymous login");
                    varfile = Security.GetVarfile();
                    if (a == "r")
                    {
                        ret = DB_Read(varfile, v);
                    }
                    else if (a == "l")
                    {
                        ret = DB_List(varfile);
                    }
                    else
                    {
                        Log.Warning("Anonymous user selected unknown or forbidden action.");
                        ret = "Unknown or forbidden action!";
                    }
                }
                else
                {
                    varfile = Security.GetVarfile(u, k);
                    if (varfile != "") {
                        if (a == "l")
                        {
                            ret = DB_List(varfile);
                        }
                        else if (a == "r")
                        {
                            ret = DB_Read(varfile, v);
                        }
                        else if (a == "n")
                        {
                            ret = DB_New(varfile, v, c);
                        }
                        else if (a == "e")
                        {
                            ret = DB_Edit(varfile, v, c);
                        }
                        else if (a == "d")
                        {
                            ret = DB_Delete(varfile, v);
                        }
                        else
                        {
                            Log.Warning("Unkonwn action!");
                            ret = "Unknown action!";
                        }
                    }
                    else
                    {
                        Log.Warning("Login failed!");
                        ret = "Login failed!";
                    }
                }
            }
            return ret;
        }

        private static string DB_Read(string varfile, string var)
        {
            string ret = "not found!";
            string[] content = File.ReadAllLines(Config.db_files + varfile);
            foreach(string line in content)
            {
                if (line.StartsWith(var + "="))
                {
                    ret = line.Replace(var+"=", "");
                }
            }
            return ret;
        }
        private static string DB_List(string varfile)
        {
            string ret = "";
            string[] content = File.ReadAllLines(Config.db_files + varfile);
            foreach (string line in content)
            {
                ret += Toolbox.Split(line, "=")[0] + ", ";
            }
            if(ret == "")
            {
                ret = "no variables found!";
            }
            return ret;
        }
        private static string DB_New(string varfile, string var, string c)
        {
            string ret = "variable already exists!";
            bool inList = false;
            string[] content = new string[] { "" };
            try
            {
                content = File.ReadAllLines(Config.db_files + varfile);
            }
            catch
            {
                Log.Warning("Can't access file '" + Config.db_files + varfile + "'");
            }
            string newcontent = "";
            foreach (string line in content)
            {
                newcontent += line + "\n";
                if (line.StartsWith(var + "="))
                    inList = true;
            }
            if (!inList)
            {
                newcontent += var + "=" + c + "\n";
                File.Delete(Config.db_files + varfile);
                File.WriteAllText(Config.db_files + varfile, newcontent);
                ret = "Created successful";
            }
            return ret;
        }
        private static string DB_Edit(string varfile, string var, string c)
        {
            string ret = "variable not found!";
            bool inList = false;
            string[] content = new string[] { "" };
            try
            {
                content = File.ReadAllLines(Config.db_files + varfile);
            }
            catch
            {
                Log.Warning("Can't access file '" + Config.db_files + varfile + "'");
            }
            string newcontent = "";
            foreach (string line in content)
            {
                if (line.StartsWith(var + "="))
                {
                    inList = true;
                    newcontent += var + "=" + c + "\n";
                }
                else
                    newcontent += line + "\n";
            }
            if (inList)
            {
                File.Delete(Config.db_files + varfile);
                File.WriteAllText(Config.db_files + varfile, newcontent);
                ret = "edited successful";
            }
            return ret;
        }
        private static string DB_Delete(string varfile, string var)
        {
            string ret = "variable not found!";
            bool inList = false;
            string[] content = new string[] { "" };
            try
            {
                content = File.ReadAllLines(Config.db_files + varfile);
            }
            catch
            {
                Log.Warning("Can't access file '" + Config.db_files + varfile + "'");
            }
            string newcontent = "";
            foreach (string line in content)
            {
                if (line.StartsWith(var + "="))
                {
                    inList = true;
                }
                else
                    newcontent += line + "\n";
            }
            if (inList)
            {
                File.Delete(Config.db_files + varfile);
                File.WriteAllText(Config.db_files + varfile, newcontent);
                ret = "deleted successful";
            }
            return ret;
        }
    }
}
