using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;
using System.Configuration;



namespace Covanta.UI.ShareFileFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }
            int _returnCodeFromSFTP = 0;
     

            try
            {
                WINSCP_Helper fc = new WINSCP_Helper();               
                
                _returnCodeFromSFTP = fc.CopyProcess();
                               
                if (_returnCodeFromSFTP == 0)
                {
                }
                else
                {
                    throw new Exception("ShareFileFTP .net application returned a return code of " + _returnCodeFromSFTP + " A return code of 0 wold have been a normal result");
                }

            }
            catch (Exception ex)
            {
                //log exception
                string EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                string EmailTo = ConfigurationManager.AppSettings["EmailTo"];
                string EmailSubject = ConfigurationManager.AppSettings["EmailSubject"];
                EmailHelper.SendEmail(EmailFrom, EmailTo, EmailSubject, "Error while processing CitectExcelConsolidate. " + ex.Message);
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
