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
    /// Performs CRUD functions to the WorkDayPayAccrualETL
    /// </summary>
    public class DALWorkDayPayAccrualETL : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALWorkDayPayAccrualETL(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods


        public List<Workday_MonthlyAccrualDays> GetWorkday_MonthlyAccrualDaysList()
        {
            DataTable dt = new DataTable();
            List<Workday_MonthlyAccrualDays> list = new List<Workday_MonthlyAccrualDays>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.WorkDayPayAccrualETL.SQLSP_WorkDayPayAccrualApp_GetMonthlyAccrualDays;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadWorkday_MonthlyAccrualDaysList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;

            //sort
            //list = (from u in list
            //        orderby u.Description
            //        select u).ToList();

            return list;

        }

        #endregion

        #region private methods

        /// <summary>
        /// Load a list of Workday_MonthlyAccrualDays Unit objects
        /// </summary>
        /// <param name="dt">Datable with Workday_MonthlyAccrualDays</param>
        /// <param name="list">List of Workday_MonthlyAccrualDays objects</param>
        private void loadWorkday_MonthlyAccrualDaysList(DataTable dt, List<Workday_MonthlyAccrualDays> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                Workday_MonthlyAccrualDays obj = new Workday_MonthlyAccrualDays();

                obj.DaysLeftInMonth = bindObj.ToInteger("DaysLeftInMonth");
                obj.LastDayOfMonth = bindObj.ToDate("LastDayOfMonth");
                obj.LastDayOfMonthName = bindObj.ToStringValue("LastDayOfMonthName");
                obj.MaxPeriodEndDateInMonth = bindObj.ToDate("MaxPeriodEndDateInMonth");

                list.Add(obj);
            }
            //sort
            list = (from u in list
                    orderby u.LastDayOfMonth
                    select u).ToList();

            int x = 1;
            foreach (Workday_MonthlyAccrualDays obj in list)
            {
                obj.ID = x;
                x++;
            }
        }
        #endregion
    }
}
