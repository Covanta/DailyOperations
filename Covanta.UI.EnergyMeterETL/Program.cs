using Covanta.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covanta.UI.EnergyMeterETL
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }
                      
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

         //  This email method no longer works.  Maybe new server security settings are in place?

                EmailHelper.SendEmail(_emailFrom, _emailTo, _emailSubject, "Error while processing EnergyMeterETL job. " + ex.Message);
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
