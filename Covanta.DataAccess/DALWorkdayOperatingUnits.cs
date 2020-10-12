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
    /// Performs CRUD functions to the DALWorkdayOperatingUnits
    /// </summary>
    public class DALWorkdayOperatingUnits : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALWorkdayOperatingUnits(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods

        public void DeleteWorkdayOperatingUnitsFromSQLServer()
        {

            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = DALSqlStatements.WorkDayOperatingUnitsETL.SQLSP_Delete_WorkDayOperatingUnits;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertWorkdayOperatingUnitsToSQLServer(object[,] dataArray)
        {
            int arrayLength0 = dataArray.GetUpperBound(0);
            int arrayLength1 = dataArray.GetUpperBound(1);

            //if a cell is null, then replace it with a spaces
            for (int i = 0; i < arrayLength0 + 1; i++)
            {
                for (int j = 0; j < arrayLength1 + 1; j++)
                {
                    if (dataArray[i, j] == null) {                        
                        dataArray[i, j] = string.Empty;
                        string a = dataArray[i, j].ToString();
                    }
                }
            }
                   
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    for (int i = 0; i < arrayLength0 + 1; i++)
                    {
                        //we don't want the title (first row) so don't include it
                        if (dataArray[i, 0].ToString().Length > 5) { continue; }

                        //int j = 0;
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.WorkDayOperatingUnitsETL.SQLSP_Insert_WorkDayOperatingUnits;
                        command.Parameters.AddWithValue("@Division", dataArray[i, 0].ToString());                     
                        command.Parameters.AddWithValue("@EEID", dataArray[i, 1].ToString());
                        command.Parameters.AddWithValue("@Operating_Unit_Division", dataArray[i, 2].ToString());
                        command.Parameters.AddWithValue("@Distribution_Percent", dataArray[i, 3].ToString());
                       
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        #endregion

        #region private methods

        #endregion
    }
}
