/*
MIT License
Copyright (c) 2018 Patti4832
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
            string[] lines = content.Replace("\r", "").Split('\n');
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
            lines = content.Replace("\r", "").Split('\n');
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
