using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.Utilities.Helpers;
using Covanta.Common.Enums;
using System.Data.SqlClient;
using System.Data;

namespace Covanta.DataAccess
{
    public class DALAutomatedJobDetails
    {

        #region private variables

        string _dbConnection = null;

        #endregion

        #region constructors

        public DALAutomatedJobDetails(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == ""))
            {
                EmailHelper.SendEmail("DALAutomatedJobDetails missing Connection String");
                _dbConnection = "";
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns a list of emails which should receive notification for each scheduled job.  This list is usually just added to the EmailToBeNotified property of the AutomatedJobDetails object
        /// </summary>
        /// <param name="emailNotificationListID">The ID of the application which we are interested in knowing who will receive notification emails.  This field is from a propertyof the AutomatedJobDetails object</param>
        /// <param name="status"></param>
        /// <param name="statusMsg"></param>
        /// <returns>a list of emails which should receive notification for each scheduled job.</returns>
        public List<string> GetAutomatedJobsEmailListByApplicationName(int emailNotificationListID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            List<string> list = new List<string>();

            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.AutomatedJobDetailsSQL.SQLSP_GET_AutomatedJobsEmailList_LIST;                     
                        commandString = populateExceptionData(command);

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];
                    }
                }
            }
            catch (Exception ex)
            {
                status = Enums.StatusEnum.Error;
                statusMsg = commandString + ex.ToString();
                return null;
            }

            // load object from DataTable if at least 1 row returned
            if (tableList.Rows.Count > 0)
            {
                foreach (DataRow dr in tableList.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);  
                    if (bindObj.ToInteger("EmailNotificationListID") == emailNotificationListID)
                    {
                        list.Add(bindObj.ToStringValue("EmailToBeNotified"));
                    }
                }     
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;
            string noEmailFound = @"noEmailFound@covantaenergy.com";
            if (list.Count == 0) { list.Add(noEmailFound); }
            return list;
        }
       
        /// <summary>
        /// Populates the AutomatedJobDetails object with metadata from the database.
        /// </summary>
        /// <param name="applicationName">The application which we are interested in knowing the details pertaining to the job.  Database info, notification info, scheduled time info, etc</param>
        /// <param name="status"></param>
        /// <param name="statusMsg"></param>
        /// <returns></returns>
        public AutomatedJobDetails GetAutomatedJobDetails(Enums.AutomatedJobNameEnum automatedJobName, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            AutomatedJobDetails automatedJobDetails = null;

            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;
            string applicationName = string.Empty;

            applicationName = GetAutomatedJobNameFromEnum(automatedJobName, applicationName);

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.AutomatedJobDetailsSQL.SQLSP_GET_AutomatedJobDetails_By_ApplicationName;
                        command.Parameters.AddWithValue("@applicationName", applicationName);

                        commandString = populateExceptionData(command);

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];
                    }
                }
            }
            catch (Exception ex)
            {
                status = Enums.StatusEnum.Error;
                statusMsg = commandString + ex.ToString();
                return null;
            }


            // load object from DataTable if at least 1 row returned
            if (tableList.Rows.Count > 0)
            {
                automatedJobDetails =  loadAutomatedJobDetailsDataOneRow(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;


            return automatedJobDetails;
        }
                      
        /// <summary>
        /// Populates a list of AutomatedJobDetails objects with metadata from the database.
        /// </summary>
        /// <returns>a list of AutomatedJobDetails objects with metadata from the database.</returns>
        public List<AutomatedJobDetails> GetAutomatedJobDetailsList()
        {
            List<AutomatedJobDetails> list = new List<AutomatedJobDetails>();
            DataSet DS = new DataSet();
            DataTable tableList = null;           

            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;
            string applicationName = string.Empty;                        

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.AutomatedJobDetailsSQL.SQLSP_GET_AutomatedJobDetails_LIST;                      

                        commandString = populateExceptionData(command);

                        connection.Open();
                        // load dataSet
                        DS.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "tableList" });

                        tableList = DS.Tables["tableList"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            // load objects from DataTable if at least 1 row returned
            if (tableList.Rows.Count > 0)
            {
                foreach (DataRow dr in tableList.Rows)
                {
                    AutomatedJobDetails details = new AutomatedJobDetails();
                    details = loadAutomatedJobDetailsData(dr);
                    list.Add(details);
                }
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;


            return list;
        }
          
        /// <summary>
        /// Updates the database with the status of the job run and description of error (if one occurs).
        /// </summary>
        /// <param name="automatedJobName">Nmae of the job running</param>
        /// <param name="automatedJobStatus">Status (Pass,Fail, etc)</param>
        /// <param name="automatedStatusDescription">Error if one occurs</param>
        /// <param name="status">staus of this call</param>
        /// <param name="statusMsg">status message of this call</param>
        public void UpdateJobStatus(Enums.AutomatedJobNameEnum automatedJobName, Enums.AutomatedJobStatusEnum automatedJobStatus, string automatedStatusDescription, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;          

            //int statusValue = -1;
            string statusValue = string.Empty;
            switch (automatedJobStatus)
            {
                case Enums.AutomatedJobStatusEnum.Success:
                    statusValue = "Success";                   
                    break;
                case Enums.AutomatedJobStatusEnum.Fail:
                    statusValue = "Fail";                   
                    break;
                case Enums.AutomatedJobStatusEnum.Reset:                   
                    statusValue = "Reset";                   
                    break;
                default:
                    statusValue = "Unknown";                 
                    break;
            }

            string applicationName = string.Empty;
            applicationName = GetAutomatedJobNameFromEnum(automatedJobName, applicationName);


            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.AutomatedJobDetailsSQL.SQLSP_Update_AutomatedJobDetails_By_ApplicationName;
                        command.Parameters.AddWithValue("@applicationName", applicationName);
                        command.Parameters.AddWithValue("@status", statusValue);
                        command.Parameters.AddWithValue("@statusDescription", automatedStatusDescription);

                        commandString = populateExceptionData(command);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                status = Enums.StatusEnum.Error;
                statusMsg = commandString + ex.ToString();
                return;
            }
        }       

        #endregion

        #region private methods

        /// <summary>
        /// Loads one AutomatedJobDetails object from a datatable
        /// </summary>
        /// <param name="dt">DataTable (should contain only 1 row)</param>
        /// <returns>1 AutomatedJobDetails object</returns>
        private AutomatedJobDetails loadAutomatedJobDetailsDataOneRow(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            AutomatedJobDetails obj = loadAutomatedJobDetailsData(dr);

            return obj;
        }
        
        /// <summary>
        /// Loads one AutomatedJobDetails object from a datarow
        /// </summary>
        /// <param name="dr">one datarow</param>
        /// <returns>a populated AutomatedJobDetails object</returns>
        private static AutomatedJobDetails loadAutomatedJobDetailsData(DataRow dr)
        {
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            string applicationName = bindObj.ToStringValue("ApplicationName");
            string job = bindObj.ToStringValue("Job");
            string status = bindObj.ToStringValue("Status");
            string statusDescription = bindObj.ToStringValue("StatusDescription");
            string jobRunsOnServerName = bindObj.ToStringValue("JobRunsOnServerName");
            string jobRunsUnderID = bindObj.ToStringValue("JobRunsUnderID");
            string author = bindObj.ToStringValue("Author");
            int emailNotificationListID = bindObj.ToInteger("EmailNotificationListID");
            string databaseServer = bindObj.ToStringValue("DatabaseServer");
            string scheduledRunTime = bindObj.ToStringValue("ScheduledRunTime");
            DateTime lastRunTime = bindObj.ToDate("LastRunTime");
            AutomatedJobDetails obj = new AutomatedJobDetails();

            obj.ApplicationName = applicationName;
            obj.Author = author;
            obj.DatabaseServer = databaseServer;
            obj.EmailNotificationListID = emailNotificationListID;
            obj.Job = job;
            obj.JobRunsOnServerName = jobRunsOnServerName;
            obj.JobRunsUnderID = jobRunsUnderID;
            obj.LastRunTime = lastRunTime;
            obj.ScheduledRunTime = scheduledRunTime;
            obj.Status = status;
            obj.StatusDescription = statusDescription;
            //obj.EmailToBeNotified
            return obj;
        }

        private static string GetAutomatedJobNameFromEnum(Enums.AutomatedJobNameEnum automatedJobName, string applicationName)
        {
            switch (automatedJobName)
            {
                case Enums.AutomatedJobNameEnum.LongviewExtract:
                    applicationName = "LongviewExtract";
                    break;
                case Enums.AutomatedJobNameEnum.C4RForecastApprovals:
                    applicationName = "C4RForecastApprovals";
                    break;
                case Enums.AutomatedJobNameEnum.WorkDaySFTP:
                    applicationName = "WorkDaySFTP";
                    break;
                case Enums.AutomatedJobNameEnum.AutomatedJobStatusEmailer:
                    applicationName = "AutomatedJobStatusEmailer";
                    break;
                case Enums.AutomatedJobNameEnum.SSIS_WorkDayExtract:
                    applicationName = "SSIS_WorkDayExtract";
                    break;
                case Enums.AutomatedJobNameEnum.SSIS_WorkDayPushToCSV:
                    applicationName = "SSIS_WorkDayPushToCSV";
                    break;
                case Enums.AutomatedJobNameEnum.SSIS_LoadDBFromADirCSV:
                    applicationName = "SSIS_LoadDBFromADirCSV";
                    break;
                default:
                    applicationName = string.Empty;
                    break;
            }
            return applicationName;
        }

        private string populateExceptionData(SqlCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(command.Connection.ConnectionString);
            sb.AppendLine(command.CommandText);
            sb.AppendLine(command.CommandType.ToString());
            foreach (SqlParameter param in command.Parameters)
            {
                sb.AppendLine(param.ToString() + "  " + param.Value.ToString());
            }

            return sb.ToString();
        }

        #endregion
    }
}
