using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALC4RForecastRecommendation : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALC4RForecastRecommendation(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods

        /// <summary>
        /// Calls Stored Procedure to delete all rows from the C4RForecastRecommendation table
        /// </summary>
        public void DeleteAllFromC4ForecastRecommendation()
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
                        command.CommandText = DALSqlStatements.C4RForecastRecommendation.SQLSP_C4RForecastRecommendationApp_DeleteAllRows;

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(commandString + ex.ToString(), ex);
            }
        }


        /// <summary>
        /// Inserts a list of ForecastRecommendationData objects into the database
        /// </summary>
        /// <param name="list">List of ForecastRecommendationData</param>
        public void InsertForecastRecommendationListToDatabase(List<ForecastRecommendationData> list)
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            foreach (ForecastRecommendationData obj in list)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_dbConnection))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = DALSqlStatements.C4RForecastRecommendation.SQLSP_C4RForecastRecommendationApp_InsertAllRows;
                            command.Parameters.AddWithValue("@AccountDesc", obj.AccountDesc);
                            command.Parameters.AddWithValue("@AccountID", obj.AccountID);
                            command.Parameters.AddWithValue("@Actual", obj.Actual);
                            command.Parameters.AddWithValue("@Actual_EST", obj.Actual_EST);
                            command.Parameters.AddWithValue("@Budget", obj.Budget);
                            command.Parameters.AddWithValue("@Facility", obj.Facility);
                            command.Parameters.AddWithValue("@FacilityShortName", obj.FacilityShortName);
                            command.Parameters.AddWithValue("@Forecast", obj.Forecast);
                            command.Parameters.AddWithValue("@Identifier", obj.Identifier);
                            command.Parameters.AddWithValue("@Month1", obj.Month);
                            command.Parameters.AddWithValue("@Region", obj.Region);
                            command.Parameters.AddWithValue("@RevShare", obj.RevShare);
                            command.Parameters.AddWithValue("@SequenceNum", obj.SequenceNum);
                            command.Parameters.AddWithValue("@Year1", obj.Year);

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

        #endregion

        #region private methods

        //private string populateString(BindDataRowToProperty bindObj, string p)
        //{
        //    if (bindObj.ToStringValue(p) == null)
        //    {
        //        return String.Empty;
        //    }
        //    else
        //    {
        //        return bindObj.ToStringValue(p);
        //    }
        //}

        //private decimal populateDecimal(BindDataRowToProperty bindObj, string p)
        //{
        //    decimal returnValue = 0;
        //    if (bindObj.ToStringValue(p) == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        string number1 = bindObj.ToStringValue(p);
        //        string number2 = replaceParenthesisWithNegativeSign(number1);

        //        if (decimal.TryParse(number2, out returnValue))
        //        {
        //            return returnValue;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}

        //private string replaceParenthesisWithNegativeSign(string p)
        //{
        //    string resultString = p;
        //    if (p.Length > 2)
        //    {
        //        if ((p.Substring(0, 1) == "(") && (p.Substring(p.Length - 1, 1) == ")"))
        //        {
        //            resultString = p.Substring(1, p.Length - 2);                   
        //            resultString = "-" +  resultString;
        //        }
        //    }
        //    return resultString;
        //}

        //private int populateInt(BindDataRowToProperty bindObj, string p)
        //{
        //    int returnValue = 0;
        //    if (bindObj.ToStringValue(p) == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        string number1 = bindObj.ToStringValue(p);
        //        string number2 = replaceParenthesisWithNegativeSign(number1);

        //        if (int.TryParse(number2, out returnValue))
        //        {
        //            return returnValue;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}

        #endregion
    }
}
