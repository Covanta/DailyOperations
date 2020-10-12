using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;
using Covanta.Utilities;
using Covanta.Common.Enums;
using Covanta.Utilities.Helpers;

namespace Covanta.DataAccess
{
    /// <summary>
    /// Performs CRUD functions to the Business Unit Data
    /// </summary>
    public class DALBusinessUnits : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALBusinessUnits(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods

        /// <summary>
        /// Gets a list of Business Units from the Database
        /// </summary>        
        /// <returns>a list of Business Units from the Database</returns>
        public List<DailyOpsBusinessUnit> GetDailyOpsBusinessUnitsList()
        {
            DataTable dt = new DataTable();
            List<DailyOpsBusinessUnit> list = new List<DailyOpsBusinessUnit>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.DailyOpsBusinessUnitSQL.SQLSP_GET_DailyOpsBUSINESS_UNIT_LIST;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadDailyOpsBusinessUnitList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            //sort
            list = (from u in list
                    orderby u.Description
                    select u).ToList();

            return list;

        }

        /// <summary>
        /// Returns a list of emails by Facility and Position
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>
        /// <param name="position">must be CE (chief Engineer) or FM (facility Manager)</param>
        /// <returns>list of emails</returns>
        public List<string> GetDailyOpsBusinessUnitContactEmailList(string facilityID, string position)
        {
            if ((position != "FM") && (position != "CE"))
            {
                return new List<string>();
            }

            DataTable dt = new DataTable();
            List<string> list = new List<string>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.DailyOpsBusinessUnitSQL.SQLSP_GET_GetDailyInputFormContactData_LIST;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                list.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    if ((bindObj.ToStringValue("FacilityID") == facilityID) && (bindObj.ToStringValue("Position") == position))
                    {
                        string email = bindObj.ToStringValue("EmailAddress");
                        list.Add(email);
                    }
                }
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }

        /// <summary>
        /// Returns a list of Contact names by Facility and Position
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>
        /// <param name="position">must be CE (chief Engineer) or FM (facility Manager)</param>
        /// <returns>list of string</returns>
        public List<string> GetDailyOpsBusinessUnitContactNameList(string facilityID, string position)
        {
            if ((position != "FM") && (position != "CE"))
            {
                return new List<string>();
            }

            DataTable dt = new DataTable();
            List<string> list = new List<string>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.DailyOpsBusinessUnitSQL.SQLSP_GET_GetDailyInputFormContactData_LIST;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                list.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    if ((bindObj.ToStringValue("FacilityID") == facilityID) && (bindObj.ToStringValue("Position") == position))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(bindObj.ToStringValue("FirstName"));
                        sb.Append(" ");
                        sb.Append(bindObj.ToStringValue("LastName"));
                        string name = sb.ToString();
                        list.Add(name);
                    }
                }
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }


        /// <summary>
        /// Returns a list of NT IDs names by Facility and Position
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>
        /// <param name="position">must be CE (chief Engineer) or FM (facility Manager)</param>
        /// <returns>list of string</returns>
        public List<string> GetDailyOpsBusinessUnitNTIDList(string facilityID, string position)
        {
            if ((position != "FM") && (position != "CE"))
            {
                return new List<string>();
            }

            DataTable dt = new DataTable();
            List<string> list = new List<string>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.DailyOpsBusinessUnitSQL.SQLSP_GET_GetDailyInputFormContactData_LIST;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                list.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    if ((bindObj.ToStringValue("FacilityID") == facilityID) && (bindObj.ToStringValue("Position") == position))
                    {
                        string id = bindObj.ToStringValue("NTID");
                        list.Add(id);
                    }
                }
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }

        /// <summary>
        /// Returns the number of hours (could be 0) that the facility is offset from Eastern time
        /// Ex. Long Beach is -3
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>      
        /// <returns>string with hours offset</returns>
        public string GetDailyOpsBusinessUnitEasternTimeOffset(string facilityID)
        {
            string timeOffset = "0";
            DataTable dt = new DataTable();         

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.DailyOpsBusinessUnitSQL.SQLSP_GET_DailyOpsBUSINESS_UNIT_LIST;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    string ps_Unit = bindObj.ToStringValue("DailyOPsBusinessUnitPSCode");
                    if (facilityID == ps_Unit)
                    {
                        timeOffset = bindObj.ToStringValue("EasternTimeOffset");
                        break;
                    }
                }
            }

            //set null reference to uneeded objects
            dt = null;

            return timeOffset;
        }


        #endregion

        #region private methods

        /// <summary>
        /// Load a list of Business Unit objects
        /// </summary>
        /// <param name="dt">Datable with Business Units</param>
        /// <param name="list">List of BusinessUnit objects</param>
        private void loadDailyOpsBusinessUnitList(DataTable dt, List<DailyOpsBusinessUnit> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                string ps_Unit = bindObj.ToStringValue("DailyOPsBusinessUnitPSCode");
                string description = bindObj.ToStringValue("DailOPsBusinessUnitDescription");
                int numOfBoilers = bindObj.ToInteger("DailOPsBusinessUnitNumberOfBoilers");
                bool isFacilityFrontEndFerrousSystem = false;
                if (bindObj.ToStringValue("IsFacilityFrontEndFerrousSystem") == "Y") { isFacilityFrontEndFerrousSystem = true; }

                DailyOpsBusinessUnit obj = new DailyOpsBusinessUnit(ps_Unit, description, numOfBoilers, isFacilityFrontEndFerrousSystem);

                obj.Region = bindObj.ToStringValue("Region");
                obj.FacilityType = bindObj.ToStringValue("FacilityType");
                obj.NumOfTurbines = bindObj.ToInteger("DailOPsBusinessUnitNumberOfTurbines");

                obj.MaxNetElectric = bindObj.ToInteger("MaxNetElectric");
                obj.MaxSteamProduced = bindObj.ToInteger("MaxSteamProduced");
                obj.MaxSteamSold = bindObj.ToInteger("MaxSteamSold");
                obj.MaxTonsDelivered = bindObj.ToInteger("MaxTonsDelivered");
                obj.MaxTonsProcessed = bindObj.ToInteger("MaxTonsProcessed");

                list.Add(obj);
            }
        }

        #endregion
    }
}
