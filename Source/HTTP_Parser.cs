/*-----------------------------------------------------------------------------
    This Class is part of "IoT Database Server"
-----------------------------------------------------------------------------*/

//Status: Finished

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Database_Server
{
    class HTTP_Parser
    {
        public static string[] GetParams(string request)
        {
            string[] paramList = new string[5]; //user, key, action, variable, content
            for(int i = 0; i < 5; i++)
            {
                paramList[i] = "";
            }

            string line_one = Toolbox.GetLines(request)[0];

            string paramsPart = Toolbox.GetParts(line_one)[1];

            if (paramsPart.StartsWith("/http"))
            {
                paramsPart = Toolbox.Split(paramsPart, "?")[1];
                foreach(string part in Toolbox.Split(paramsPart, "&"))
                {
                    if (part.StartsWith("u=")) //User
                    {
                        paramList[0] = part.Replace("u=", "");
                    }
                    if (part.StartsWith("k=")) //Key
                    {
                        paramList[1] = part.Replace("k=", "");
                    }
                    if (part.StartsWith("a=")) //Action
                    {
                        paramList[2] = part.Replace("a=", "");
                    }
                    if (part.StartsWith("v=")) //Variable
                    {
                        paramList[3] = part.Replace("v=", "");
                    }
                    if (part.StartsWith("c=")) //Content
                    {
                        paramList[4] = part.Replace("c=", "");
                    }
                }
            }

            return paramList;
        }
        public static string CreateAnswer(string content)
        {
            CultureInfo culture = new CultureInfo("en-US");
            string date =
                culture.DateTimeFormat.GetAbbreviatedDayName(DateTime.Now.DayOfWeek) + ", " +
                DateTime.Now.Day + " " + culture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month) + " " +
                DateTime.Now.Year + " " +
                DateTime.Now.ToUniversalTime().Hour + ":" + DateTime.Now.ToUniversalTime().Minute + ":" + DateTime.Now.ToUniversalTime().Second + " GMT";
            string ret =
               "HTTP/1.1 200 OK\n" +
               "Date: " + date + "\n" +
               "Server: IoT-DB-Server\n" +
               "Content-Length: " + content.Length + "\n" +
               "Content-Type: text/html\n\n" +
               content + "";
            return ret;
        }
    }
}
