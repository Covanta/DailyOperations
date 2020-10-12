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

namespace Covanta.UI.WorkDayOpUnitsSFTPandLoad
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
            string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
            string _emailTo = ConfigurationManager.AppSettings["EmailTo"];

            try
            {
                //Call SFTP
                _returnCodeFromSFTP = transferWithSFTP(_returnCodeFromSFTP, _emailTo);

                //Load Database
                loadDatabase(_connStringCovStaging);
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing WorkDayOpUnitsSFTPandLoad. " + ex.Message);
            }          
        }

        private static void loadDatabase(string _conString)
        {
            WorkdayOperatingUnitsManager manager = new WorkdayOperatingUnitsManager(_conString);
            manager.ProcessFiles();
        }

        private static int transferWithSFTP(int _returnCodeFromSFTP, string _emailTo)
        {
            try
            {
                Covanta.Utilities.FileCopierWithWinSCPnew fc = new Utilities.FileCopierWithWinSCPnew();

                _returnCodeFromSFTP = fc.CopyProcess();

                if (_returnCodeFromSFTP == 0)
                {
                    //success
                }
                else
                {
                    //fail
                    EmailHelper.SendEmail("collaborationsTeam@covantaenergy.com", _emailTo, "WorkDaySFTP OPs Units Extract Job Error", "WinSCP returned a code of: " + _returnCodeFromSFTP);
                }

            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("collaborationsTeam@covantaenergy.com", _emailTo, "WorkDaySFTP OPs Units Extract Job Error", "Error while processing WorkDaySFTP. " + ex.Message);
            }
            return _returnCodeFromSFTP;
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
