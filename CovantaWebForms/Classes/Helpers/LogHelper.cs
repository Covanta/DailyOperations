using System;
using System.Configuration;
using System.IO;

namespace CovantaWebForms.Classes.Helpers
{
    public class LogHelper
    {
        private static string _filename;
        private static string _logDirectoryPath;
        private static string LogDirectoryPath
        {
            get
            {
                _filename = String.Format("Log__{0:yyyy-MM-dd}.txt", DateTime.Now);
                _logDirectoryPath = ConfigurationManager.AppSettings["LogDirectoryPath"] + _filename;
                if (!File.Exists(_logDirectoryPath))
                {
                    File.CreateText(_logDirectoryPath).Dispose();
                }
                return _logDirectoryPath;
            }
        }
        
        /// <summary>
        /// Log Error/Exception to log.txt file.
        /// </summary>
        /// <param name="ex"></param>
        public static void ErrorLogging(Exception ex)
        {
            using (StreamWriter sw = File.AppendText(LogDirectoryPath))
            {
                sw.WriteLine(DateTime.Now + ":  ");
                sw.WriteLine("Error Message: " + ex.Message);
                sw.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Log info to log.txt file.
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(string message)
        {
            using (StreamWriter sw = File.AppendText(LogDirectoryPath))
            {
                sw.WriteLine(DateTime.Now + ":  ");
                sw.WriteLine("INFO: " + message);
            }
        }
    }
}