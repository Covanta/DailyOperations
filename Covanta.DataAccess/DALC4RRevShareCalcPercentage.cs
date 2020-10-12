using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;
//using Oracle.DataAccess.Client;


namespace Covanta.DataAccess
{
    public class DALC4RRevShareCalcPercentage : DALBase
    {

        string _dbConnectionOracle;
        #region constructors
      
        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALC4RRevShareCalcPercentage(string dbConnection, string dbConnectionOracle) : base(dbConnection)
        {
            _dbConnectionOracle = dbConnectionOracle;
        }       

        #endregion

       
        #region public methods

        /// <summary>
        /// Gets a list of C4RRevShareJournalData from the Oracle Database
        /// </summary>        
        /// <returns>a list of C4RRevShareJournalData from the Oracle Database</returns>
        public List<C4RRevShareJournalData> GetC4RRevShareJournalDataListFromOracle()
        {
            string oradb = _dbConnectionOracle;

            List<C4RRevShareJournalData> list = new List<C4RRevShareJournalData>();

            //try
            //{
            //    using (OracleConnection conn = new OracleConnection(oradb)) 
            //    {
            //        conn.Open();

            //        OracleCommand cmd = new OracleCommand();
            //        cmd.Connection = conn;               
            //        cmd.CommandText = "SELECT BUSINESS_UNIT, LINE_DESCR ,ACCOUNT ,ACCOUNTING_PERIOD ,FISCAL_YEAR, MONETARY_AMOUNT FROM SYSADM.PS_MOD_JRNL_DRL_VW WHERE LEDGER = 'ACTUALS' AND CURRENCY_CD = 'USD' AND ACCOUNT = '44999' AND FISCAL_YEAR > '2011'";
  
            //        cmd.CommandType = CommandType.Text;

            //        cmd.CommandType = CommandType.Text;
            //        OracleDataReader dr = cmd.ExecuteReader();
            //        int recordCounter =0;
            //        while ((dr.Read()) && (recordCounter <= 100000))
            //       // while (dr.Read()) 
            //        {
            //            C4RRevShareJournalData journalData = new C4RRevShareJournalData();
            //            journalData.Account = dr["ACCOUNT"].ToString();
            //            journalData.LineDescription = dr["LINE_DESCR"].ToString();
            //            journalData.BusinessUnit = dr["BUSINESS_UNIT"].ToString();
            //            journalData.Month = dr["ACCOUNTING_PERIOD"].ToString();
            //            journalData.Year = dr["FISCAL_YEAR"].ToString();                      
            //            decimal xx = 0;
            //            if (decimal.TryParse(dr["MONETARY_AMOUNT"].ToString(), out xx))
            //            {
            //                journalData.Amount = xx;
            //            }
            //            else
            //            {
            //                journalData.Amount = 0;
            //            }
            //            list.Add(journalData);     
            //            recordCounter++;                
            //        }
                  
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //string xx = ex.Message;
            //    throw ex; 
            //}                     

            return list;

        }
             
        public void TruncateCovmetatableWildcardC4RRevSharePercentages()
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = DALSqlStatements.C4RRevShareCalcPercentageSQL.SQLSP_Truncate_C4RRevShareCalcPercentage;
                    command.ExecuteNonQuery();
                }
            }
        }
              
        public void InsertJournalRowsToSQLServer(List<C4RRevShareJournalData> list)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();

                foreach (C4RRevShareJournalData item in list)
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.C4RRevShareCalcPercentageSQL.SQLSP_InsertIntoWildcardC4RRevSharePercentage;
                        command.Parameters.AddWithValue("@Account", item.Account);
                        command.Parameters.AddWithValue("@Amount", item.Amount);
                        command.Parameters.AddWithValue("@BusinessUnit", item.BusinessUnit);
                        command.Parameters.AddWithValue("@LineDescription", item.LineDescription);
                        command.Parameters.AddWithValue("@Month", item.Month);  
                        command.Parameters.AddWithValue("@Year", item.Year);  
                        command.ExecuteNonQuery();
                    }
                }
            }

        }


        #endregion

        #region private methods

        /// <summary>
        /// Load a list of C4RForecastPathsAndFileNames
        /// </summary>
        /// <param name="dt">Datable with C4RForecastPathsAndFileNames</param>
        /// <param name="list">List of C4RForecastPathsAndFileNames</param>
        //private void loadC4RForecastPathsAndFileNamesList(DataTable dt, List<C4RForecastExtractSourceFileInfo> list)
        //{
        //    list.Clear();

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

        //        string C4RForecastPath = bindObj.ToStringValue("FilePaths");
        //        string C4RForecastFileName = bindObj.ToStringValue("FileNames");                

        //        int rowStartDIM1 = bindObj.ToInteger("DIM1_RowStart");
        //        int rowEndDIM1 = bindObj.ToInteger("DIM1_RowEnd");
        //        int rowStartDIM2_part1 = bindObj.ToInteger("DIM2_Part1_RowStart");
        //        int rowEndDIM2_part1 = bindObj.ToInteger("DIM2_Part1_RowEnd");
        //        int rowStartDIM2_part2 = bindObj.ToInteger("DIM2_Part2_RowStart");
        //        int rowEndDIM2_part2 = bindObj.ToInteger("DIM2_Part2_RowEnd");
        //        int rowStartDIM2_part3 = bindObj.ToInteger("DIM2_Part3_RowStart");
        //        int rowEndDIM2_part3 = bindObj.ToInteger("DIM2_Part3_RowEnd");
        //        int rowStartDIM2_part4 = bindObj.ToInteger("DIM2_Part4_RowStart");
        //        int rowEndDIM2_part4 = bindObj.ToInteger("DIM2_Part4_RowEnd");
        //        C4RForecastExtractSourceFileInfo obj = new C4RForecastExtractSourceFileInfo(C4RForecastPath, C4RForecastFileName, rowStartDIM1, rowEndDIM1, rowStartDIM2_part1, rowEndDIM2_part1, rowStartDIM2_part2, rowEndDIM2_part2, rowStartDIM2_part3, rowEndDIM2_part3, rowStartDIM2_part4, rowEndDIM2_part4);

        //        list.Add(obj);
        //    }
        //}

        #endregion
    }
}
