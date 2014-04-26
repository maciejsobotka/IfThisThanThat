using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ITTT_Final
{
    class Logs
    {
        private static StreamWriter logFile;
        private static DateTime time;
        public static void WriteLog(string nfo)
        {
            time = DateTime.Now;
            logFile = File.AppendText("LOG.txt");
            logFile.WriteLine('[' + time.ToLongTimeString() + ' ' + time.ToShortDateString() + "] " + nfo);
            logFile.WriteLine("--------------------------------------------------------------");
            logFile.Close();
        }
        public static void Info(string nfo)
        {
            WriteLog("Info: " + nfo);
        }
        public static void Error(string err)
        {
            WriteLog("Error: " + err);
        }
    }
}
