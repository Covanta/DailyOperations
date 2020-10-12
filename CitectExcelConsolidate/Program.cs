using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;
using System.Configuration;
using Covanta.BusinessLogic;



namespace CitectExcelConsolidate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {                
                return;
            }

            try
            {
                Processor process = new Processor();
                process.processFiles();
            }
            catch (Exception ex)
            {
                //log exception
                string EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                string EmailTo = ConfigurationManager.AppSettings["EmailTo"];
                string EmailSubject = ConfigurationManager.AppSettings["EmailSubject"];
                EmailHelper.SendEmail(EmailFrom, EmailTo, EmailSubject,  "Error while processing CitectExcelConsolidate. " + ex.Message);
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
