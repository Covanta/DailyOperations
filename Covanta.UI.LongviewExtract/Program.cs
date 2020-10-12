using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;
using Covanta.Entities;
using Covanta.BusinessLogic;
using System.Configuration;
using Covanta.Common.Enums;

namespace Covanta.UI.LongviewExtract
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
                FileProcessor fp = new FileProcessor();
                
                //Get the job details of this application so we can send an updated status of the outcome of this job to the Automated Job Status Emailer application
                _jobDetails = AutomatedJobDetailsManager.GetAutomatedJobDetails(Enums.AutomatedJobNameEnum.LongviewExtract, _connStringCovStaging);
               
                fp.ProcessFile(FileProcessor.enumFileBeingProcessed.LongViewRevenue);
                //fp.ProcessFile(FileProcessor.enumFileBeingProcessed.LongViewTons);
                fp.ProcessFile(FileProcessor.enumFileBeingProcessed.FINLSTAT);
                             

                //fp.sendCompletedEmail();
                AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.LongviewExtract, Enums.AutomatedJobStatusEnum.Success, "Completed Successfully", _connStringCovStaging);
            }
            catch (Exception ex)
            {
                //log exception
                //EmailHelper.SendEmail("Error while processing LongviewExtract. " + ex.Message);
                AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.LongviewExtract, Enums.AutomatedJobStatusEnum.Fail, ex.Message, _connStringCovStaging);
                EmailHelper.SendEmail("collaborationsTeam@covantaenergy.com",_jobDetails.EmailsToBeNotified, "Longview Extract Job Error","Error while processing LongviewExtract. " + ex.Message);
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
