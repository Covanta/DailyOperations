using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;
using Covanta.Entities;
using System.Configuration;
using Covanta.BusinessLogic;
using Covanta.Common.Enums;
using Covanta.Utilities;

namespace Covanta.UI.EstepSFTP
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
                
                FileCopierWithWinSCPnew fc = new FileCopierWithWinSCPnew();

                _returnCodeFromSFTP = fc.CopyProcess();

                if (_returnCodeFromSFTP == 0)
                {

                }
                else
                {
                    EmailHelper.SendEmail("Error while processing .NET job - EstepSFTP. " + "WinSCP returned a code of: " + _returnCodeFromSFTP);
                }

            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing .NET job - EstepSFTP. " + ex.Message);
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
