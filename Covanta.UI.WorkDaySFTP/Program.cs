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

namespace Covanta.UI.WorkDaySFTP
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
            AutomatedJobDetails _jobDetails = new AutomatedJobDetails();

            try
            {
                //Get the job details of this application so we can send an updated status of the outcome of this job to the Automated Job Status Emailer application
                _jobDetails = AutomatedJobDetailsManager.GetAutomatedJobDetails(Enums.AutomatedJobNameEnum.WorkDaySFTP , _connStringCovStaging);

                FileCopierWithWinSCP fc = new FileCopierWithWinSCP();
                
                _returnCodeFromSFTP = fc.CopyProcess();
                               
                if (_returnCodeFromSFTP == 0)
                {
                    //success
                    AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.WorkDaySFTP, Enums.AutomatedJobStatusEnum.Success, "Completed Successfully", _connStringCovStaging);
                }
                else
                {
                    //fail
                    AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.WorkDaySFTP, Enums.AutomatedJobStatusEnum.Fail, "WinSCP returned a code of: " + _returnCodeFromSFTP, _connStringCovStaging);
                    EmailHelper.SendEmail("collaborationsTeam@covantaenergy.com", _jobDetails.EmailsToBeNotified, "WorkDaySFTP Extract Job Error", "WinSCP returned a code of: " + _returnCodeFromSFTP);
                }

            }
            catch (Exception ex)
            {
                //log exception
               // EmailHelper.SendEmail("Error while processing .NET job - WorkDaySFTP. " + ex.Message);
                AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.WorkDaySFTP, Enums.AutomatedJobStatusEnum.Fail, ex.Message, _connStringCovStaging);
                EmailHelper.SendEmail("collaborationsTeam@covantaenergy.com", _jobDetails.EmailsToBeNotified, "WorkDaySFTP Extract Job Error", "Error while processing WorkDaySFTP. " + ex.Message);
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
