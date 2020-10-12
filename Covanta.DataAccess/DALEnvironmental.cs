using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALEnvironmental : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALEnvironmental(string dbConnection) : base(dbConnection) { }

        #endregion

        #region private variables
        DateTime _docModifiedDate;
        #endregion

        #region public methods

        /// <summary>
        /// Calls Stored Procedure to delete all rows from the EnvEmissionData table
        /// </summary>
        public void DeleteAllFromEmissionsData()
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
                        command.CommandText = DALSqlStatements.EnvironmentalETL.SQLSP_Environmental_00100_Delete_EmissionData;

                        commandString = populateExceptionDataBase(command);

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

        public void ParseEnvironmentalDataTableBaseline(DataTable dt, List<EnvEmissionData> list, string type1, DateTime docModifiedDate)
        {
            _docModifiedDate = docModifiedDate;
            type1 = type1.Replace(" ", "");
            parseBaselineDetail(dt, list, type1, "a", "b", "2006");
            parseBaselineDetail(dt, list, type1, "c", "d", "2007");
            parseBaselineDetail(dt, list, type1, "e", "f", "2008");
            parseBaselineDetail(dt, list, type1, "g", "h", "2009");
            parseBaselineDetail(dt, list, type1, "i", "j", "2010");
            parseBaselineDetail(dt, list, type1, "k", "l", "2011");
            parseBaselineDetail(dt, list, type1, "m", "n", "2012");
            parseBaselineDetail(dt, list, type1, "o", "p", "2013");
            parseBaselineDetail(dt, list, type1, "q", "r", "2014");
            parseBaselineDetail(dt, list, type1, "s", "t", "2015");
            parseBaselineDetail(dt, list, type1, "u", "v", "2016");
            parseBaselineDetail(dt, list, type1, "w", "x", "2017");
            parseBaselineDetail(dt, list, type1, "y", "z", "2018");
            parseBaselineDetail(dt, list, type1, "aa", "ab", "2019");
            parseBaselineDetail(dt, list, type1, "ac", "ad", "2020");

        }

        private void parseBaselineDetail(DataTable dt, List<EnvEmissionData> list, string type1, string currentColumnName, string currentColumnPercent, string currentYear)
        {
            foreach (DataRow dr in dt.Rows) 
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                EnvEmissionData obj = new EnvEmissionData();
                // add baseline data
                if (
                       (populateString(bindObj, currentColumnName) != string.Empty)
                    && (populateString(bindObj, currentColumnName) != "Overall Index")
                    && (populateString(bindObj, currentColumnName) != "Count")
                    && (populateString(bindObj, currentColumnName) != currentYear)
                    )
                {
                    obj.Entity = populateString(bindObj, currentColumnName);
                    obj.Type = type1;
                    obj.Year = currentYear;
                    obj.Value = populateDecimal(bindObj, currentColumnPercent);
                    obj.DocumentLastModifiedDate = _docModifiedDate;
                    list.Add(obj);
                }
                //add baseline overall index row
                if (populateString(bindObj, currentColumnName) == "Overall Index")
                {
                    obj.Entity = "Overall";
                    obj.Type = type1 + "Overall";
                    obj.Year = currentYear;
                    obj.Value = populateDecimal(bindObj, currentColumnPercent);
                    obj.DocumentLastModifiedDate = _docModifiedDate;
                    list.Add(obj);
                }
            }
        }

        public void ParseEnvironmentalDataTableEmissionTypes(DataTable dt, List<EnvEmissionData> list, string type1, DateTime docModifiedDate)
        {
            _docModifiedDate = docModifiedDate;
            string columnLocation = "Location";
            string column2006 = "2006";
            string column2007 = "2007";
            string column2008 = "2008";
            string column2009 = "2009";
            string column2010 = "2010";
            string column2011 = "2011";
            string column2012 = "2012";
            string column2013 = "2013";
            string column2014 = "2014";
            string column2015 = "2015";
            string column2016 = "2016";
            string column2017 = "2017";
            string column2018 = "2018";
            string column2019 = "2019";
            string column2020 = "2020";
            string column2021 = "2021";
            string column2022 = "2022";
            string column2023 = "2023";
            string column2024 = "2024";
            string column2025 = "2025";
            string column2026 = "2026";

            dt.Columns[0].ColumnName = columnLocation;
            dt.Columns[1].ColumnName = column2006;
            dt.Columns[2].ColumnName = column2007;
            dt.Columns[3].ColumnName = column2008;
            dt.Columns[4].ColumnName = column2009;
            dt.Columns[5].ColumnName = column2010;
            dt.Columns[6].ColumnName = column2011;
            dt.Columns[7].ColumnName = column2012;
            dt.Columns[8].ColumnName = column2013;
            dt.Columns[9].ColumnName = column2014;

            // Added logic to allow for furture years to be added to the excel with no change top the application.
            // The application will just read it if it is there

            if (dt.Columns.Count > 10) { dt.Columns[10].ColumnName = column2015; }
            if (dt.Columns.Count > 11) { dt.Columns[11].ColumnName = column2016; }
            if (dt.Columns.Count > 12) { dt.Columns[12].ColumnName = column2017; }
            if (dt.Columns.Count > 13) { dt.Columns[13].ColumnName = column2018; }
            if (dt.Columns.Count > 14) { dt.Columns[14].ColumnName = column2019; }
            if (dt.Columns.Count > 15) { dt.Columns[15].ColumnName = column2020; }
            if (dt.Columns.Count > 16) { dt.Columns[16].ColumnName = column2021; }
            if (dt.Columns.Count > 17) { dt.Columns[17].ColumnName = column2022; }
            if (dt.Columns.Count > 18) { dt.Columns[18].ColumnName = column2023; }
            if (dt.Columns.Count > 19) { dt.Columns[19].ColumnName = column2024; }
            if (dt.Columns.Count > 20) { dt.Columns[20].ColumnName = column2025; }
            if (dt.Columns.Count > 21) { dt.Columns[21].ColumnName = column2026; }


            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                EnvEmissionData obj = new EnvEmissionData();

                obj.Entity = populateString(bindObj, columnLocation);

                if (populateString(bindObj, columnLocation) != string.Empty)
                {
                    if (populateString(bindObj, columnLocation) != "Corporate Average")
                    {
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2006, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2007, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2008, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2009, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2010, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2011, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2012, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2013, columnLocation, type1);
                        populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2014, columnLocation, type1);
                        //will be used for 2015 - 2026
                        if (dt.Columns.Count > 10) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2015, columnLocation, type1); }
                        if (dt.Columns.Count > 11) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2016, columnLocation, type1); }
                        if (dt.Columns.Count > 12) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2017, columnLocation, type1); }
                        if (dt.Columns.Count > 13) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2018, columnLocation, type1); }
                        if (dt.Columns.Count > 14) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2019, columnLocation, type1); }
                        if (dt.Columns.Count > 15) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2020, columnLocation, type1); }
                        if (dt.Columns.Count > 16) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2021, columnLocation, type1); }
                        if (dt.Columns.Count > 17) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2022, columnLocation, type1); }
                        if (dt.Columns.Count > 18) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2023, columnLocation, type1); }
                        if (dt.Columns.Count > 19) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2024, columnLocation, type1); }
                        if (dt.Columns.Count > 20) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2025, columnLocation, type1); }
                        if (dt.Columns.Count > 21) { populateEmissionsObject(list, bindObj, new EnvEmissionData(), column2026, columnLocation, type1); }
                    }

                }

            }
        }

        private void populateEmissionsObject(List<EnvEmissionData> list, BindDataRowToProperty bindObj, EnvEmissionData obj, string year, string location, string type1)
        {
            obj.Entity = populateString(bindObj, location);
            obj.Type = type1;
            obj.Year = year;
            obj.Value = populateDecimal(bindObj, year);
            obj.DocumentLastModifiedDate = _docModifiedDate;
            list.Add(obj);

        }


        /// <summary>
        /// Inserts a list of EnvEmissionData objects into the database
        /// </summary>
        /// <param name="list">List of EnvEmissionData</param>
        public void InsertEnvironmentalDataListToDatabase(List<EnvEmissionData> list)
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            foreach (EnvEmissionData obj in list)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_dbConnection))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = DALSqlStatements.EnvironmentalETL.SQLSP_Environmental_00300_Insert_EmissionData;
                            command.Parameters.AddWithValue("@Entity", obj.Entity);
                            command.Parameters.AddWithValue("@Type", obj.Type);
                            command.Parameters.AddWithValue("@Year", obj.Year);
                            command.Parameters.AddWithValue("@Value", obj.Value);
                            command.Parameters.AddWithValue("@DocumentModifiedDate", obj.DocumentLastModifiedDate);

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

        private string populateString(BindDataRowToProperty bindObj, string p)
        {
            if (bindObj.ToStringValue(p) == null)
            {
                return String.Empty;
            }
            else
            {
                return bindObj.ToStringValue(p);
            }
        }

        private decimal populateDecimal(BindDataRowToProperty bindObj, string p)
        {
            decimal returnValue = 0;
            if (bindObj.ToStringValue(p) == null)
            {
                return 0;
            }
            else
            {
                string number1 = bindObj.ToStringValue(p);
                string number2 = replaceParenthesisWithNegativeSign(number1);

                number2 = number2.Replace("%","");
                if (decimal.TryParse(number2, out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    return 0;
                }
            }
        }

        private string replaceParenthesisWithNegativeSign(string p)
        {
            string resultString = p;
            if (p.Length > 2)
            {
                if ((p.Substring(0, 1) == "(") && (p.Substring(p.Length - 1, 1) == ")"))
                {
                    resultString = p.Substring(1, p.Length - 2);
                    resultString = "-" + resultString;
                }
            }
            return resultString;
        }

        private int populateInt(BindDataRowToProperty bindObj, string p)
        {
            int returnValue = 0;
            if (bindObj.ToStringValue(p) == null)
            {
                return 0;
            }
            else
            {
                string number1 = bindObj.ToStringValue(p);
                string number2 = replaceParenthesisWithNegativeSign(number1);

                if (int.TryParse(number2, out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion
    }
}
