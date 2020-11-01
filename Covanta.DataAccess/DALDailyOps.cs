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
    public class DALDailyOps
    {

        #region private variables

        string _dbConnection = null;

        #endregion

        #region constructors

        public DALDailyOps(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == ""))
            {
                EmailHelper.SendEmail("DAL_DailyOps missing Connection String");
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
        /// Get 1 days worth of Daily Ops Data from the database by 1 specific Date and 1 specific Facility.
        /// Also returns a Status Code and a Status Message (If an error occurs)
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data from</param>
        /// <param name="faciltyID">Facility ID we want date from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">Status Enum</param>
        /// <param name="statusMsg">Message containing exception if applicable</param>
        /// <returns>A populated Daily Ops object (or null if an error occurs, or no record found) </returns>
        public DailyOpsData LoadDailyOpsDataByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            DailyOpsData dailyOpsData = null;

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
                        //command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetDailyOpsDataByDateAndFacility;
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetDailyOpsDataByDateAndFacility_V2;
                        command.Parameters.AddWithValue("@date", dateDataRepresents);
                        command.Parameters.AddWithValue("@facilityID", faciltyID);

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
                dailyOpsData = loadDailyOpsData(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return dailyOpsData;
        }

        public List<MSWInventoryExceptions> GetMSWInventoryExceptions(DateTime dateDataRepresents, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            List<MSWInventoryExceptions> mswInventoryExceptions = null;

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
                        //command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetDailyOpsDataByDateAndFacility;
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_MSWInventoryException;
                        command.Parameters.AddWithValue("@date", dateDataRepresents);

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
                mswInventoryExceptions = loadMSWInventoryExceptions(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return mswInventoryExceptions;
        }

        private List<MSWInventoryExceptions> loadMSWInventoryExceptions(DataTable dt)
        {
            List<MSWInventoryExceptions> objList = new List<MSWInventoryExceptions>();

            foreach (DataRow row in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(row);

                string facilityType = bindObj.ToStringValue("FacilityType");
                string facility = bindObj.ToStringValue("Facility");
                string limit = bindObj.ToStringValue("Limit");
                decimal ActualInventory = bindObj.ToDecimal("ActualInventory");
                int InventoryMinLimit = bindObj.ToInteger("InventoryMinLimit");
                int InventoryMaxLimit = bindObj.ToInteger("InventoryMaxLimit");

                MSWInventoryExceptions obj = new MSWInventoryExceptions(facilityType, facility, limit, ActualInventory, InventoryMinLimit, InventoryMaxLimit);

                objList.Add(obj);
            }
            return objList;
        }

        public CompleteDowntime CumulativeDowntime(DateTime dateDataRepresents, string faciltyID, Enums.DowntimeBoilerEnum downtimeBoiler,  ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            CompleteDowntime completeDowntime = new CompleteDowntime();

            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            string storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDowntimeBoiler1;

            if (downtimeBoiler == Enums.DowntimeBoilerEnum.DowntimeBoiler2)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDowntimeBoiler2;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.DowntimeBoiler3)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDowntimeBoiler3;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.DowntimeBoiler4)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDowntimeBoiler4;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.DowntimeBoiler5)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDowntimeBoiler5;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.DowntimeBoiler6)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDowntimeBoiler6;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.DownTimeTurbGen1)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDownTimeTurbGen1;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.DownTimeTurbGen2)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeDownTimeTurbGen2;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.EnhancedFerrousSystemHoursUnavailable)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeEnhancedFerrousSystemHoursUnavailable;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.EnhancedNonFerrousSystemHoursUnavailable)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeEnhancedNonFerrousSystemHoursUnavailable;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.FerrousSystemHoursUnavailable)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeFerrousSystemHoursUnavailable;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.FrontEndFerrousSystemHoursUnavailable)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeFrontEndFerrousSystemHoursUnavailable;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.NonFerrousSmallsSystemHoursUnavailable)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeNonFerrousSmallsSystemHoursUnavailable;
            }
            else if (downtimeBoiler == Enums.DowntimeBoilerEnum.NonFerrousSystemHoursUnavailable)
            {
                storedProcedure = DALSqlStatements.CumulativeDowntimeSQL.SQLSP_CumulativeNonFerrousSystemHoursUnavailable;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storedProcedure;
                        command.Parameters.AddWithValue("@date", dateDataRepresents);
                        command.Parameters.AddWithValue("@facilityID", faciltyID);

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
                return completeDowntime;
            }


            // load object from DataTable if at least 1 row returned
            if (tableList.Rows.Count > 0)
            {
                DataRow dr = tableList.Rows[0];
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                completeDowntime.CumulativeDowntime = bindObj.ToDecimal("CumulativeDowntime");
                completeDowntime.WeekToDate = bindObj.ToDecimal("WeekToDate");
                completeDowntime.MonthToDate = bindObj.ToDecimal("MonthToDate");
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return completeDowntime;
        }

        public DailyOpsDataWithID LoadDailyOpsDataByID(int ID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            DailyOpsDataWithID dailyOpsDataWithID = null;

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetDailyOpsDataByID;
                        command.Parameters.AddWithValue("@ID", ID);

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
                dailyOpsDataWithID = loadDailyOpsDataWithID(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return dailyOpsDataWithID;
        }

        public List<DailyOpsData> LoadDailyOpsDataByMonthAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            List<DailyOpsData> dailyOpsDataList = new List<DailyOpsData>();

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetDailyOpsDataByMonthAndFacility;
                        command.Parameters.AddWithValue("@date", dateDataRepresents);
                        command.Parameters.AddWithValue("@facilityID", faciltyID);

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
                dailyOpsDataList = loadDailyOpsDataByMonth(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return dailyOpsDataList;
        }
        public List<DailyOpsDataWithID> LoadDailyOpsDataWithIDByMonthAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            List<DailyOpsDataWithID> dailyOpsWithIDDataList = new List<DailyOpsDataWithID>();

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetDailyOpsDataByMonthAndFacility;
                        command.Parameters.AddWithValue("@date", dateDataRepresents);
                        command.Parameters.AddWithValue("@facilityID", faciltyID);

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
                dailyOpsWithIDDataList = loadDailyOpsDataWithIDByMonth(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return dailyOpsWithIDDataList;
        }
        public List<DailyOpsDataWithID> LoadActiveDailyOpsDataWithIDByFacility(string faciltyID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            List<DailyOpsDataWithID> dailyOpsWithIDDataList = new List<DailyOpsDataWithID>();

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
                        //command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetActiveDailyOpsDataByFacilityID;
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GetActiveDailyOpsDataByFacilityID_V2;
                        command.Parameters.AddWithValue("@facilityID", faciltyID);
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
                dailyOpsWithIDDataList = loadDailyOpsDataWithIDByMonth(tableList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;

            //sort        
            //list = (from u in list
            //        orderby u.Cer_Unit, u.PS_Unit
            //        select u).ToList();

            return dailyOpsWithIDDataList;
        }


        /// <summary>
        /// Inserts a daily Ops data record into the database.
        /// If the record already exists in the database with the same Reporting Date and Facility ID,
        /// then we add this record and set the active flag on the existing record in the database to 'N'
        /// </summary>
        /// <param name="dailyOpsData">1 daily ops data record which will be added to the database.</param>
        /// <param name="status">results of the database call</param>
        /// <param name="statusMsg">error message if applicable</param>
        public void InsertDailyOpsData(DailyOpsData data, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_InsertDailyOpsData;
                        command.Parameters.AddWithValue("@FacilityID", data.FacilityID);
                        command.Parameters.AddWithValue("@ReportingDate", data.ReportingDate);
                        command.Parameters.AddWithValue("@TonsDelivered", data.TonsDelivered);
                        command.Parameters.AddWithValue("@TonsProcessed", data.TonsProcessed);
                        command.Parameters.AddWithValue("@SteamProduced", data.SteamProduced);
                        command.Parameters.AddWithValue("@SteamSold", data.SteamSold);
                        command.Parameters.AddWithValue("@NetElectric", data.NetElectric);
                        command.Parameters.AddWithValue("@PitInventory", data.PitInventory);

                        command.Parameters.AddWithValue("@DownTimeBoiler1", data.DownTimeBoiler1);
                        command.Parameters.AddWithValue("@OutageTypeBoiler1", data.OutageTypeBoiler1);
                        command.Parameters.AddWithValue("@ExplanationBoiler1", data.ExplanationBoiler1);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler1", data.ScheduledOutageReasonBoiler1);

                        command.Parameters.AddWithValue("@DownTimeBoiler2", data.DownTimeBoiler2);
                        command.Parameters.AddWithValue("@OutageTypeBoiler2", data.OutageTypeBoiler2);
                        command.Parameters.AddWithValue("@ExplanationBoiler2", data.ExplanationBoiler2);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler2", data.ScheduledOutageReasonBoiler2);

                        command.Parameters.AddWithValue("@DownTimeBoiler3", data.DownTimeBoiler3);
                        command.Parameters.AddWithValue("@OutageTypeBoiler3", data.OutageTypeBoiler3);
                        command.Parameters.AddWithValue("@ExplanationBoiler3", data.ExplanationBoiler3);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler3", data.ScheduledOutageReasonBoiler3);

                        command.Parameters.AddWithValue("@DownTimeBoiler4", data.DownTimeBoiler4);
                        command.Parameters.AddWithValue("@OutageTypeBoiler4", data.OutageTypeBoiler4);
                        command.Parameters.AddWithValue("@ExplanationBoiler4", data.ExplanationBoiler4);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler4", data.ScheduledOutageReasonBoiler4);

                        command.Parameters.AddWithValue("@DownTimeBoiler5", data.DownTimeBoiler5);
                        command.Parameters.AddWithValue("@OutageTypeBoiler5", data.OutageTypeBoiler5);
                        command.Parameters.AddWithValue("@ExplanationBoiler5", data.ExplanationBoiler5);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler5", data.ScheduledOutageReasonBoiler5);

                        command.Parameters.AddWithValue("@DownTimeBoiler6", data.DownTimeBoiler6);
                        command.Parameters.AddWithValue("@OutageTypeBoiler6", data.OutageTypeBoiler6);
                        command.Parameters.AddWithValue("@ExplanationBoiler6", data.ExplanationBoiler6);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler6", data.ScheduledOutageReasonBoiler6);

                        command.Parameters.AddWithValue("@OutageTypeTurbGen1", data.OutageTypeTurbGen1);
                        command.Parameters.AddWithValue("@DownTimeTurbGen1", data.DownTimeTurbGen1);
                        command.Parameters.AddWithValue("@ExplanationTurbGen1", data.ExplanationTurbGen1);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonTurbGen1", data.ScheduledOutageReasonTurbGen1);

                        command.Parameters.AddWithValue("@OutageTypeTurbGen2", data.OutageTypeTurbGen2);
                        command.Parameters.AddWithValue("@DownTimeTurbGen2", data.DownTimeTurbGen2);
                        command.Parameters.AddWithValue("@ExplanationTurbGen2", data.ExplanationTurbGen2);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonTurbGen2", data.ScheduledOutageReasonTurbGen2);

                        command.Parameters.AddWithValue("@FerrousTons", data.FerrousTons);
                        command.Parameters.AddWithValue("@NonFerrousTons", data.NonFerrousTons);

                        command.Parameters.AddWithValue("@FerrousSystemHoursUnavailable", data.FerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@FerrousSystemHoursUnavailableReason", data.FerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@FerrousSystemExpectedBackOnlineDate", data.FerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughFerrousSystem", data.WasAshReprocessedThroughFerrousSystem);
                        command.Parameters.AddWithValue("@WasFerrousSystem100PercentAvailable", data.WasFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@NonFerrousSystemHoursUnavailable", data.NonFerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@NonFerrousSystemHoursUnavailableReason", data.NonFerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@NonFerrousSystemExpectedBackOnlineDate", data.NonFerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughNonFerrousSystem", data.WasAshReprocessedThroughNonFerrousSystem);
                        command.Parameters.AddWithValue("@WasNonFerrousSystem100PercentAvailable", data.WasNonFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@NonFerrousSmallsSystemHoursUnavailable", data.NonFerrousSmallsSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@NonFerrousSmallsSystemHoursUnavailableReason", data.NonFerrousSmallsSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@NonFerrousSmallsSystemExpectedBackOnlineDate", data.NonFerrousSmallsSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughNonFerrousSmallsSystem", data.WasAshReprocessedThroughNonFerrousSmallsSystem);
                        command.Parameters.AddWithValue("@WasNonFerrousSmallsSystem100PercentAvailable", data.WasNonFerrousSmallsSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@FrontEndFerrousSystemHoursUnavailable", data.FrontEndFerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@FrontEndFerrousSystemHoursUnavailableReason", data.FrontEndFerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@FrontEndFerrousSystemExpectedBackOnlineDate", data.FrontEndFerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasFrontEndFerrousSystem100PercentAvailable", data.WasFrontEndFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@FireSystemOutOfService", data.FireSystemOutOfService);
                        command.Parameters.AddWithValue("@FireSystemOutOfServiceExpectedBackOnlineDate", data.FireSystemOutOfServiceExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@CriticalAssetsInAlarm", data.CriticalAssetsInAlarm);


                        if (data.IsEnvironmentalEvents) { command.Parameters.AddWithValue("@isEnvironmentalEvents", "Y"); } else { command.Parameters.AddWithValue("@isEnvironmentalEvents", "N"); }
                        command.Parameters.AddWithValue("@EnvironmentalEventsType", data.EnvironmentalEventsType);
                        command.Parameters.AddWithValue("@EnvironmentalEventsExplanation", data.EnvironmentalEventsExplanation);

                        if (data.IsCEMSEvents) { command.Parameters.AddWithValue("@isCEMSEvents", "Y"); } else { command.Parameters.AddWithValue("@isCEMSEvents", "N"); }
                        command.Parameters.AddWithValue("@CEMSEventsType", data.CEMSEventsType);
                        command.Parameters.AddWithValue("@CEMSEventsExplanation", data.CEMSEventsExplanation);

                        command.Parameters.AddWithValue("@HealthSafetyFirstAid", data.HealthSafetyFirstAid);
                        command.Parameters.AddWithValue("@HealthSafetyOSHAReportable", data.HealthSafetyOSHAReportable);
                        command.Parameters.AddWithValue("@HealthSafetyNearMiss", data.HealthSafetyNearMiss);
                        command.Parameters.AddWithValue("@HealthSafetyContractor", data.HealthSafetyContractor);


                        command.Parameters.AddWithValue("@Comments", data.Comments);
                        command.Parameters.AddWithValue("@ActiveFlag", "Y");
                        command.Parameters.AddWithValue("@DateRowCreated", DateTime.Now);
                        command.Parameters.AddWithValue("@UserRowCreated", data.UserRowCreated);
                        command.Parameters.AddWithValue("@DateLastModified", DateTime.Now);
                        command.Parameters.AddWithValue("@UserLastModified", data.UserRowCreated);

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

        public void InsertDailyOpsData_V2(DailyOpsData data, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_InsertDailyOpsData_V2;
                        command.Parameters.AddWithValue("@FacilityID", data.FacilityID);
                        command.Parameters.AddWithValue("@ReportingDate", data.ReportingDate);
                        command.Parameters.AddWithValue("@TonsDelivered", data.TonsDelivered);
                        command.Parameters.AddWithValue("@TonsProcessed", data.TonsProcessed);
                        command.Parameters.AddWithValue("@SteamProduced", data.SteamProduced);
                        command.Parameters.AddWithValue("@SteamSold", data.SteamSold);
                        command.Parameters.AddWithValue("@NetElectric", data.NetElectric);
                        command.Parameters.AddWithValue("@PitInventory", data.PitInventory);
                        command.Parameters.AddWithValue("@PreShredInventory", data.PreShredInventory);
                        command.Parameters.AddWithValue("@PostShredInventory", data.PostShredInventory);

                        command.Parameters.AddWithValue("@DownTimeBoiler1", data.DownTimeBoiler1);
                        command.Parameters.AddWithValue("@OutageTypeBoiler1", data.OutageTypeBoiler1);
                        command.Parameters.AddWithValue("@ExplanationBoiler1", data.ExplanationBoiler1);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler1", data.ScheduledOutageReasonBoiler1);
                        command.Parameters.AddWithValue("@Boiler1ExpectedBackOnlineDate", data.Boiler1ExpectedRepairDate);
                        
                        command.Parameters.AddWithValue("@DownTimeBoiler2", data.DownTimeBoiler2);
                        command.Parameters.AddWithValue("@OutageTypeBoiler2", data.OutageTypeBoiler2);
                        command.Parameters.AddWithValue("@ExplanationBoiler2", data.ExplanationBoiler2);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler2", data.ScheduledOutageReasonBoiler2);
                        command.Parameters.AddWithValue("@Boiler2ExpectedBackOnlineDate", data.Boiler2ExpectedRepairDate);

                        command.Parameters.AddWithValue("@DownTimeBoiler3", data.DownTimeBoiler3);
                        command.Parameters.AddWithValue("@OutageTypeBoiler3", data.OutageTypeBoiler3);
                        command.Parameters.AddWithValue("@ExplanationBoiler3", data.ExplanationBoiler3);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler3", data.ScheduledOutageReasonBoiler3);
                        command.Parameters.AddWithValue("@Boiler3ExpectedBackOnlineDate", data.Boiler3ExpectedRepairDate);

                        command.Parameters.AddWithValue("@DownTimeBoiler4", data.DownTimeBoiler4);
                        command.Parameters.AddWithValue("@OutageTypeBoiler4", data.OutageTypeBoiler4);
                        command.Parameters.AddWithValue("@ExplanationBoiler4", data.ExplanationBoiler4);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler4", data.ScheduledOutageReasonBoiler4);
                        command.Parameters.AddWithValue("@Boiler4ExpectedBackOnlineDate", data.Boiler4ExpectedRepairDate);

                        command.Parameters.AddWithValue("@DownTimeBoiler5", data.DownTimeBoiler5);
                        command.Parameters.AddWithValue("@OutageTypeBoiler5", data.OutageTypeBoiler5);
                        command.Parameters.AddWithValue("@ExplanationBoiler5", data.ExplanationBoiler5);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler5", data.ScheduledOutageReasonBoiler5);
                        command.Parameters.AddWithValue("@Boiler5ExpectedBackOnlineDate", data.Boiler5ExpectedRepairDate);

                        command.Parameters.AddWithValue("@DownTimeBoiler6", data.DownTimeBoiler6);
                        command.Parameters.AddWithValue("@OutageTypeBoiler6", data.OutageTypeBoiler6);
                        command.Parameters.AddWithValue("@ExplanationBoiler6", data.ExplanationBoiler6);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonBoiler6", data.ScheduledOutageReasonBoiler6);
                        command.Parameters.AddWithValue("@Boiler6ExpectedBackOnlineDate", data.Boiler6ExpectedRepairDate);

                        command.Parameters.AddWithValue("@OutageTypeTurbGen1", data.OutageTypeTurbGen1);
                        command.Parameters.AddWithValue("@DownTimeTurbGen1", data.DownTimeTurbGen1);
                        command.Parameters.AddWithValue("@ExplanationTurbGen1", data.ExplanationTurbGen1);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonTurbGen1", data.ScheduledOutageReasonTurbGen1);
                        command.Parameters.AddWithValue("@TurbGen1ExpectedBackOnlineDate", data.TurbGen1ExpectedRepairDate);
                        
                        command.Parameters.AddWithValue("@OutageTypeTurbGen2", data.OutageTypeTurbGen2);
                        command.Parameters.AddWithValue("@DownTimeTurbGen2", data.DownTimeTurbGen2);
                        command.Parameters.AddWithValue("@ExplanationTurbGen2", data.ExplanationTurbGen2);
                        command.Parameters.AddWithValue("@ScheduledOutageReasonTurbGen2", data.ScheduledOutageReasonTurbGen2);
                        command.Parameters.AddWithValue("@TurbGen2ExpectedBackOnlineDate", data.TurbGen2ExpectedRepairDate);
                        
                        command.Parameters.AddWithValue("@FerrousTons", data.FerrousTons);
                        command.Parameters.AddWithValue("@NonFerrousTons", data.NonFerrousTons);

                        command.Parameters.AddWithValue("@FerrousSystemHoursUnavailable", data.FerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@FerrousSystemHoursUnavailableReason", data.FerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@FerrousSystemExpectedBackOnlineDate", data.FerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughFerrousSystem", data.WasAshReprocessedThroughFerrousSystem);
                        command.Parameters.AddWithValue("@WasFerrousSystem100PercentAvailable", data.WasFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@NonFerrousSystemHoursUnavailable", data.NonFerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@NonFerrousSystemHoursUnavailableReason", data.NonFerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@NonFerrousSystemExpectedBackOnlineDate", data.NonFerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughNonFerrousSystem", data.WasAshReprocessedThroughNonFerrousSystem);
                        command.Parameters.AddWithValue("@WasNonFerrousSystem100PercentAvailable", data.WasNonFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@NonFerrousSmallsSystemHoursUnavailable", data.NonFerrousSmallsSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@NonFerrousSmallsSystemHoursUnavailableReason", data.NonFerrousSmallsSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@NonFerrousSmallsSystemExpectedBackOnlineDate", data.NonFerrousSmallsSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughNonFerrousSmallsSystem", data.WasAshReprocessedThroughNonFerrousSmallsSystem);
                        command.Parameters.AddWithValue("@WasNonFerrousSmallsSystem100PercentAvailable", data.WasNonFerrousSmallsSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@FrontEndFerrousSystemHoursUnavailable", data.FrontEndFerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@FrontEndFerrousSystemHoursUnavailableReason", data.FrontEndFerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@FrontEndFerrousSystemExpectedBackOnlineDate", data.FrontEndFerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughFrontEndFerrousSystem", data.WasAshReprocessedThroughFrontEndFerrousSystem);
                        command.Parameters.AddWithValue("@WasFrontEndFerrousSystem100PercentAvailable", data.WasFrontEndFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@EnhancedFerrousSystemHoursUnavailable", data.EnhancedFerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@EnhancedFerrousSystemHoursUnavailableReason", data.EnhancedFerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@EnhancedFerrousSystemExpectedBackOnlineDate", data.EnhancedFerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughEnhancedFerrousSystem", data.WasAshReprocessedThroughEnhancedFerrousSystem);
                        command.Parameters.AddWithValue("@WasEnhancedFerrousSystem100PercentAvailable", data.WasEnhancedFerrousSystem100PercentAvailable);

                        command.Parameters.AddWithValue("@EnhancedNonFerrousSystemHoursUnavailable", data.EnhancedNonFerrousSystemHoursUnavailable);
                        command.Parameters.AddWithValue("@EnhancedNonFerrousSystemHoursUnavailableReason", data.EnhancedNonFerrousSystemHoursUnavailableReason);
                        command.Parameters.AddWithValue("@EnhancedNonFerrousSystemExpectedBackOnlineDate", data.EnhancedNonFerrousSystemExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@WasAshReprocessedThroughEnhancedNonFerrousSystem", data.WasAshReprocessedThroughEnhancedNonFerrousSystem);
                        command.Parameters.AddWithValue("@WasEnhancedNonFerrousSystem100PercentAvailable", data.WasEnhancedNonFerrousSystem100PercentAvailable);


                        command.Parameters.AddWithValue("@FireSystemOutOfService", data.FireSystemOutOfService);
                        command.Parameters.AddWithValue("@FireSystemOutOfServiceExpectedBackOnlineDate", data.FireSystemOutOfServiceExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@CriticalAssetsInAlarm", data.CriticalAssetsInAlarm);


                        if (data.IsEnvironmentalEvents) { command.Parameters.AddWithValue("@isEnvironmentalEvents", "Y"); } else { command.Parameters.AddWithValue("@isEnvironmentalEvents", "N"); }
                        command.Parameters.AddWithValue("@EnvironmentalEventsType", data.EnvironmentalEventsType);
                        command.Parameters.AddWithValue("@EnvironmentalEventsExplanation", data.EnvironmentalEventsExplanation);

                        if (data.IsCEMSEvents) { command.Parameters.AddWithValue("@isCEMSEvents", "Y"); } else { command.Parameters.AddWithValue("@isCEMSEvents", "N"); }
                        command.Parameters.AddWithValue("@CEMSEventsType", data.CEMSEventsType);
                        command.Parameters.AddWithValue("@CEMSEventsExplanation", data.CEMSEventsExplanation);

                        command.Parameters.AddWithValue("@HealthSafetyFirstAid", data.HealthSafetyFirstAid);
                        command.Parameters.AddWithValue("@HealthSafetyOSHAReportable", data.HealthSafetyOSHAReportable);
                        command.Parameters.AddWithValue("@HealthSafetyNearMiss", data.HealthSafetyNearMiss);
                        command.Parameters.AddWithValue("@HealthSafetyContractor", data.HealthSafetyContractor);


                        command.Parameters.AddWithValue("@Comments", data.Comments);
                        command.Parameters.AddWithValue("@ActiveFlag", "Y");
                        command.Parameters.AddWithValue("@DateRowCreated", DateTime.Now);
                        command.Parameters.AddWithValue("@UserRowCreated", data.UserRowCreated);
                        command.Parameters.AddWithValue("@DateLastModified", DateTime.Now);
                        command.Parameters.AddWithValue("@UserLastModified", data.UserRowCreated);
                        command.Parameters.AddWithValue("@CriticalAssetsExpectedBackOnlineDate", data.CriticalAssetsExpectedBackOnlineDate);
                        command.Parameters.AddWithValue("@CriticalEquipmentOOSExpectedBackOnlineDate", data.CriticalEquipmentOOSExpectedBackOnlineDate);

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

        public void UpdateDailyOpsDataWithID(DailyOpsDataWithID data, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_UpdateDailyOpsData;
                        command.Parameters.AddWithValue("@Id", data.ID);
                        command.Parameters.AddWithValue("@TonsDelivered", data.TonsDelivered);
                        command.Parameters.AddWithValue("@TonsProcessed", data.TonsProcessed);
                        command.Parameters.AddWithValue("@SteamProduced", data.SteamProduced);
                        command.Parameters.AddWithValue("@SteamSold", data.SteamSold);
                        command.Parameters.AddWithValue("@NetElectric", data.NetElectric);
                        command.Parameters.AddWithValue("@PitInventory", data.PitInventory);
                        command.Parameters.AddWithValue("@DownTimeBoiler1", data.DownTimeBoiler1);
                        command.Parameters.AddWithValue("@OutageTypeBoiler1", data.OutageTypeBoiler1);
                        command.Parameters.AddWithValue("@OutageTypeBoiler2", data.OutageTypeBoiler2);
                        command.Parameters.AddWithValue("@DownTimeBoiler2", data.DownTimeBoiler2);
                        command.Parameters.AddWithValue("@DownTimeBoiler3", data.DownTimeBoiler3);
                        command.Parameters.AddWithValue("@OutageTypeBoiler3", data.OutageTypeBoiler3);
                        command.Parameters.AddWithValue("@DownTimeBoiler4", data.DownTimeBoiler4);
                        command.Parameters.AddWithValue("@OutageTypeBoiler4", data.OutageTypeBoiler4);
                        command.Parameters.AddWithValue("@DownTimeBoiler5", data.DownTimeBoiler5);
                        command.Parameters.AddWithValue("@OutageTypeBoiler5", data.OutageTypeBoiler5);
                        command.Parameters.AddWithValue("@DownTimeBoiler6", data.DownTimeBoiler6);
                        command.Parameters.AddWithValue("@OutageTypeBoiler6", data.OutageTypeBoiler6);
                        command.Parameters.AddWithValue("@OutageTypeTurbGen1", data.OutageTypeTurbGen1);
                        command.Parameters.AddWithValue("@DownTimeTurbGen1", data.DownTimeTurbGen1);
                        command.Parameters.AddWithValue("@OutageTypeTurbGen2", data.OutageTypeTurbGen2);
                        command.Parameters.AddWithValue("@DownTimeTurbGen2", data.DownTimeTurbGen2);
                        command.Parameters.AddWithValue("@FerrousTons", data.FerrousTons);
                        command.Parameters.AddWithValue("@NonFerrousTons", data.NonFerrousTons);
                        command.Parameters.AddWithValue("@DateLastModified", DateTime.Now);
                        command.Parameters.AddWithValue("@UserLastModified", data.UserLastModified);
                        commandString = populateExceptionData(command);
                        connection.Open();
                        command.ExecuteNonQuery();
                        statusMsg = "Update Success!";
                    }
                }
            }
            catch (Exception ex)
            {
                status = Enums.StatusEnum.Error;
                statusMsg = string.Empty;
                statusMsg = commandString + ex.ToString();
                return;
            }
        }

        /// <summary>
        /// Get a list of reporting dates which have be enterened already in the database for a specific month for a specific Facility.
        /// Also returns a Status Code and a Status Message (If an error occurs)
        /// </summary>
        /// <param name="monthDataRepresents">Date we want data from</param>
        /// <param name="faciltyID">Facility ID we want date from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">Status Enum</param>
        /// <param name="statusMsg">Message containing exception if applicable</param>
        /// <returns>A populated Daily Ops object (or null if an error occurs, or no record found) </returns>
        public List<DateTime> GetDailyOpsReportedDatesListByFacilityAndMonth(DateTime monthDataRepresents, string faciltyID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            status = Enums.StatusEnum.OK;
            statusMsg = string.Empty;
            DataSet DS = new DataSet();
            DataTable tableList = null;

            List<DateTime> dateList = new List<DateTime>();

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GET_GetDailyOpsReportedDatesListByFacilityAndMonth_LIST;
                        command.Parameters.AddWithValue("@date", monthDataRepresents);
                        command.Parameters.AddWithValue("@facilityID", faciltyID);

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
                return dateList;
            }


            // load object from DataTable if at least 1 row returned
            if (tableList.Rows.Count > 0)
            {
                loadReportingDatesData(tableList, ref dateList);
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;
            return dateList;
        }


        /// <summary>
        /// Gets a list of exception report email Recipients 
        /// </summary>
        /// <returns>list of emails</returns>
        public List<string> GetDailyOpsExceptionEmailRecipientsList()
        {
            DataSet DS = new DataSet();
            DataTable tableList = null;
            List<string> emailList = new List<string>();

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
                        command.CommandText = DALSqlStatements.DailyOpsSQL.SQLSP_GET_GetDailyOpsExceptionReportEmail_LIST;

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
                string x = ex.ToString();
                return emailList;
            }


            // load object from DataTable if at least 1 row returned
            if (tableList.Rows.Count > 0)
            {
                foreach (DataRow dr in tableList.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    emailList.Add(bindObj.ToStringValue("Email"));
                }
            }

            //set null reference to uneeded objects
            tableList = null;
            DS = null;
            return emailList;
        }
        
        #endregion

        #region private methods

        /// <summary>
        /// Loads one DailyDataOps object from a datatable
        /// </summary>
        /// <param name="dt">DataTable (should contain only 1 row)</param>
        /// <returns>1 DailyOpsData object</returns>
        private DailyOpsData loadDailyOpsData(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            string facilityID = bindObj.ToStringValue("FacilityID");
            DateTime reportingDate = bindObj.ToDate("ReportingDate");
            decimal tonsDelivered = bindObj.ToDecimal("TonsDelivered");
            decimal tonsProcessed = bindObj.ToDecimal("TonsProcessed");
            decimal steamProduced = bindObj.ToDecimal("SteamProduced");
            decimal steamSold = bindObj.ToDecimal("SteamSold");
            decimal netElectric = bindObj.ToDecimal("NetElectric");

            decimal downTimeBoiler1 = bindObj.ToDecimal("DownTimeBoiler1");
            string outageTypeBoiler1 = bindObj.ToStringValue("OutageTypeBoiler1");
            string explanationBoiler1 = bindObj.ToStringValue("ExplanationBoiler1");
            string scheduledOutageReasonBoiler1 = bindObj.ToStringValue("ScheduledOutageReasonBoiler1");

            decimal downTimeBoiler2 = bindObj.ToDecimal("DownTimeBoiler2");
            string outageTypeBoiler2 = bindObj.ToStringValue("OutageTypeBoiler2");
            string explanationBoiler2 = bindObj.ToStringValue("ExplanationBoiler2");
            string scheduledOutageReasonBoiler2 = bindObj.ToStringValue("ScheduledOutageReasonBoiler2");

            decimal downTimeBoiler3 = bindObj.ToDecimal("DownTimeBoiler3");
            string outageTypeBoiler3 = bindObj.ToStringValue("OutageTypeBoiler3");
            string explanationBoiler3 = bindObj.ToStringValue("ExplanationBoiler3");
            string scheduledOutageReasonBoiler3 = bindObj.ToStringValue("ScheduledOutageReasonBoiler3");

            decimal downTimeBoiler4 = bindObj.ToDecimal("DownTimeBoiler4");
            string outageTypeBoiler4 = bindObj.ToStringValue("OutageTypeBoiler4");
            string explanationBoiler4 = bindObj.ToStringValue("ExplanationBoiler4");
            string scheduledOutageReasonBoiler4 = bindObj.ToStringValue("ScheduledOutageReasonBoiler4");

            decimal downTimeBoiler5 = bindObj.ToDecimal("DownTimeBoiler5");
            string outageTypeBoiler5 = bindObj.ToStringValue("OutageTypeBoiler5");
            string explanationBoiler5 = bindObj.ToStringValue("ExplanationBoiler5");
            string scheduledOutageReasonBoiler5 = bindObj.ToStringValue("ScheduledOutageReasonBoiler5");

            decimal downTimeBoiler6 = bindObj.ToDecimal("DownTimeBoiler6");
            string outageTypeBoiler6 = bindObj.ToStringValue("OutageTypeBoiler6");
            string explanationBoiler6 = bindObj.ToStringValue("ExplanationBoiler6");
            string scheduledOutageReasonBoiler6 = bindObj.ToStringValue("ScheduledOutageReasonBoiler6");


            string outageTypeTurbGen1 = bindObj.ToStringValue("OutageTypeTurbGen1");
            decimal downTimeTurbGen1 = bindObj.ToDecimal("DownTimeTurbGen1");
            string explanationTurbGen1 = bindObj.ToStringValue("ExplanationTurbGen1");
            string scheduledOutageReasonTurbGen1 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen1");

            string outageTypeTurbGen2 = bindObj.ToStringValue("OutageTypeTurbGen2");
            decimal downTimeTurbGen2 = bindObj.ToDecimal("DownTimeTurbGen2");
            string explanationTurbGen2 = bindObj.ToStringValue("ExplanationTurbGen2");
            string scheduledOutageReasonTurbGen2 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen2");

            decimal ferrousTons = bindObj.ToDecimal("FerrousTons");
            decimal nonFerrousTons = bindObj.ToDecimal("NonFerrousTons");

            decimal ferrousSystemHoursUnavailable = bindObj.ToDecimal("FerrousSystemHoursUnavailable");
            string ferrousSystemHoursUnavailableReason = bindObj.ToStringValue("FerrousSystemHoursUnavailableReason");
            DateTime ferrousSystemExpectedBackOnlineDate = bindObj.ToDate("FerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFerrousSystem");
            string wasFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFerrousSystem100PercentAvailable");

            decimal nonFerrousSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSystemHoursUnavailable");
            string nonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSystemHoursUnavailableReason");
            DateTime nonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSystem");
            string wasNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSystem100PercentAvailable");

            decimal nonFerrousSmallsSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSmallsSystemHoursUnavailable");
            string nonFerrousSmallsSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSmallsSystemHoursUnavailableReason");
            DateTime nonFerrousSmallsSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSmallsSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughNonFerrousSmallsSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSmallsSystem");
            string wasNonFerrousSmallsSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSmallsSystem100PercentAvailable");

            decimal frontEndFerrousSystemHoursUnavailable = bindObj.ToDecimal("FrontEndFerrousSystemHoursUnavailable");
            string frontEndFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("FrontEndFerrousSystemHoursUnavailableReason");
            DateTime frontEndFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("FrontEndFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughFrontEndFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFrontEndFerrousSystem");
            string wasFrontEndFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFrontEndFerrousSystem100PercentAvailable");

            decimal enhancedFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedFerrousSystemHoursUnavailable");
            string enhancedFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedFerrousSystemHoursUnavailableReason");
            DateTime enhancedFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughEnhancedFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedFerrousSystem");
            string wasEnhancedFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedFerrousSystem100PercentAvailable");

            decimal enhancedNonFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedNonFerrousSystemHoursUnavailable");
            string enhancedNonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedNonFerrousSystemHoursUnavailableReason");
            DateTime enhancedNonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedNonFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughenhancedNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedNonFerrousSystem");
            string wasEnhancedNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedNonFerrousSystem100PercentAvailable");

            string fireSystemOutOfService = bindObj.ToStringValue("FireSystemOutOfService");
            DateTime fireSystemOutOfServiceExpectedBackOnlineDate = bindObj.ToDate("FireSystemOutOfServiceExpectedBackOnlineDate");

            string criticalAssetsInAlarm = bindObj.ToStringValue("CriticalAssetsInAlarm");

            bool isEnvironmentalEvents = false;
            if (bindObj.ToStringValue("isEnvironmentalEvents") == "Y") { isEnvironmentalEvents = true; }
            string environmentalEventsType = bindObj.ToStringValue("EnvironmentalEventsType");
            string environmentalEventsExplanation = bindObj.ToStringValue("EnvironmentalEventsExplanation");

            bool isCEMSEvents = false;
            if (bindObj.ToStringValue("isCEMSEvents") == "Y") { isCEMSEvents = true; }
            string cemsEventsType = bindObj.ToStringValue("CEMSEventsType");
            string cemsEventsExplanation = bindObj.ToStringValue("CEMSEventsExplanation");

            string healthSafetyFirstAid = bindObj.ToStringValue("HealthSafetyFirstAid");
            string healthSafetyOSHAReportable = bindObj.ToStringValue("HealthSafetyOSHAReportable");
            string healthSafetyNearMiss = bindObj.ToStringValue("HealthSafetyNearMiss");
            string healthSafetyContractor = bindObj.ToStringValue("HealthSafetyContractor");

            string comments = bindObj.ToStringValue("Comments");
            string userRowCreated = bindObj.ToStringValue("UserRowCreated");

            decimal pitInventory = bindObj.ToDecimal("PitInventory");
            decimal preShredInventory = bindObj.ToDecimal("PreShredInventory");
            decimal postShredInventory = bindObj.ToDecimal("PostShredInventory");

            DateTime Boiler1ExpectedRepairDate = bindObj.ToDate("Boiler1ExpectedBackOnlineDate");
            DateTime Boiler2ExpectedRepairDate = bindObj.ToDate("Boiler2ExpectedBackOnlineDate");
            DateTime Boiler3ExpectedRepairDate = bindObj.ToDate("Boiler3ExpectedBackOnlineDate");
            DateTime Boiler4ExpectedRepairDate = bindObj.ToDate("Boiler4ExpectedBackOnlineDate");
            DateTime Boiler5ExpectedRepairDate = bindObj.ToDate("Boiler5ExpectedBackOnlineDate");
            DateTime Boiler6ExpectedRepairDate = bindObj.ToDate("Boiler6ExpectedBackOnlineDate");
            DateTime TurbGen1ExpectedRepairDate = bindObj.ToDate("TurbGen1ExpectedBackOnlineDate");
            DateTime TurbGen2ExpectedRepairDate = bindObj.ToDate("TurbGen2ExpectedBackOnlineDate");
            DateTime CriticalAssetsExpectedBackOnlineDate = bindObj.ToDate("CriticalAssetsExpectedBackOnlineDate");
            DateTime CriticalEquipmentOOSExpectedBackOnlineDate = bindObj.ToDate("CriticalEquipmentOOSExpectedBackOnlineDate");

            DailyOpsData obj = new DailyOpsData(facilityID, reportingDate, tonsDelivered, tonsProcessed, steamProduced, steamSold, netElectric,
                outageTypeBoiler1, downTimeBoiler1, explanationBoiler1,Boiler1ExpectedRepairDate,
                outageTypeBoiler2, downTimeBoiler2, explanationBoiler2,Boiler2ExpectedRepairDate,
                outageTypeBoiler3, downTimeBoiler3, explanationBoiler3,Boiler3ExpectedRepairDate,
                outageTypeBoiler4, downTimeBoiler4, explanationBoiler4,Boiler4ExpectedRepairDate,
                outageTypeBoiler5, downTimeBoiler5, explanationBoiler5,Boiler5ExpectedRepairDate,
                outageTypeBoiler6, downTimeBoiler6, explanationBoiler6,Boiler6ExpectedRepairDate,
                outageTypeTurbGen1, downTimeTurbGen1, explanationTurbGen1,TurbGen1ExpectedRepairDate,
                outageTypeTurbGen2, downTimeTurbGen2, explanationTurbGen2, TurbGen2ExpectedRepairDate,
                ferrousTons, nonFerrousTons,
                ferrousSystemHoursUnavailable, ferrousSystemHoursUnavailableReason, ferrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFerrousSystem, wasFerrousSystem100PercentAvailable,
                nonFerrousSystemHoursUnavailable, nonFerrousSystemHoursUnavailableReason, nonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSystem, wasNonFerrousSystem100PercentAvailable,
                nonFerrousSmallsSystemHoursUnavailable, nonFerrousSmallsSystemHoursUnavailableReason, nonFerrousSmallsSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSmallsSystem, wasNonFerrousSmallsSystem100PercentAvailable,
                frontEndFerrousSystemHoursUnavailable, frontEndFerrousSystemHoursUnavailableReason, frontEndFerrousSystemExpectedBackOnlineDate,wasAshReprocessedThroughFrontEndFerrousSystem, wasFrontEndFerrousSystem100PercentAvailable,
                enhancedFerrousSystemHoursUnavailable,enhancedFerrousSystemHoursUnavailableReason,enhancedFerrousSystemExpectedBackOnlineDate,wasAshReprocessedThroughEnhancedFerrousSystem,wasEnhancedFerrousSystem100PercentAvailable,
                enhancedNonFerrousSystemHoursUnavailable,enhancedNonFerrousSystemHoursUnavailableReason,enhancedNonFerrousSystemExpectedBackOnlineDate,wasAshReprocessedThroughenhancedNonFerrousSystem,wasEnhancedNonFerrousSystem100PercentAvailable,
                fireSystemOutOfService, fireSystemOutOfServiceExpectedBackOnlineDate, criticalAssetsInAlarm,
                isEnvironmentalEvents, environmentalEventsType, environmentalEventsExplanation,
                isCEMSEvents, cemsEventsType, cemsEventsExplanation,
                healthSafetyFirstAid, healthSafetyOSHAReportable, healthSafetyNearMiss, healthSafetyContractor,
                comments, userRowCreated, pitInventory, CriticalAssetsExpectedBackOnlineDate, CriticalEquipmentOOSExpectedBackOnlineDate, preShredInventory, postShredInventory);

            // these fields are not in the constructor so we set them here.           
            obj.DateLastModified = bindObj.ToDate("DateLastModified");
            obj.ScheduledOutageReasonBoiler1 = scheduledOutageReasonBoiler1;
            obj.ScheduledOutageReasonBoiler2 = scheduledOutageReasonBoiler2;
            obj.ScheduledOutageReasonBoiler3 = scheduledOutageReasonBoiler3;
            obj.ScheduledOutageReasonBoiler4 = scheduledOutageReasonBoiler4;
            obj.ScheduledOutageReasonBoiler5 = scheduledOutageReasonBoiler5;
            obj.ScheduledOutageReasonBoiler6 = scheduledOutageReasonBoiler6;
            obj.ScheduledOutageReasonTurbGen1 = scheduledOutageReasonTurbGen1;
            obj.ScheduledOutageReasonTurbGen2 = scheduledOutageReasonTurbGen2;

            return obj;

        }

        private DailyOpsDataWithID loadDailyOpsDataWithID(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

            int Id = bindObj.ToInteger("ID");
            string facilityID = bindObj.ToStringValue("FacilityID");
            DateTime reportingDate = bindObj.ToDate("ReportingDate");
            decimal tonsDelivered = bindObj.ToDecimal("TonsDelivered");
            decimal tonsProcessed = bindObj.ToDecimal("TonsProcessed");
            decimal steamProduced = bindObj.ToDecimal("SteamProduced");
            decimal steamSold = bindObj.ToDecimal("SteamSold");
            decimal netElectric = bindObj.ToDecimal("NetElectric");

            decimal downTimeBoiler1 = bindObj.ToDecimal("DownTimeBoiler1");
            string outageTypeBoiler1 = bindObj.ToStringValue("OutageTypeBoiler1");
            string explanationBoiler1 = bindObj.ToStringValue("ExplanationBoiler1");
            string scheduledOutageReasonBoiler1 = bindObj.ToStringValue("ScheduledOutageReasonBoiler1");

            decimal downTimeBoiler2 = bindObj.ToDecimal("DownTimeBoiler2");
            string outageTypeBoiler2 = bindObj.ToStringValue("OutageTypeBoiler2");
            string explanationBoiler2 = bindObj.ToStringValue("ExplanationBoiler2");
            string scheduledOutageReasonBoiler2 = bindObj.ToStringValue("ScheduledOutageReasonBoiler2");

            decimal downTimeBoiler3 = bindObj.ToDecimal("DownTimeBoiler3");
            string outageTypeBoiler3 = bindObj.ToStringValue("OutageTypeBoiler3");
            string explanationBoiler3 = bindObj.ToStringValue("ExplanationBoiler3");
            string scheduledOutageReasonBoiler3 = bindObj.ToStringValue("ScheduledOutageReasonBoiler3");

            decimal downTimeBoiler4 = bindObj.ToDecimal("DownTimeBoiler4");
            string outageTypeBoiler4 = bindObj.ToStringValue("OutageTypeBoiler4");
            string explanationBoiler4 = bindObj.ToStringValue("ExplanationBoiler4");
            string scheduledOutageReasonBoiler4 = bindObj.ToStringValue("ScheduledOutageReasonBoiler4");

            decimal downTimeBoiler5 = bindObj.ToDecimal("DownTimeBoiler5");
            string outageTypeBoiler5 = bindObj.ToStringValue("OutageTypeBoiler5");
            string explanationBoiler5 = bindObj.ToStringValue("ExplanationBoiler5");
            string scheduledOutageReasonBoiler5 = bindObj.ToStringValue("ScheduledOutageReasonBoiler5");

            decimal downTimeBoiler6 = bindObj.ToDecimal("DownTimeBoiler6");
            string outageTypeBoiler6 = bindObj.ToStringValue("OutageTypeBoiler6");
            string explanationBoiler6 = bindObj.ToStringValue("ExplanationBoiler6");
            string scheduledOutageReasonBoiler6 = bindObj.ToStringValue("ScheduledOutageReasonBoiler6");


            string outageTypeTurbGen1 = bindObj.ToStringValue("OutageTypeTurbGen1");
            decimal downTimeTurbGen1 = bindObj.ToDecimal("DownTimeTurbGen1");
            string explanationTurbGen1 = bindObj.ToStringValue("ExplanationTurbGen1");
            string scheduledOutageReasonTurbGen1 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen1");

            string outageTypeTurbGen2 = bindObj.ToStringValue("OutageTypeTurbGen2");
            decimal downTimeTurbGen2 = bindObj.ToDecimal("DownTimeTurbGen2");
            string explanationTurbGen2 = bindObj.ToStringValue("ExplanationTurbGen2");
            string scheduledOutageReasonTurbGen2 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen2");

            decimal ferrousTons = bindObj.ToDecimal("FerrousTons");
            decimal nonFerrousTons = bindObj.ToDecimal("NonFerrousTons");

            decimal ferrousSystemHoursUnavailable = bindObj.ToDecimal("FerrousSystemHoursUnavailable");
            string ferrousSystemHoursUnavailableReason = bindObj.ToStringValue("FerrousSystemHoursUnavailableReason");
            DateTime ferrousSystemExpectedBackOnlineDate = bindObj.ToDate("FerrousSystemExpectedBackOnlineDate");

            string wasAshReprocessedThroughFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFerrousSystem");
            string wasFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFerrousSystem100PercentAvailable");

            decimal nonFerrousSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSystemHoursUnavailable");
            string nonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSystemHoursUnavailableReason");
            DateTime nonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSystem");
            string wasNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSystem100PercentAvailable");

            decimal nonFerrousSmallsSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSmallsSystemHoursUnavailable");
            string nonFerrousSmallsSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSmallsSystemHoursUnavailableReason");
            DateTime nonFerrousSmallsSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSmallsSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughNonFerrousSmallsSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSmallsSystem");
            string wasNonFerrousSmallsSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSmallsSystem100PercentAvailable");

            decimal frontEndFerrousSystemHoursUnavailable = bindObj.ToDecimal("FrontEndFerrousSystemHoursUnavailable");
            string frontEndFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("FrontEndFerrousSystemHoursUnavailableReason");
            DateTime frontEndFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("FrontEndFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughFrontEndFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFrontEndFerrousSystem");
            string wasFrontEndFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFrontEndFerrousSystem100PercentAvailable");

            decimal enhancedFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedFerrousSystemHoursUnavailable");
            string enhancedFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedFerrousSystemHoursUnavailableReason");
            DateTime enhancedFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughEnhancedFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedFerrousSystem");
            string wasEnhancedFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedFerrousSystem100PercentAvailable");

            decimal enhancedNonFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedNonFerrousSystemHoursUnavailable");
            string enhancedNonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedNonFerrousSystemHoursUnavailableReason");
            DateTime enhancedNonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedNonFerrousSystemExpectedBackOnlineDate");
            string wasAshReprocessedThroughEnhancedNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedNonFerrousSystem");
            string wasEnhancedNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedNonFerrousSystem100PercentAvailable");

            string fireSystemOutOfService = bindObj.ToStringValue("FireSystemOutOfService");
            DateTime fireSystemOutOfServiceExpectedBackOnlineDate = bindObj.ToDate("FireSystemOutOfServiceExpectedBackOnlineDate");

            string criticalAssetsInAlarm = bindObj.ToStringValue("CriticalAssetsInAlarm");

            bool isEnvironmentalEvents = false;
            if (bindObj.ToStringValue("isEnvironmentalEvents") == "Y") { isEnvironmentalEvents = true; }
            string environmentalEventsType = bindObj.ToStringValue("EnvironmentalEventsType");
            string environmentalEventsExplanation = bindObj.ToStringValue("EnvironmentalEventsExplanation");

            bool isCEMSEvents = false;
            if (bindObj.ToStringValue("isCEMSEvents") == "Y") { isCEMSEvents = true; }
            string cemsEventsType = bindObj.ToStringValue("CEMSEventsType");
            string cemsEventsExplanation = bindObj.ToStringValue("CEMSEventsExplanation");

            string healthSafetyFirstAid = bindObj.ToStringValue("HealthSafetyFirstAid");
            string healthSafetyOSHAReportable = bindObj.ToStringValue("HealthSafetyOSHAReportable");
            string healthSafetyNearMiss = bindObj.ToStringValue("HealthSafetyNearMiss");
            string healthSafetyContractor = bindObj.ToStringValue("HealthSafetyContractor");

            string comments = bindObj.ToStringValue("Comments");
            string userRowCreated = bindObj.ToStringValue("UserRowCreated");
            string userLastModified = bindObj.ToStringValue("UserLastModified");

            decimal pitInventory = bindObj.ToDecimal("PitInventory");


            DailyOpsDataWithID obj = new DailyOpsDataWithID(Id, facilityID, reportingDate, tonsDelivered, tonsProcessed, steamProduced, steamSold, netElectric,
                outageTypeBoiler1, downTimeBoiler1, explanationBoiler1,
                outageTypeBoiler2, downTimeBoiler2, explanationBoiler2,
                outageTypeBoiler3, downTimeBoiler3, explanationBoiler3,
                outageTypeBoiler4, downTimeBoiler4, explanationBoiler4,
                outageTypeBoiler5, downTimeBoiler5, explanationBoiler5,
                outageTypeBoiler6, downTimeBoiler6, explanationBoiler6,
                outageTypeTurbGen1, downTimeTurbGen1, explanationTurbGen1,
                outageTypeTurbGen2, downTimeTurbGen2, explanationTurbGen2,
                ferrousTons, nonFerrousTons,
                ferrousSystemHoursUnavailable, ferrousSystemHoursUnavailableReason, ferrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFerrousSystem, wasFerrousSystem100PercentAvailable,
                nonFerrousSystemHoursUnavailable, nonFerrousSystemHoursUnavailableReason, nonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSystem, wasNonFerrousSystem100PercentAvailable,
                nonFerrousSmallsSystemHoursUnavailable, nonFerrousSmallsSystemHoursUnavailableReason, nonFerrousSmallsSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSmallsSystem, wasNonFerrousSmallsSystem100PercentAvailable,
                frontEndFerrousSystemHoursUnavailable, frontEndFerrousSystemHoursUnavailableReason, frontEndFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFrontEndFerrousSystem, wasFrontEndFerrousSystem100PercentAvailable,
                enhancedFerrousSystemHoursUnavailable, enhancedFerrousSystemHoursUnavailableReason, enhancedFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughEnhancedFerrousSystem, wasEnhancedFerrousSystem100PercentAvailable,
                enhancedNonFerrousSystemHoursUnavailable, enhancedNonFerrousSystemHoursUnavailableReason, enhancedNonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughEnhancedNonFerrousSystem, wasEnhancedNonFerrousSystem100PercentAvailable,
                fireSystemOutOfService, fireSystemOutOfServiceExpectedBackOnlineDate, criticalAssetsInAlarm,
                isEnvironmentalEvents, environmentalEventsType, environmentalEventsExplanation,
                isCEMSEvents, cemsEventsType, cemsEventsExplanation,
                healthSafetyFirstAid, healthSafetyOSHAReportable, healthSafetyNearMiss, healthSafetyContractor,
                comments, userRowCreated, userLastModified, pitInventory);

            // these fields are not in the constructor so we set them here.           
            obj.DateLastModified = bindObj.ToDate("DateLastModified");
            obj.ScheduledOutageReasonBoiler1 = scheduledOutageReasonBoiler1;
            obj.ScheduledOutageReasonBoiler2 = scheduledOutageReasonBoiler2;
            obj.ScheduledOutageReasonBoiler3 = scheduledOutageReasonBoiler3;
            obj.ScheduledOutageReasonBoiler4 = scheduledOutageReasonBoiler4;
            obj.ScheduledOutageReasonBoiler5 = scheduledOutageReasonBoiler5;
            obj.ScheduledOutageReasonBoiler6 = scheduledOutageReasonBoiler6;
            obj.ScheduledOutageReasonTurbGen1 = scheduledOutageReasonTurbGen1;
            obj.ScheduledOutageReasonTurbGen2 = scheduledOutageReasonTurbGen2;

            return obj;

        }

        private List<DailyOpsData> loadDailyOpsDataByMonth(DataTable dt)
        {
            List<DailyOpsData> dailyOPsDataList = new List<DailyOpsData>();
            foreach (DataRow dr in dt.Rows)
            {

                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                string facilityID = bindObj.ToStringValue("FacilityID");
                DateTime reportingDate = bindObj.ToDate("ReportingDate");
                decimal tonsDelivered = bindObj.ToDecimal("TonsDelivered");
                decimal tonsProcessed = bindObj.ToDecimal("TonsProcessed");
                decimal steamProduced = bindObj.ToDecimal("SteamProduced");
                decimal steamSold = bindObj.ToDecimal("SteamSold");
                decimal netElectric = bindObj.ToDecimal("NetElectric");

                decimal downTimeBoiler1 = bindObj.ToDecimal("DownTimeBoiler1");
                string outageTypeBoiler1 = bindObj.ToStringValue("OutageTypeBoiler1");
                string explanationBoiler1 = bindObj.ToStringValue("ExplanationBoiler1");
                string scheduledOutageReasonBoiler1 = bindObj.ToStringValue("ScheduledOutageReasonBoiler1");
                DateTime boiler1ExpectedBackOnlineDate = bindObj.ToDate("Boiler1ExpectedBackOnlineDate");

                decimal downTimeBoiler2 = bindObj.ToDecimal("DownTimeBoiler2");
                string outageTypeBoiler2 = bindObj.ToStringValue("OutageTypeBoiler2");
                string explanationBoiler2 = bindObj.ToStringValue("ExplanationBoiler2");
                string scheduledOutageReasonBoiler2 = bindObj.ToStringValue("ScheduledOutageReasonBoiler2");
                DateTime boiler2ExpectedBackOnlineDate = bindObj.ToDate("Boiler2ExpectedBackOnlineDate");

                decimal downTimeBoiler3 = bindObj.ToDecimal("DownTimeBoiler3");
                string outageTypeBoiler3 = bindObj.ToStringValue("OutageTypeBoiler3");
                string explanationBoiler3 = bindObj.ToStringValue("ExplanationBoiler3");
                string scheduledOutageReasonBoiler3 = bindObj.ToStringValue("ScheduledOutageReasonBoiler3");
                DateTime boiler3ExpectedBackOnlineDate = bindObj.ToDate("Boiler3ExpectedBackOnlineDate");


                decimal downTimeBoiler4 = bindObj.ToDecimal("DownTimeBoiler4");
                string outageTypeBoiler4 = bindObj.ToStringValue("OutageTypeBoiler4");
                string explanationBoiler4 = bindObj.ToStringValue("ExplanationBoiler4");
                string scheduledOutageReasonBoiler4 = bindObj.ToStringValue("ScheduledOutageReasonBoiler4");
                DateTime boiler4ExpectedBackOnlineDate = bindObj.ToDate("Boiler4ExpectedBackOnlineDate");


                decimal downTimeBoiler5 = bindObj.ToDecimal("DownTimeBoiler5");
                string outageTypeBoiler5 = bindObj.ToStringValue("OutageTypeBoiler5");
                string explanationBoiler5 = bindObj.ToStringValue("ExplanationBoiler5");
                string scheduledOutageReasonBoiler5 = bindObj.ToStringValue("ScheduledOutageReasonBoiler5");
                DateTime boiler5ExpectedBackOnlineDate = bindObj.ToDate("Boiler5ExpectedBackOnlineDate");


                decimal downTimeBoiler6 = bindObj.ToDecimal("DownTimeBoiler6");
                string outageTypeBoiler6 = bindObj.ToStringValue("OutageTypeBoiler6");
                string explanationBoiler6 = bindObj.ToStringValue("ExplanationBoiler6");
                string scheduledOutageReasonBoiler6 = bindObj.ToStringValue("ScheduledOutageReasonBoiler6");
                DateTime boiler6ExpectedBackOnlineDate = bindObj.ToDate("Boiler6ExpectedBackOnlineDate");



                string outageTypeTurbGen1 = bindObj.ToStringValue("OutageTypeTurbGen1");
                decimal downTimeTurbGen1 = bindObj.ToDecimal("DownTimeTurbGen1");
                string explanationTurbGen1 = bindObj.ToStringValue("ExplanationTurbGen1");
                string scheduledOutageReasonTurbGen1 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen1");
                DateTime turbGen1ExpectedBackOnlineDate = bindObj.ToDate("TurbGen1ExpectedBackOnlineDate");


                string outageTypeTurbGen2 = bindObj.ToStringValue("OutageTypeTurbGen2");
                decimal downTimeTurbGen2 = bindObj.ToDecimal("DownTimeTurbGen2");
                string explanationTurbGen2 = bindObj.ToStringValue("ExplanationTurbGen2");
                string scheduledOutageReasonTurbGen2 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen2");
                DateTime turbGen2ExpectedBackOnlineDate = bindObj.ToDate("TurbGen2ExpectedBackOnlineDate");

                decimal ferrousTons = bindObj.ToDecimal("FerrousTons");
                decimal nonFerrousTons = bindObj.ToDecimal("NonFerrousTons");

                decimal ferrousSystemHoursUnavailable = bindObj.ToDecimal("FerrousSystemHoursUnavailable");
                string ferrousSystemHoursUnavailableReason = bindObj.ToStringValue("FerrousSystemHoursUnavailableReason");
                DateTime ferrousSystemExpectedBackOnlineDate = bindObj.ToDate("FerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFerrousSystem");
                string wasFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFerrousSystem100PercentAvailable");

                decimal nonFerrousSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSystemHoursUnavailable");
                string nonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSystemHoursUnavailableReason");
                DateTime nonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSystem");
                string wasNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSystem100PercentAvailable");

                decimal nonFerrousSmallsSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSmallsSystemHoursUnavailable");
                string nonFerrousSmallsSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSmallsSystemHoursUnavailableReason");
                DateTime nonFerrousSmallsSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSmallsSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughNonFerrousSmallsSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSmallsSystem");
                string wasNonFerrousSmallsSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSmallsSystem100PercentAvailable");

                decimal frontEndFerrousSystemHoursUnavailable = bindObj.ToDecimal("FrontEndFerrousSystemHoursUnavailable");
                string frontEndFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("FrontEndFerrousSystemHoursUnavailableReason");
                DateTime frontEndFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("FrontEndFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughFrontEndFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFrontEndFerrousSystem");
                string wasFrontEndFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFrontEndFerrousSystem100PercentAvailable");

                decimal enhancedFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedFerrousSystemHoursUnavailable");
                string enhancedFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedFerrousSystemHoursUnavailableReason");
                DateTime enhancedFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughEnhancedFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedFerrousSystem");
                string wasEnhancedFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedFerrousSystem100PercentAvailable");

                decimal enhancedNonFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedNonFerrousSystemHoursUnavailable");
                string enhancedNonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedNonFerrousSystemHoursUnavailableReason");
                DateTime enhancedNonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedNonFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughEnhancedNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedNonFerrousSystem");
                string wasEnhancedNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedNonFerrousSystem100PercentAvailable");

                string fireSystemOutOfService = bindObj.ToStringValue("FireSystemOutOfService");
                DateTime fireSystemOutOfServiceExpectedBackOnlineDate = bindObj.ToDate("FireSystemOutOfServiceExpectedBackOnlineDate");

                string criticalAssetsInAlarm = bindObj.ToStringValue("CriticalAssetsInAlarm");

                bool isEnvironmentalEvents = false;
                if (bindObj.ToStringValue("isEnvironmentalEvents") == "Y") { isEnvironmentalEvents = true; }
                string environmentalEventsType = bindObj.ToStringValue("EnvironmentalEventsType");
                string environmentalEventsExplanation = bindObj.ToStringValue("EnvironmentalEventsExplanation");

                bool isCEMSEvents = false;
                if (bindObj.ToStringValue("isCEMSEvents") == "Y") { isCEMSEvents = true; }
                string cemsEventsType = bindObj.ToStringValue("CEMSEventsType");
                string cemsEventsExplanation = bindObj.ToStringValue("CEMSEventsExplanation");

                string healthSafetyFirstAid = bindObj.ToStringValue("HealthSafetyFirstAid");
                string healthSafetyOSHAReportable = bindObj.ToStringValue("HealthSafetyOSHAReportable");
                string healthSafetyNearMiss = bindObj.ToStringValue("HealthSafetyNearMiss");
                string healthSafetyContractor = bindObj.ToStringValue("HealthSafetyContractor");

                string comments = bindObj.ToStringValue("Comments");
                string userRowCreated = bindObj.ToStringValue("UserRowCreated");

                decimal pitInventory = bindObj.ToDecimal("PitInventory");
                decimal preShredInventory = bindObj.ToDecimal("PreShredInventory");
                decimal postShredInventory = bindObj.ToDecimal("PostShredInventory");
                DateTime CriticalAssetsExpectedBackOnlineDate = bindObj.ToDate("CriticalAssetsExpectedBackOnlineDate");
                DateTime CriticalEquipmentOOSExpectedBackOnlineDate = bindObj.ToDate("CriticalEquipmentOOSExpectedBackOnlineDate");


                DailyOpsData obj = new DailyOpsData(facilityID, reportingDate, tonsDelivered, tonsProcessed, steamProduced, steamSold, netElectric,
                    outageTypeBoiler1, downTimeBoiler1, explanationBoiler1,boiler1ExpectedBackOnlineDate,
                    outageTypeBoiler2, downTimeBoiler2, explanationBoiler2,boiler2ExpectedBackOnlineDate,
                    outageTypeBoiler3, downTimeBoiler3, explanationBoiler3,boiler3ExpectedBackOnlineDate,
                    outageTypeBoiler4, downTimeBoiler4, explanationBoiler4,boiler4ExpectedBackOnlineDate,
                    outageTypeBoiler5, downTimeBoiler5, explanationBoiler5,boiler5ExpectedBackOnlineDate,
                    outageTypeBoiler6, downTimeBoiler6, explanationBoiler6,boiler6ExpectedBackOnlineDate,
                    outageTypeTurbGen1, downTimeTurbGen1, explanationTurbGen1,turbGen1ExpectedBackOnlineDate,
                    outageTypeTurbGen2, downTimeTurbGen2, explanationTurbGen2,turbGen2ExpectedBackOnlineDate,
                    ferrousTons, nonFerrousTons,
                    ferrousSystemHoursUnavailable, ferrousSystemHoursUnavailableReason, ferrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFerrousSystem, wasFerrousSystem100PercentAvailable,
                    nonFerrousSystemHoursUnavailable, nonFerrousSystemHoursUnavailableReason, nonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSystem, wasNonFerrousSystem100PercentAvailable,
                    nonFerrousSmallsSystemHoursUnavailable, nonFerrousSmallsSystemHoursUnavailableReason, nonFerrousSmallsSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSmallsSystem, wasNonFerrousSmallsSystem100PercentAvailable,
                    frontEndFerrousSystemHoursUnavailable, frontEndFerrousSystemHoursUnavailableReason, frontEndFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFrontEndFerrousSystem, wasFrontEndFerrousSystem100PercentAvailable,
                    enhancedFerrousSystemHoursUnavailable, enhancedFerrousSystemHoursUnavailableReason, enhancedFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughEnhancedFerrousSystem, wasEnhancedFerrousSystem100PercentAvailable,
                    enhancedNonFerrousSystemHoursUnavailable, enhancedNonFerrousSystemHoursUnavailableReason, enhancedNonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughEnhancedNonFerrousSystem, wasEnhancedNonFerrousSystem100PercentAvailable,
                    fireSystemOutOfService, fireSystemOutOfServiceExpectedBackOnlineDate, criticalAssetsInAlarm,
                    isEnvironmentalEvents, environmentalEventsType, environmentalEventsExplanation,
                    isCEMSEvents, cemsEventsType, cemsEventsExplanation,
                    healthSafetyFirstAid, healthSafetyOSHAReportable, healthSafetyNearMiss, healthSafetyContractor,
                    comments, userRowCreated, pitInventory, CriticalAssetsExpectedBackOnlineDate, CriticalEquipmentOOSExpectedBackOnlineDate, preShredInventory, postShredInventory);

                // these fields are not in the constructor so we set them here.           
                obj.DateLastModified = bindObj.ToDate("DateLastModified");
                obj.ScheduledOutageReasonBoiler1 = scheduledOutageReasonBoiler1;
                obj.ScheduledOutageReasonBoiler2 = scheduledOutageReasonBoiler2;
                obj.ScheduledOutageReasonBoiler3 = scheduledOutageReasonBoiler3;
                obj.ScheduledOutageReasonBoiler4 = scheduledOutageReasonBoiler4;
                obj.ScheduledOutageReasonBoiler5 = scheduledOutageReasonBoiler5;
                obj.ScheduledOutageReasonBoiler6 = scheduledOutageReasonBoiler6;
                obj.ScheduledOutageReasonTurbGen1 = scheduledOutageReasonTurbGen1;
                obj.ScheduledOutageReasonTurbGen2 = scheduledOutageReasonTurbGen2;
                obj.Boiler1ExpectedRepairDate = boiler1ExpectedBackOnlineDate;
                obj.Boiler2ExpectedRepairDate = boiler2ExpectedBackOnlineDate;
                obj.Boiler3ExpectedRepairDate = boiler3ExpectedBackOnlineDate;
                obj.Boiler4ExpectedRepairDate = boiler4ExpectedBackOnlineDate;
                obj.Boiler5ExpectedRepairDate = boiler5ExpectedBackOnlineDate;
                obj.Boiler6ExpectedRepairDate = boiler6ExpectedBackOnlineDate;
                obj.TurbGen1ExpectedRepairDate = turbGen1ExpectedBackOnlineDate;
                obj.TurbGen2ExpectedRepairDate = turbGen2ExpectedBackOnlineDate;
                

                //return obj;
                dailyOPsDataList.Add(obj);
            }
            return dailyOPsDataList;
            //DataRow dr = dt.Rows[0];
            

        }

        private List<DailyOpsDataWithID> loadDailyOpsDataWithIDByMonth(DataTable dt)
        {
            List<DailyOpsDataWithID> dailyOPsDataWithIDList = new List<DailyOpsDataWithID>();
            foreach (DataRow dr in dt.Rows)
            {

                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                int Id = bindObj.ToInteger("ID");
                string facilityID = bindObj.ToStringValue("FacilityID");
                DateTime reportingDate = bindObj.ToDate("ReportingDate");
                decimal tonsDelivered = bindObj.ToDecimal("TonsDelivered");
                decimal tonsProcessed = bindObj.ToDecimal("TonsProcessed");
                decimal steamProduced = bindObj.ToDecimal("SteamProduced");
                decimal steamSold = bindObj.ToDecimal("SteamSold");
                decimal netElectric = bindObj.ToDecimal("NetElectric");

                decimal downTimeBoiler1 = bindObj.ToDecimal("DownTimeBoiler1");
                string outageTypeBoiler1 = bindObj.ToStringValue("OutageTypeBoiler1");
                string explanationBoiler1 = bindObj.ToStringValue("ExplanationBoiler1");
                string scheduledOutageReasonBoiler1 = bindObj.ToStringValue("ScheduledOutageReasonBoiler1");

                decimal downTimeBoiler2 = bindObj.ToDecimal("DownTimeBoiler2");
                string outageTypeBoiler2 = bindObj.ToStringValue("OutageTypeBoiler2");
                string explanationBoiler2 = bindObj.ToStringValue("ExplanationBoiler2");
                string scheduledOutageReasonBoiler2 = bindObj.ToStringValue("ScheduledOutageReasonBoiler2");

                decimal downTimeBoiler3 = bindObj.ToDecimal("DownTimeBoiler3");
                string outageTypeBoiler3 = bindObj.ToStringValue("OutageTypeBoiler3");
                string explanationBoiler3 = bindObj.ToStringValue("ExplanationBoiler3");
                string scheduledOutageReasonBoiler3 = bindObj.ToStringValue("ScheduledOutageReasonBoiler3");

                decimal downTimeBoiler4 = bindObj.ToDecimal("DownTimeBoiler4");
                string outageTypeBoiler4 = bindObj.ToStringValue("OutageTypeBoiler4");
                string explanationBoiler4 = bindObj.ToStringValue("ExplanationBoiler4");
                string scheduledOutageReasonBoiler4 = bindObj.ToStringValue("ScheduledOutageReasonBoiler4");

                decimal downTimeBoiler5 = bindObj.ToDecimal("DownTimeBoiler5");
                string outageTypeBoiler5 = bindObj.ToStringValue("OutageTypeBoiler5");
                string explanationBoiler5 = bindObj.ToStringValue("ExplanationBoiler5");
                string scheduledOutageReasonBoiler5 = bindObj.ToStringValue("ScheduledOutageReasonBoiler5");

                decimal downTimeBoiler6 = bindObj.ToDecimal("DownTimeBoiler6");
                string outageTypeBoiler6 = bindObj.ToStringValue("OutageTypeBoiler6");
                string explanationBoiler6 = bindObj.ToStringValue("ExplanationBoiler6");
                string scheduledOutageReasonBoiler6 = bindObj.ToStringValue("ScheduledOutageReasonBoiler6");


                string outageTypeTurbGen1 = bindObj.ToStringValue("OutageTypeTurbGen1");
                decimal downTimeTurbGen1 = bindObj.ToDecimal("DownTimeTurbGen1");
                string explanationTurbGen1 = bindObj.ToStringValue("ExplanationTurbGen1");
                string scheduledOutageReasonTurbGen1 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen1");

                string outageTypeTurbGen2 = bindObj.ToStringValue("OutageTypeTurbGen2");
                decimal downTimeTurbGen2 = bindObj.ToDecimal("DownTimeTurbGen2");
                string explanationTurbGen2 = bindObj.ToStringValue("ExplanationTurbGen2");
                string scheduledOutageReasonTurbGen2 = bindObj.ToStringValue("ScheduledOutageReasonTurbGen2");

                decimal ferrousTons = bindObj.ToDecimal("FerrousTons");
                decimal nonFerrousTons = bindObj.ToDecimal("NonFerrousTons");

                decimal ferrousSystemHoursUnavailable = bindObj.ToDecimal("FerrousSystemHoursUnavailable");
                string ferrousSystemHoursUnavailableReason = bindObj.ToStringValue("FerrousSystemHoursUnavailableReason");
                DateTime ferrousSystemExpectedBackOnlineDate = bindObj.ToDate("FerrousSystemExpectedBackOnlineDate");

                string wasAshReprocessedThroughFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFerrousSystem");
                string wasFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFerrousSystem100PercentAvailable");

                decimal nonFerrousSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSystemHoursUnavailable");
                string nonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSystemHoursUnavailableReason");
                DateTime nonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSystem");
                string wasNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSystem100PercentAvailable");

                decimal nonFerrousSmallsSystemHoursUnavailable = bindObj.ToDecimal("NonFerrousSmallsSystemHoursUnavailable");
                string nonFerrousSmallsSystemHoursUnavailableReason = bindObj.ToStringValue("NonFerrousSmallsSystemHoursUnavailableReason");
                DateTime nonFerrousSmallsSystemExpectedBackOnlineDate = bindObj.ToDate("NonFerrousSmallsSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughNonFerrousSmallsSystem = bindObj.ToStringValue("WasAshReprocessedThroughNonFerrousSmallsSystem");
                string wasNonFerrousSmallsSystem100PercentAvailable = bindObj.ToStringValue("WasNonFerrousSmallsSystem100PercentAvailable");

                decimal frontEndFerrousSystemHoursUnavailable = bindObj.ToDecimal("FrontEndFerrousSystemHoursUnavailable");
                string frontEndFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("FrontEndFerrousSystemHoursUnavailableReason");
                DateTime frontEndFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("FrontEndFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughFrontEndFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughFrontEndFerrousSystem");
                string wasFrontEndFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasFrontEndFerrousSystem100PercentAvailable");

                decimal enhancedFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedFerrousSystemHoursUnavailable");
                string enhancedFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedFerrousSystemHoursUnavailableReason");
                DateTime enhancedFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughEnhancedFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedFerrousSystem");
                string wasEnhancedFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedFerrousSystem100PercentAvailable");

                decimal enhancedNonFerrousSystemHoursUnavailable = bindObj.ToDecimal("EnhancedNonFerrousSystemHoursUnavailable");
                string enhancedNonFerrousSystemHoursUnavailableReason = bindObj.ToStringValue("EnhancedNonFerrousSystemHoursUnavailableReason");
                DateTime enhancedNonFerrousSystemExpectedBackOnlineDate = bindObj.ToDate("EnhancedNonFerrousSystemExpectedBackOnlineDate");
                string wasAshReprocessedThroughEnhancedNonFerrousSystem = bindObj.ToStringValue("WasAshReprocessedThroughEnhancedNonFerrousSystem");
                string wasEnhancedNonFerrousSystem100PercentAvailable = bindObj.ToStringValue("WasEnhancedNonFerrousSystem100PercentAvailable");

                string fireSystemOutOfService = bindObj.ToStringValue("FireSystemOutOfService");
                DateTime fireSystemOutOfServiceExpectedBackOnlineDate = bindObj.ToDate("FireSystemOutOfServiceExpectedBackOnlineDate");

                string criticalAssetsInAlarm = bindObj.ToStringValue("CriticalAssetsInAlarm");

                bool isEnvironmentalEvents = false;
                if (bindObj.ToStringValue("isEnvironmentalEvents") == "Y") { isEnvironmentalEvents = true; }
                string environmentalEventsType = bindObj.ToStringValue("EnvironmentalEventsType");
                string environmentalEventsExplanation = bindObj.ToStringValue("EnvironmentalEventsExplanation");

                bool isCEMSEvents = false;
                if (bindObj.ToStringValue("isCEMSEvents") == "Y") { isCEMSEvents = true; }
                string cemsEventsType = bindObj.ToStringValue("CEMSEventsType");
                string cemsEventsExplanation = bindObj.ToStringValue("CEMSEventsExplanation");

                string healthSafetyFirstAid = bindObj.ToStringValue("HealthSafetyFirstAid");
                string healthSafetyOSHAReportable = bindObj.ToStringValue("HealthSafetyOSHAReportable");
                string healthSafetyNearMiss = bindObj.ToStringValue("HealthSafetyNearMiss");
                string healthSafetyContractor = bindObj.ToStringValue("HealthSafetyContractor");

                string comments = bindObj.ToStringValue("Comments");
                string userRowCreated = bindObj.ToStringValue("UserRowCreated");
                string userLastModified = bindObj.ToStringValue("UserLastModified");
                decimal pitInventory = bindObj.ToDecimal("PitInventory");


                DailyOpsDataWithID obj = new DailyOpsDataWithID(Id, facilityID, reportingDate, tonsDelivered, tonsProcessed, steamProduced, steamSold, netElectric,
                    outageTypeBoiler1, downTimeBoiler1, explanationBoiler1,
                    outageTypeBoiler2, downTimeBoiler2, explanationBoiler2,
                    outageTypeBoiler3, downTimeBoiler3, explanationBoiler3,
                    outageTypeBoiler4, downTimeBoiler4, explanationBoiler4,
                    outageTypeBoiler5, downTimeBoiler5, explanationBoiler5,
                    outageTypeBoiler6, downTimeBoiler6, explanationBoiler6,
                    outageTypeTurbGen1, downTimeTurbGen1, explanationTurbGen1,
                    outageTypeTurbGen2, downTimeTurbGen2, explanationTurbGen2,
                    ferrousTons, nonFerrousTons,
                    ferrousSystemHoursUnavailable, ferrousSystemHoursUnavailableReason, ferrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFerrousSystem, wasFerrousSystem100PercentAvailable,
                    nonFerrousSystemHoursUnavailable, nonFerrousSystemHoursUnavailableReason, nonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSystem, wasNonFerrousSystem100PercentAvailable,
                    nonFerrousSmallsSystemHoursUnavailable, nonFerrousSmallsSystemHoursUnavailableReason, nonFerrousSmallsSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSmallsSystem, wasNonFerrousSmallsSystem100PercentAvailable,
                    frontEndFerrousSystemHoursUnavailable, frontEndFerrousSystemHoursUnavailableReason, frontEndFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFrontEndFerrousSystem, wasFrontEndFerrousSystem100PercentAvailable,
                    enhancedFerrousSystemHoursUnavailable, enhancedFerrousSystemHoursUnavailableReason, enhancedFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughEnhancedFerrousSystem, wasEnhancedFerrousSystem100PercentAvailable,
                    enhancedNonFerrousSystemHoursUnavailable, enhancedNonFerrousSystemHoursUnavailableReason, enhancedNonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughEnhancedNonFerrousSystem, wasEnhancedNonFerrousSystem100PercentAvailable,
                    fireSystemOutOfService, fireSystemOutOfServiceExpectedBackOnlineDate, criticalAssetsInAlarm,
                    isEnvironmentalEvents, environmentalEventsType, environmentalEventsExplanation,
                    isCEMSEvents, cemsEventsType, cemsEventsExplanation,
                    healthSafetyFirstAid, healthSafetyOSHAReportable, healthSafetyNearMiss, healthSafetyContractor,
                    comments, userRowCreated, userLastModified, pitInventory);

                // these fields are not in the constructor so we set them here.           
                obj.DateLastModified = bindObj.ToDate("DateLastModified");
                obj.ScheduledOutageReasonBoiler1 = scheduledOutageReasonBoiler1;
                obj.ScheduledOutageReasonBoiler2 = scheduledOutageReasonBoiler2;
                obj.ScheduledOutageReasonBoiler3 = scheduledOutageReasonBoiler3;
                obj.ScheduledOutageReasonBoiler4 = scheduledOutageReasonBoiler4;
                obj.ScheduledOutageReasonBoiler5 = scheduledOutageReasonBoiler5;
                obj.ScheduledOutageReasonBoiler6 = scheduledOutageReasonBoiler6;
                obj.ScheduledOutageReasonTurbGen1 = scheduledOutageReasonTurbGen1;
                obj.ScheduledOutageReasonTurbGen2 = scheduledOutageReasonTurbGen2;

                //return obj;
                dailyOPsDataWithIDList.Add(obj);
            }
            return dailyOPsDataWithIDList;
            //DataRow dr = dt.Rows[0];


        }



        /// <summary>
        /// Load a list of reporting dates which have data in the database for the month specified
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="dateList">list of reporting dates which have data in the database for the month specified</param>
        private void loadReportingDatesData(DataTable dt, ref List<DateTime> dateList)
        {
            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                dateList.Add(bindObj.ToDate("ReportingDate"));
            }
            return;
        }

        private string populateExceptionData(SqlCommand command)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
               
                sb.AppendLine(command.Connection.ConnectionString);
                sb.AppendLine(command.CommandText);
                sb.AppendLine(command.CommandType.ToString());
                foreach (SqlParameter param in command.Parameters)
                {
                    sb.AppendLine(param.ToString() + "  " + param.Value.ToString());
                }
            }
            catch ( Exception ex)
            { }
            return sb.ToString();
        }

        #endregion
    }
}
