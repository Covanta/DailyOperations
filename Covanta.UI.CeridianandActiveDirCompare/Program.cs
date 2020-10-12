using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.CeridianandActiveDirCompare
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
                CeridianAndActiveDirDifferencesParser parser = new CeridianAndActiveDirDifferencesParser();
                parser.ProcessJob();                
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing CeridianandActiveDirCompare. " + ex.Message);
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
