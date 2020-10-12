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

namespace Covanta.UI.C4RForecastApprovals
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
            AutomatedJobDetails _jobDetails = new AutomatedJobDetails();

            try
            {
                //Get the job details of this application so we can send an updated status of the outcome of this job to the Automated Job Status Emailer application
                _jobDetails = AutomatedJobDetailsManager.GetAutomatedJobDetails(Enums.AutomatedJobNameEnum.C4RForecastApprovals, _connStringCovStaging);
                
                C4RForecastApprovalParser parser = new C4RForecastApprovalParser();

                parser.ProcessJob();

                AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.C4RForecastApprovals, Enums.AutomatedJobStatusEnum.Success, "Completed Successfully", _connStringCovStaging);

            }
            catch (Exception ex)
            {
                //log exception
             //   EmailHelper.SendEmail("Error while processing C4RForecastApprovalsFromSharePoint. " + ex.Message);
                AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.C4RForecastApprovals, Enums.AutomatedJobStatusEnum.Fail, ex.Message, _connStringCovStaging);
                EmailHelper.SendEmail("collaborationsTeam@covantaenergy.com", _jobDetails.EmailsToBeNotified, "C4RForecastApprovals Job Error", "Error while processing C4RForecastApprovals. " + ex.Message);

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
