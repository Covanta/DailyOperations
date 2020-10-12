using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALEmployeeData : DALBase
    {
        #region constructors
      
        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALEmployeeData(string dbConnection) : base(dbConnection) { }       

        #endregion

        #region public methods
               
        /// <summary>
        /// Returns a list of ActiveDirectoryInfo objects by LastName
        /// </summary>
        /// <param name="lastname">LastName</param>
        /// <returns>list of ActiveDirectoryInfo objects</returns>
        public List<ActiveDirectoryInfo> GetActiveDirectoryInfoListByLastName(string lastname)
        {
            DataTable dt = new DataTable();
            List<ActiveDirectoryInfo> list = new List<ActiveDirectoryInfo>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.EmployeeDataSQL.SQLSP_Get_ActiveDirectoryInfo_By_LastName_LIST;

            command.Parameters.AddWithValue("@lastName", lastname);

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadActiveDirectoryInfoList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            return list;            
        }

        /// <summary>
        /// Returns a list of EmployeeInfo objects by LastName
        /// </summary>
        /// <param name="lastname">LastName</param>
        /// <returns>list of EmployeeInfo objects</returns>
        public List<EmployeeInfo> GetEmployeeInfoListByLastName(string lastname)
        {
            DataTable dt = new DataTable();
            List<EmployeeInfo> list = new List<EmployeeInfo>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.EmployeeDataSQL.SQLSP_Get_EmployeeInfo_By_LastName_LIST;

            command.Parameters.AddWithValue("@lastName", lastname);

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadEmployeeInfoList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }

        #endregion

        #region private methods
        
        private void loadEmployeeInfoList(DataTable dt, List<EmployeeInfo> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                EmployeeInfo obj = new EmployeeInfo();
                obj.Run_date = bindObj.ToDate("Run_date");
                obj.EE_ID = bindObj.ToStringValue("EE_ID");
                obj.Salary_Class = bindObj.ToStringValue("Salary_Class");
                obj.PayRateType = bindObj.ToStringValue("PayRateType");
                obj.Exempt = bindObj.ToStringValue("Exempt");
                obj.SSN = bindObj.ToStringValue("SSN");
                obj.WD_UserName = bindObj.ToStringValue("WD_UserName");
                obj.LAST = bindObj.ToStringValue("LAST");
                obj.FIRST = bindObj.ToStringValue("FIRST");
                obj.MIDDLE = bindObj.ToStringValue("MIDDLE");
                obj.Suffix = bindObj.ToStringValue("Suffix");
                obj.HOME_ADDR1 = bindObj.ToStringValue("HOME_ADDR1");
                obj.HOME_ADDR2 = bindObj.ToStringValue("HOME_ADDR2");
                obj.CITY = bindObj.ToStringValue("CITY");
                obj.STATE = bindObj.ToStringValue("STATE");
                obj.ZIP = bindObj.ToStringValue("ZIP");
                obj.Country = bindObj.ToStringValue("Country");
                obj.MARITAL = bindObj.ToStringValue("MARITAL");
                obj.Status = bindObj.ToStringValue("Status");
                obj.Termination_Date = bindObj.ToDate("Termination_Date");
                obj.OrigHireDate = bindObj.ToDate("OrigHireDate");
                obj.Work_Email = bindObj.ToStringValue("Work_Email");
                obj.JobTitle = bindObj.ToStringValue("JobTitle");
                obj.DivisionID = bindObj.ToStringValue("DivisionID");
                obj.PayGroup = bindObj.ToStringValue("PayGroup");
                obj.UnionName = bindObj.ToStringValue("UnionName");
                obj.RegionID = bindObj.ToStringValue("RegionID");
                obj.CompanyName = bindObj.ToStringValue("CompanyName");
                obj.Location = bindObj.ToStringValue("Location");
                obj.Department = bindObj.ToStringValue("Department");
                obj.Work_Addr1 = bindObj.ToStringValue("Work_Addr1");
                obj.Work_Addr2 = bindObj.ToStringValue("Work_Addr2");
                obj.Work_City = bindObj.ToStringValue("Work_City");
                obj.Work_State = bindObj.ToStringValue("Work_State");
                obj.Work_Country = bindObj.ToStringValue("Work_Country");
                obj.Work_Postal = bindObj.ToStringValue("Work_Postal");
                obj.Work_phone = bindObj.ToStringValue("Work_phone");
                obj.Mgr_FullName = bindObj.ToStringValue("Mgr_FullName");
                obj.Mgr_EE_ID = bindObj.ToStringValue("Mgr_EE_ID");
                obj.Mgr_Email = bindObj.ToStringValue("Mgr_Email");   
                list.Add(obj);            
            }
        }
       
        private void loadActiveDirectoryInfoList(DataTable dt, List<ActiveDirectoryInfo> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                ActiveDirectoryInfo obj = new ActiveDirectoryInfo();
                obj.Department = bindObj.ToStringValue("Department");
                obj.Employeeid = bindObj.ToStringValue("Employeeid");
                obj.GivenName = bindObj.ToStringValue("GivenName");
                obj.Initials = bindObj.ToStringValue("Initials");
                obj.MailNickname = bindObj.ToStringValue("MailNickname");
                obj.Manager = bindObj.ToStringValue("Manager");
                obj.PhysicalDeliveryOfficeName = bindObj.ToStringValue("PhysicalDeliveryOfficeName");
                obj.SamAccountName = bindObj.ToStringValue("SamAccountName");
                obj.Surname = bindObj.ToStringValue("Surname");
                obj.TelephoneNumber = bindObj.ToStringValue("TelephoneNumber");
                obj.Title = bindObj.ToStringValue("Title");             

                list.Add(obj);
            }
        }

        #endregion
    }
}
