using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALCeridianADSync : DALBase
    {
        #region constructors
      
        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALCeridianADSync(string dbConnection) : base(dbConnection) { }       

        #endregion

        #region public methods


        public void InsertCeridianAndADSyncOutputRow(List<CeridianAndADSyncOututRow> list)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();

                foreach (var item in list)
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.CeridianAndActiveDirDifferencesSQL.SQLSP_Insert_AD_Ceridian_Differences_Data;
                        command.Parameters.AddWithValue("@AD_Department", item.AD_Department);
                        command.Parameters.AddWithValue("@ad_Employeeid", item.ad_Employeeid);
                        command.Parameters.AddWithValue("@AD_GivenName", item.AD_GivenName);
                        command.Parameters.AddWithValue("@AD_Initials", item.AD_Initials);
                        command.Parameters.AddWithValue("@AD_MailNickname", item.AD_MailNickname);
                        command.Parameters.AddWithValue("@AD_Manager", item.AD_Manager);
                        command.Parameters.AddWithValue("@AD_Phone", item.AD_Phone);
                        command.Parameters.AddWithValue("@AD_PhysicalDeliveryOfficeName", item.AD_PhysicalDeliveryOfficeName);
                        command.Parameters.AddWithValue("@AD_SamAccountName", item.AD_SamAccountName);
                        command.Parameters.AddWithValue("@AD_Surname", item.AD_Surname);
                        command.Parameters.AddWithValue("@AD_Title", item.AD_Title);
                        command.Parameters.AddWithValue("@cer_ClockNumber", item.cer_ClockNumber);
                        command.Parameters.AddWithValue("@cer_DateEntered", item.cer_DateEntered);
                        command.Parameters.AddWithValue("@cer_DateLastHired", item.cer_DateLastHired);
                        command.Parameters.AddWithValue("@cer_DayBorn", item.cer_DayBorn);
                        command.Parameters.AddWithValue("@Cer_Dept", item.Cer_Dept);
                        command.Parameters.AddWithValue("@cer_Division", item.cer_Division);
                        command.Parameters.AddWithValue("@cer_EbPSID", item.cer_EbPSID);
                        command.Parameters.AddWithValue("@cer_Employee_Supervisor_Name", item.cer_Employee_Supervisor_Name);
                        command.Parameters.AddWithValue("@cer_ExemptStatus", item.cer_ExemptStatus);
                        command.Parameters.AddWithValue("@cer_FirstName", item.cer_FirstName);
                        command.Parameters.AddWithValue("@cer_LastName", item.cer_LastName);
                        command.Parameters.AddWithValue("@cer_Location", item.cer_Location);
                        command.Parameters.AddWithValue("@cer_Mid_Init", item.cer_Mid_Init);
                        command.Parameters.AddWithValue("@cer_MonthBorn", item.cer_MonthBorn);
                        command.Parameters.AddWithValue("@cer_OfficePhone", item.cer_OfficePhone);
                        command.Parameters.AddWithValue("@cer_PayGroup", item.cer_PayGroup);
                        command.Parameters.AddWithValue("@cer_PayType", item.cer_PayType);
                        command.Parameters.AddWithValue("@cer_Region", item.cer_Region);
                        command.Parameters.AddWithValue("@cer_SSN", item.cer_SSN);
                        command.Parameters.AddWithValue("@cer_Status", item.cer_Status);
                        command.Parameters.AddWithValue("@cer_Suffix", item.cer_Suffix);
                        command.Parameters.AddWithValue("@cer_Supervisor_Flxid", item.cer_Supervisor_Flxid);
                        command.Parameters.AddWithValue("@cer_TermDate", item.cer_TermDate);
                        command.Parameters.AddWithValue("@cer_Title", item.cer_Title);
                        command.Parameters.AddWithValue("@cer_Union", item.cer_Union);
                        command.Parameters.AddWithValue("@cer_UnionIndicator", item.cer_UnionIndicator);
                        command.Parameters.AddWithValue("@Flag_Different_FirstName", item.Flag_Different_FirstName);
                        command.Parameters.AddWithValue("@Flag_Different_Lastname", item.Flag_Different_Lastname);
                        command.Parameters.AddWithValue("@Flag_Different_Manager", item.Flag_Different_Manager);
                        command.Parameters.AddWithValue("@Flag_Different_Title", item.Flag_Different_Title);
                        command.Parameters.AddWithValue("@Flag_AtLeastOneDifferenceFound", item.Flag_AtLeastOneDifferenceFound);
                      
                        command.ExecuteNonQuery();
                    }
                }
            }

          

        }

        public void TruncateActiveDirAndCeridianDifferencesTable()
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = DALSqlStatements.CeridianAndActiveDirDifferencesSQL.SQLSP_Truncate_ActiveDirAndCeridianDifferences;
                    command.ExecuteNonQuery();
                }                
            }
        }

        /// <summary>
        /// calls a stored procedure to Insert new rows into the Propossed SamAccountNameTable
        /// </summary>
        public void InsertIntoProposedSamAccountNamesTable()
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = DALSqlStatements.CeridianAndActiveDirDifferencesSQL.SQLSP_Insert_ProposedSamAccountNames;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Gets a list of CeridianAndADSyncInputRow from the Database
        /// </summary>        
        /// <returns>a list of CeridianAndADSyncInputRow from the Database</returns>
        public List<CeridianAndADSyncInputRow> GetCeridianAndADSyncInputRowList()
        {
            DataTable dt = new DataTable();
            List<CeridianAndADSyncInputRow> list = new List<CeridianAndADSyncInputRow>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.CeridianAndActiveDirDifferencesSQL.SQLSP_Get_AD_Ceridian_Merge_VW_Data;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadCeridianAndADSyncInputRowList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            return list;

        }

        /// <summary>
        /// Gets a list of Proposed new Ad accounts and details with no conflicts
        /// </summary>
        /// <returns>a list of CeridianAndADSyncProposedSAMwithNoConflict objects</returns>
        public List<CeridianAndADSyncProposedSAMwithNoConflict> GetCeridianAndADProposedSAMwithNoConflicts()
        {
            DataTable dt = new DataTable();
            List<CeridianAndADSyncProposedSAMwithNoConflict> list = new List<CeridianAndADSyncProposedSAMwithNoConflict>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.CeridianAndActiveDirDifferencesSQL.SQLSP_Get_AD_Ceridian_Proposed_With_No_Conflict_Detail;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadCeridianAndADSyncProposedSAMwithNoConflictList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
          
        }

       
        #endregion

        #region private methods

        /// <summary>
        /// Load a list of CeridianAndADSyncInputRow
        /// </summary>
        /// <param name="dt">Datable with CeridianAndADSyncInputRow</param>
        /// <param name="list">List of CeridianAndADSyncInputRow</param>
        private void loadCeridianAndADSyncInputRowList(DataTable dt, List<CeridianAndADSyncInputRow> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                CeridianAndADSyncInputRow obj = new CeridianAndADSyncInputRow();
                obj.AD_Department = bindObj.ToStringValue("AD_Department");
                obj.ad_Employeeid = bindObj.ToStringValue("ad_Employeeid");
                obj.AD_GivenName = bindObj.ToStringValue("AD_GivenName");
                obj.AD_Initials = bindObj.ToStringValue("AD_Initials");
                obj.AD_MailNickname = bindObj.ToStringValue("AD_MailNickname");
                obj.AD_Manager = bindObj.ToStringValue("AD_Manager");
                obj.AD_Phone = bindObj.ToStringValue("AD_Phone");
                obj.AD_PhysicalDeliveryOfficeName = bindObj.ToStringValue("AD_PhysicalDeliveryOfficeName");
                obj.AD_SamAccountName = bindObj.ToStringValue("AD_SamAccountName");
                obj.AD_Surname = bindObj.ToStringValue("AD_Surname");
                obj.AD_Title = bindObj.ToStringValue("AD_Title");
                obj.cer_ClockNumber = bindObj.ToStringValue("cer_ClockNumber");
                obj.cer_DateEntered = bindObj.ToDate("cer_DateEntered");
                obj.cer_DateLastHired = bindObj.ToDate("cer_DateLastHired");
                // some rows do not have Month Born or Day Born, to avoid an Exception, we just put in a dummy value
                if (bindObj.ToStringValue("cer_DayBorn") != null)
                {
                    obj.cer_DayBorn = bindObj.ToStringValue("cer_DayBorn");
                }
                else
                {
                    obj.cer_DayBorn = "01";
                }
                //obj.cer_DayBorn = bindObj.ToStringValue("cer_DayBorn");
                obj.Cer_Dept = bindObj.ToStringValue("Cer_Dept");
                obj.cer_Division = bindObj.ToStringValue("cer_Division");
                obj.cer_EbPSID = bindObj.ToStringValue("cer_EbPSID");
                obj.cer_Employee_Supervisor_Name = bindObj.ToStringValue("cer_Employee_Supervisor_Name");
                obj.cer_ExemptStatus = bindObj.ToStringValue("cer_ExemptStatus");
                obj.cer_FirstName = bindObj.ToStringValue("cer_FirstName");
                obj.cer_LastName = bindObj.ToStringValue("cer_LastName");
                obj.cer_Location = bindObj.ToStringValue("cer_Location");
                obj.cer_Mid_Init = bindObj.ToStringValue("cer_Mid_Init");
                // some rows do not have Month Born or Day Born, to avoid an Exception, we just put in a dummy value
                if (bindObj.ToStringValue("cer_MonthBorn") != null)
                {
                    obj.cer_MonthBorn = bindObj.ToStringValue("cer_MonthBorn");
                }
                else
                {
                    obj.cer_MonthBorn = "01";
                }
                //obj.cer_MonthBorn = bindObj.ToStringValue("cer_MonthBorn");
                obj.cer_OfficePhone = bindObj.ToStringValue("cer_OfficePhone");
                obj.cer_PayGroup = bindObj.ToStringValue("cer_PayGroup");
                obj.cer_PayType = bindObj.ToStringValue("cer_PayType");
                obj.cer_Region = bindObj.ToStringValue("cer_Region");
                obj.cer_SSN = bindObj.ToStringValue("cer_SSN");
                obj.cer_Status = bindObj.ToStringValue("cer_Status");
                obj.cer_Suffix = bindObj.ToStringValue("cer_Suffix");
                obj.cer_Supervisor_Flxid = bindObj.ToInteger("cer_Supervisor_Flxid");
                obj.cer_TermDate = bindObj.ToDate("cer_TermDate");
                obj.cer_Title = bindObj.ToStringValue("cer_Title");
                obj.cer_Union = bindObj.ToStringValue("cer_Union");
                obj.cer_UnionIndicator = bindObj.ToStringValue("cer_UnionIndicator");
               
                list.Add(obj);
            }
        }

        /// <summary>
        /// Load a list of CeridianAndADSyncProposedSAMwithNoConflict
        /// </summary>
        /// <param name="dt">Datable with CeridianAndADSyncProposedSAMwithNoConflict</param>
        /// <param name="list">List of CeridianAndADSyncProposedSAMwithNoConflict objects</param>
        private void loadCeridianAndADSyncProposedSAMwithNoConflictList(DataTable dt, List<CeridianAndADSyncProposedSAMwithNoConflict> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                CeridianAndADSyncProposedSAMwithNoConflict obj = new CeridianAndADSyncProposedSAMwithNoConflict();
                obj.ClockNumber = bindObj.ToStringValue("ClockNumber");
                obj.FirstName = bindObj.ToStringValue("FirstName");
                obj.MiddleInitial = bindObj.ToStringValue("MiddleInitial");
                obj.LastName = bindObj.ToStringValue("LastName");
                obj.ProposedSamAccountName = bindObj.ToStringValue("Proposed SamAccountName");
                obj.Title = bindObj.ToStringValue("Title");
                obj.Region = bindObj.ToStringValue("Region");
                obj.Department = bindObj.ToStringValue("Department");
                obj.Division = bindObj.ToStringValue("Division");
                obj.MgrClockNumber = bindObj.ToStringValue("MgrClockNumber");
                obj.UnionIndicator = bindObj.ToStringValue("Union Indicator");
                obj.ExemptStatus = bindObj.ToStringValue("ExemptStatus");

                list.Add(obj);
            }
        }

        #endregion
    }
}
