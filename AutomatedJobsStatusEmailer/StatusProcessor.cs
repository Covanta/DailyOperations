using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using Covanta.BusinessLogic;
using Covanta.Entities;
using Covanta.Utilities.Helpers;
using Covanta.Common.Enums;

namespace Covanta.UI.AutomatedJobsStatusEmailer
{
    public class StatusProcessor
    {
        #region private variables

        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
        List<AutomatedJobDetails> _list;
        List<string> _successfulJobsList = new List<string>();
        List<string> _failedJobsList = new List<string>();
        List<string> _jobsWhichDidnotRunList = new List<string>();
        string _emailsToBeNotified = string.Empty;
        string _thisJobName = "AutomatedJobStatusEmailer";
        string _body = string.Empty;

        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public StatusProcessor() { }

        #endregion

        #region public methods

        public void Process()
        {
            //Get the list of jobs in the system
            _list = AutomatedJobDetailsManager.GetAutomatedJobDetailsList(_connStringCovStaging);
            populateStatusLists();
            buildEmail();
            EmailHelper.SendEmailNotHTML("collaborationsTeam@covantaenergy.com", _emailsToBeNotified, "Automated Job Status Emailer Results", _body);
            resetStatuses();
        }       

        #endregion

        #region private methods

        private void populateStatusLists()
        {
            foreach (AutomatedJobDetails details in _list)
            {
                if (details.Status == "Success")
                {
                    _successfulJobsList.Add(details.Job);
                }
                else
                    if (details.Status == "Fail")
                    {
                        _failedJobsList.Add(details.Job);
                    }
                    else
                        if (details.Status == "Reset")
                        {
                            _jobsWhichDidnotRunList.Add(details.Job);
                        }
                        else
                        {
                            _failedJobsList.Add(details.Job);
                        }
            }
            _successfulJobsList.Sort();
            _failedJobsList.Sort();
            _jobsWhichDidnotRunList.Sort();

        }

        private void resetStatuses()
        {
            foreach (AutomatedJobDetails detail in _list)
            {
                Enums.AutomatedJobNameEnum appName = Enums.AutomatedJobNameEnum.None;
                if (detail.ApplicationName == "AutomatedJobStatusEmailer") { appName = Enums.AutomatedJobNameEnum.AutomatedJobStatusEmailer; }
                if (detail.ApplicationName == "C4RForecastApprovals") { appName = Enums.AutomatedJobNameEnum.C4RForecastApprovals; }
                if (detail.ApplicationName == "LongviewExtract") { appName = Enums.AutomatedJobNameEnum.LongviewExtract; }
                if (detail.ApplicationName == "WorkDaySFTP") { appName = Enums.AutomatedJobNameEnum.WorkDaySFTP; }
                if (detail.ApplicationName == "SSIS_WorkDayExtract") { appName = Enums.AutomatedJobNameEnum.SSIS_WorkDayExtract; }
                if (detail.ApplicationName == "SSIS_WorkDayPushToCSV") { appName = Enums.AutomatedJobNameEnum.SSIS_WorkDayPushToCSV; }
                if (detail.ApplicationName == "SSIS_LoadDBFromADirCSV") { appName = Enums.AutomatedJobNameEnum.SSIS_LoadDBFromADirCSV; }

                AutomatedJobDetailsManager.UpdateJobStatus(appName, Enums.AutomatedJobStatusEnum.Reset, "Reset", _connStringCovStaging);
            }
        }      

        private void buildEmail()
        {
            _emailsToBeNotified = string.Empty;

            if (_list.Exists(x => x.Job == _thisJobName))
            {
                _emailsToBeNotified = _list.Find(x => x.Job == _thisJobName).EmailsToBeNotified;
            }
            else
            {
                _emailsToBeNotified = @"nobody@covantaenergy.com";
            }
            buildEmailStatusText();
        }

        private void buildEmailStatusText()
        {
            StringBuilder sb = new StringBuilder();
            //successful jobs
            sb.AppendLine("*************************************************************************************");
            sb.AppendLine("The following jobs were Successful. ");
            sb.AppendLine("*************************************************************************************");
            if (_successfulJobsList.Count == 0)
            {
                sb.AppendLine("0 Successful");
                sb.AppendLine();
            }
            else
            {
                foreach (string item in _successfulJobsList)
                {
                    sb.AppendLine(item);
                }
            }
            sb.AppendLine();
            sb.AppendLine("*************************************************************************************");
            //failed jobs
            sb.AppendLine("The following jobs failed. ");
            sb.AppendLine("*************************************************************************************");
            if (_failedJobsList.Count == 0)
            {
                sb.AppendLine("0 Failed");
                sb.AppendLine();
            }
            else
            {
                foreach (string item in _failedJobsList)
                {
                    sb.AppendLine(item);
                    string reason = _list.Find(x => x.Job == item).StatusDescription;
                    sb.AppendLine(reason);
                    sb.AppendLine();
                }
            }

            sb.AppendLine();
            sb.AppendLine("*************************************************************************************");
            //not run jobs
            sb.AppendLine("The following jobs did not run. ");
            sb.AppendLine("*************************************************************************************");

            if (_jobsWhichDidnotRunList.Count == 0)
            {
                sb.AppendLine("0 did not run");                
            }
            else
            {
                foreach (string item in _jobsWhichDidnotRunList)
                {
                    if (item != "AutomatedJobStatusEmailer")
                    {
                        sb.AppendLine(item);
                    }
                }
            }
            _body = sb.ToString();
        }

        #endregion

    }
}