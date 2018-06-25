using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Database_Server
{
    class Toolbox
    {
        public static string[] GetLines(string text)
        {
            text = text.Replace("\r", "");
            return text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetParts(string text)
        {
            return text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] Split(string text, string splitter)
        {
            return text.Split(new string[] { splitter }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
