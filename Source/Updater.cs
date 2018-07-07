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
using System.Net;
using System.Threading.Tasks;

namespace Updater
{
    public class Updater
    {
        public int version = 1;

        public int files = 1;
        public bool[] updateAvailable;
        public bool updateable = false;
        public int[] versions;
        public string[] fileNames;
        public string updateURL = "";

        WebClient client = new WebClient();

        public void CheckForUpdate()
        {
            updateAvailable = new bool[versions.Length];

            string updateFile = client.DownloadString(updateURL + "versions.txt");

            int[] fileVersions = new int[4];

            int tmpCount = 0;

            foreach (string s in updateFile.Replace("\n", "").Replace("\r", "").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (versions[tmpCount] < int.Parse(s.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                    updateAvailable[tmpCount] = true;
                else
                    updateAvailable[tmpCount] = false;
                tmpCount++;
            }


            for (int i = 0; i < tmpCount; i++)
            {
                if (updateAvailable[i])
                    updateable = true;
            }
        }

        public void InstallUpdate()
        {
            if (!Directory.Exists("Update"))
                Directory.CreateDirectory("Update");

            for (int i = 0; i < updateAvailable.Length; i++)
            {
                if (updateAvailable[i])
                {
                    try
                    {
                        client.DownloadFile(updateURL + fileNames[i], "Update/" + fileNames[i]); //.Replace(" ", "+")
                    }
                    catch { }
                }
            }

            ExecuteScript(CreateScript());
            Environment.Exit(0);
        }

        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        private string CreateScript()
        {
            string updateFile = "update";
            if (!IsLinux)
                updateFile += ".bat";

            client.DownloadFile(updateURL + updateFile, "Update/" + updateFile);
            while (!File.Exists("Update/" + updateFile)) ;
            return System.Environment.CurrentDirectory + "/Update/" + updateFile;
        }

        private void ExecuteScript(string script)
        {
            while (!File.Exists(script)) ;
            Task t = new Task(
                delegate 
                {
                    System.Environment.CurrentDirectory = "Update";
                    System.Diagnostics.Process.Start(script);
                });
            t.Start();
            t.Wait();
        }
    }
}
