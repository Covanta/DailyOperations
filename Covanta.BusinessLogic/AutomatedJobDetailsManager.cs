using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;
using Covanta.Common.Enums;

namespace Covanta.BusinessLogic
{
    public class AutomatedJobDetailsManager
    {
        #region  public static methods

        /// <summary>
        /// Updates database with the job status of the job that is running.  Pass, Fail, Etc. and last run date and time
        /// </summary>
        /// <param name="automatedJobName">This Job Name</param>
        /// <param name="automatedJobStatus">Status we are passing to the database</param>
        /// <param name="automatedStatusDescription">Job Status Description text</param>
        /// <param name="dbConnection">connection to the database</param>
        public static void UpdateJobStatus(Enums.AutomatedJobNameEnum automatedJobName, Enums.AutomatedJobStatusEnum automatedJobStatus, string automatedStatusDescription, string dbConnection)
        {
            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            string statusMsg = string.Empty;

            try
            {
                DALAutomatedJobDetails dal = new DALAutomatedJobDetails(dbConnection);           
                dal.UpdateJobStatus(automatedJobName, automatedJobStatus, automatedStatusDescription, ref status, ref statusMsg);   
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }      
        }
                
        /// <summary>
        /// Gets the job details of this job from the database
        /// </summary>
        /// <param name="automatedJobName">his Job Name</param>
        /// <param name="dbConnection">connection to the database</param>
        /// <returns>A populated object of this jobs run details</returns>
        public static AutomatedJobDetails GetAutomatedJobDetails(Enums.AutomatedJobNameEnum automatedJobName, string dbConnection)
        {            
            AutomatedJobDetails automatedJobDetails = new AutomatedJobDetails();
            List<string> listOfEmails = new List<string>();
            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            string statusMsg = string.Empty;
            try
            {
                DALAutomatedJobDetails dal = new DALAutomatedJobDetails(dbConnection);
                automatedJobDetails = dal.GetAutomatedJobDetails(automatedJobName, ref status, ref statusMsg);
                listOfEmails = dal.GetAutomatedJobsEmailListByApplicationName(automatedJobDetails.EmailNotificationListID, ref status, ref statusMsg);
                automatedJobDetails.EmailsToBeNotifiedList = listOfEmails;
                populateEmailProperty(automatedJobDetails);
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return automatedJobDetails;
        }
        
        /// <summary>
        /// Gets a list of AutomatedJobDetails
        /// </summary>
        /// <param name="dbConnection">connection to the database</param>
        /// <returns>A populated list of AutomatedJobDetails</returns>
        public static List<AutomatedJobDetails> GetAutomatedJobDetailsList(string dbConnection)
        {
            List<AutomatedJobDetails> list = new List<AutomatedJobDetails>();
           
            try
            {
                DALAutomatedJobDetails dal = new DALAutomatedJobDetails(dbConnection);
                list = dal.GetAutomatedJobDetailsList();
                Enums.StatusEnum status = Enums.StatusEnum.NotSet;
                string statusMsg = string.Empty;
                foreach (AutomatedJobDetails detail in list)
                {
                    status = Enums.StatusEnum.NotSet;
                    statusMsg = string.Empty;
                    List<string> listOfEmails = new List<string>();
                    listOfEmails = dal.GetAutomatedJobsEmailListByApplicationName(detail.EmailNotificationListID, ref status, ref statusMsg);
                    detail.EmailsToBeNotifiedList = listOfEmails;
                    populateEmailProperty(detail);
                }

            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return list;
        }

        #endregion

        #region private static methods

        /// <summary>
        /// Formats the exception message
        /// </summary>      
        /// <param name="e">The exception which was thrown</param>
        /// <returns>Formatted text describing the exception</returns>
        private static string formatExceptionText(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("An Exception has occurred.");
            sb.Append("\n\n");
            sb.Append("Message: " + e.Message);
            sb.Append("\n");
            sb.Append("Source: " + e.Source);

            return sb.ToString();
        }
       

        public static void populateEmailProperty(AutomatedJobDetails automatedJobDetails)
        {
            string emailString = string.Empty;
            foreach (string email in automatedJobDetails.EmailsToBeNotifiedList)
            {
                emailString = emailString + email + ",";
            }
            if (emailString.Length > 2)
            {
                automatedJobDetails.EmailsToBeNotified = emailString.Substring(0, emailString.Length - 1);
            }
            else
            {
                automatedJobDetails.EmailsToBeNotified = @"nobody@covantaenergy.com";
            }
        }
        #endregion
    }
}