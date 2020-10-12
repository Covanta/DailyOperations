using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.EnvironmentalEmissionsETL
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }

            string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
            string _emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string _emailSubject = ConfigurationManager.AppSettings["EmailSubject"];
            string _emailTo = ConfigurationManager.AppSettings["EmailTo"];

            try
            {
                Processor process = new Processor();
                process.processFiles();
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail(_emailFrom, _emailTo, _emailSubject, "Error while processing EnvironmentalEmissionsETL job. " + ex.Message);
                throw ex;
            }
        }

        private static bool isAppAlreadyRunning()
        {
            Process[] appProc;
            string strModName;
            string strProcName;
            strModName = Process.GetCurrentProcess().MainModule.ModuleName;
            strProcName = System.IO.Path.GetFileNameWithoutExtension(strModName);
            appProc = Process.GetProcessesByName(strProcName);
            if (appProc.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
