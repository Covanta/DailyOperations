using Covanta.Utilities;
using Covanta.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Covanta.UI.EnergyMeterFTP
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
                    EmailHelper.SendEmail("Error while processing .NET job - EnergyMeterFTP. " + "WinSCP returned a code of: " + _returnCodeFromSFTP);
                }

            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing .NET job - EnergyMeterFTP. " + ex.Message);
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
