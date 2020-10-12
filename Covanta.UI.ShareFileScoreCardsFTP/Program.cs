using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.ShareFileScoreCardsFTP
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
                    throw new Exception("ShareFileScoreCardsFTP .net application returned a return code of " + _returnCodeFromSFTP + " A return code of 0 wold have been a normal result");
                }

            }
            catch (Exception ex)
            {
                //log exception
                string EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                string EmailTo = ConfigurationManager.AppSettings["EmailTo"];
                string EmailSubject = ConfigurationManager.AppSettings["EmailSubject"];
                EmailHelper.SendEmail(EmailFrom, EmailTo, EmailSubject, "Error while processing ShareFileScoreCardsFTP. " + ex.Message);
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
