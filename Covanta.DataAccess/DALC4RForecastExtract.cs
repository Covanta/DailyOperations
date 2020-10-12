using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALC4RForecastExtract : DALBase
    {
        #region constructors
      
        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALC4RForecastExtract(string dbConnection) : base(dbConnection) { }       

        #endregion

        #region public methods

        /// <summary>
        /// Gets a list of GetC4RForecast Paths And FileName from the Database
        /// </summary>        
        /// <returns>a list of GetC4RForecast Paths And FileNames from the Database</returns>
        public List<C4RForecastExtractSourceFileInfo> GetC4RForecastExtractSourceFileInfoList()
        {
            DataTable dt = new DataTable();
            List<C4RForecastExtractSourceFileInfo> list = new List<C4RForecastExtractSourceFileInfo>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = DALSqlStatements.C4RForecastExtractSQL.SQLSP_GetC4RForecastExtractSourceFileInfo;

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                loadC4RForecastPathsAndFileNamesList(dt, list);
            }

            //set null reference to uneeded objects
            dt = null;          

            return list;

        }

        #endregion

        #region private methods

        /// <summary>
        /// Load a list of C4RForecastPathsAndFileNames
        /// </summary>
        /// <param name="dt">Datable with C4RForecastPathsAndFileNames</param>
        /// <param name="list">List of C4RForecastPathsAndFileNames</param>
        private void loadC4RForecastPathsAndFileNamesList(DataTable dt, List<C4RForecastExtractSourceFileInfo> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                string C4RForecastPath = bindObj.ToStringValue("FilePaths");
                string C4RForecastFileName = bindObj.ToStringValue("FileNames");                

                int rowStartDIM1 = bindObj.ToInteger("DIM1_RowStart");
                int rowEndDIM1 = bindObj.ToInteger("DIM1_RowEnd");
                int rowStartDIM2_part1 = bindObj.ToInteger("DIM2_Part1_RowStart");
                int rowEndDIM2_part1 = bindObj.ToInteger("DIM2_Part1_RowEnd");
                int rowStartDIM2_part2 = bindObj.ToInteger("DIM2_Part2_RowStart");
                int rowEndDIM2_part2 = bindObj.ToInteger("DIM2_Part2_RowEnd");
                int rowStartDIM2_part3 = bindObj.ToInteger("DIM2_Part3_RowStart");
                int rowEndDIM2_part3 = bindObj.ToInteger("DIM2_Part3_RowEnd");
                int rowStartDIM2_part4 = bindObj.ToInteger("DIM2_Part4_RowStart");
                int rowEndDIM2_part4 = bindObj.ToInteger("DIM2_Part4_RowEnd");
                C4RForecastExtractSourceFileInfo obj = new C4RForecastExtractSourceFileInfo(C4RForecastPath, C4RForecastFileName, rowStartDIM1, rowEndDIM1, rowStartDIM2_part1, rowEndDIM2_part1, rowStartDIM2_part2, rowEndDIM2_part2, rowStartDIM2_part3, rowEndDIM2_part3, rowStartDIM2_part4, rowEndDIM2_part4);

                list.Add(obj);
            }
        }

        #endregion
    }
}
