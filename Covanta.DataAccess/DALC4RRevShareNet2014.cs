using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Covanta.DataAccess
{
    public class DALC4RRevShareNet2014 : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALC4RRevShareNet2014(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods


        public List<LongviewFactOPRevenue> Get_OPRevenueList()
        {
            DataTable dt = new DataTable();
            List<LongviewFactOPRevenue> list = new List<LongviewFactOPRevenue>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.OPRevenue.SQLSP_NetRevCalc_Get_LongviewOPRevenueReduced_LIST;



            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadLongviewOPRevenue_LIST(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }

        public List<PS_JRNL_NET_REV> Get_PS_JournalNetRevList()
        {
            DataTable dt = new DataTable();
            List<PS_JRNL_NET_REV> list = new List<PS_JRNL_NET_REV>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.PS_JRNL.SQLSP_NetRevCalc_Get_PS_JRNL_NET_Rev44999_LIST;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadPSJournalNetRev_LIST(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }

        //public List<C4RForecastRecExcelInfo> Get_C4RForecastRecExcelInfoList()
        //{
        //    DataTable dt = new DataTable();
        //    List<C4RForecastRecExcelInfo> list = new List<C4RForecastRecExcelInfo>();

        //    //create the connection and command objects
        //    SqlConnection connection = new SqlConnection(_dbConnection);
        //    SqlCommand command = new SqlCommand();

        //    //populate the command object
        //    command.Connection = connection;
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = DALSqlStatements.C4RForecastRecExcel.SQLSP_Get_C4RForecastRecommendationApp_GetAllRows_LIST;

        //    //execute command
        //    dt = executeSQLCommandGetDataTable(command, connection);

        //    //load Lists from DataTable if at least 1 row returned
        //    if (dt.Rows.Count > 0)
        //    {
        //        loadC4RForecastRecExcel_LIST(dt, list);
        //    }

        //    //set null reference to uneeded objects
        //    dt = null;

        //    return list;
        //}

        public void InsertNetRevToDatabase(List<LongviewFactOPRevenue> list)
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            foreach (LongviewFactOPRevenue obj in list)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_dbConnection))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = DALSqlStatements.C4RNetRev.SQLSP_Insert_NetRevValues;
                            command.Parameters.AddWithValue("@Entity", obj.Entity);
                            command.Parameters.AddWithValue("@AccountID", obj.Account);
                            command.Parameters.AddWithValue("@Month1", obj.Month);
                            command.Parameters.AddWithValue("@Year1", obj.Year);
                            command.Parameters.AddWithValue("@Value", obj.Value);
                            command.Parameters.AddWithValue("@PercentOfTotal", obj.PercentOfTotal);
                            command.Parameters.AddWithValue("@NetValue", obj.NetValue);
                            command.Parameters.AddWithValue("@JournalTotal", obj.JournalTotal);


                            commandString = populateExceptionDataBase(command);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public void DeleteNetRevToDatabase()
        {
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
                        command.CommandText = DALSqlStatements.C4RNetRev.SQLSP_Delete_NetRevValues;

                        commandString = populateExceptionDataBase(command);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion

        #region private methods

        //private void loadC4RForecastRecExcel_LIST(DataTable dt, List<C4RForecastRecExcelInfo> list)
        //{
        //    list.Clear();

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

        //        C4RForecastRecExcelInfo obj = new C4RForecastRecExcelInfo();
        //        obj.Entity = bindObj.ToStringValue("FacilityShortName");
        //        obj.Account = bindObj.ToStringValue("AccountID");
        //        obj.Identifier = bindObj.ToStringValue("Identifier");
        //        obj.Month1 = bindObj.ToStringValue("Month1");
        //        obj.RevShare = bindObj.ToStringValue("RevShare");
        //        //obj.SequenceNum = bindObj.ToStringValue("SequenceNum");
        //        obj.Year1 = bindObj.ToStringValue("Year1");
        //        //obj.Budget = bindObj.ToInteger("Budget");
        //        //obj.Forecast = bindObj.ToInteger("Forecast");
        //        //obj.Actual_EST = bindObj.ToInteger("Actual_EST");
        //        //obj.Actual = bindObj.ToInteger("Actual");

        //        list.Add(obj);

        //    }
        //}


        private void loadLongviewOPRevenue_LIST(DataTable dt, List<LongviewFactOPRevenue> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                LongviewFactOPRevenue obj = new LongviewFactOPRevenue();
                obj.Account = bindObj.ToStringValue("Account");
                //obj.Detail1 = bindObj.ToStringValue("Detail1");
                //obj.Detail2 = bindObj.ToStringValue("Detail2");
                obj.Entity = bindObj.ToStringValue("Entity");
                if (obj.Entity == "SECON") { obj.Entity = "TRMSE"; }
                obj.Month = bindObj.ToStringValue("Month");
                obj.Type = bindObj.ToStringValue("Type");
                obj.Year = bindObj.ToStringValue("Year");
                obj.Value = bindObj.ToDouble("Value");

                list.Add(obj);

            }
        }

        private void loadPSJournalNetRev_LIST(DataTable dt, List<PS_JRNL_NET_REV> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                PS_JRNL_NET_REV obj = new PS_JRNL_NET_REV();
                obj.Account = bindObj.ToStringValue("ACCOUNT");
                int x = bindObj.ToInteger("ACCOUNTING_PERIOD");
                if (x == 1) { obj.AccountingPeriod = "01"; }
                if (x == 2) { obj.AccountingPeriod = "02"; }
                if (x == 3) { obj.AccountingPeriod = "03"; }
                if (x == 4) { obj.AccountingPeriod = "04"; }
                if (x == 5) { obj.AccountingPeriod = "05"; }
                if (x == 6) { obj.AccountingPeriod = "06"; }
                if (x == 7) { obj.AccountingPeriod = "07"; }
                if (x == 8) { obj.AccountingPeriod = "08"; }
                if (x == 9) { obj.AccountingPeriod = "09"; }
                if (x == 10) { obj.AccountingPeriod = "10"; }
                if (x == 11) { obj.AccountingPeriod = "11"; }
                if (x == 12) { obj.AccountingPeriod = "12"; }

                //obj.AccountingPeriod = x.ToString();
                obj.Amount = double.Parse(dr["MONETARY_AMOUNT"].ToString());
                obj.Entity = bindObj.ToStringValue("BUSINESS_UNIT");
                if (obj.Entity == "SECON") { obj.Entity = "TRMSE"; }
                //obj.JournalDate = bindObj.ToDate("JOURNAL_DATE");
                obj.Year = dr["FISCAL_YEAR"].ToString();

                list.Add(obj);
            }
        }

        #endregion
    }
}
