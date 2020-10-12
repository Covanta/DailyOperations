using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Covanta.DataAccess
{
    /// <summary>
    /// This class is just used as an entity for the Update Deltas Method
    /// </summary>
    /// 
    public class EnergyMeter
    {
        public EnergyMeter() { }
        public int id { get; set; }
        public DateTime date1 { get; set; }
        public decimal MegaWattHoursDelv { get; set; }
        public decimal MegaWattHoursDelvDelta { get; set; }
        public decimal MegaVARHoursDelv { get; set; }
        public decimal MegaVARHoursDelvDelta { get; set; }
    }

    public class DALEnergyMeter : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALEnergyMeter(string dbConnection) : base(dbConnection) { }

        #endregion

        #region private variables
        public List<EnergyMeter> energyMeterList = new List<EnergyMeter>();

        #endregion

        #region public methods

        /// <summary>
        /// Gets a list of the Energy Meter files which were already processed
        /// </summary>
        /// <returns>thew list of file names</returns>
        public List<string> GetProcessedFileList()
        {
            DataTable dt = new DataTable();
            List<string> list = new List<string>();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "EnergyMeterApp_GetFileAndLocationNames";

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            list.Clear();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    string fileName = bindObj.ToStringValue("FileName");
                    list.Add(fileName);
                }
            }

            //set null reference to uneeded objects
            dt = null;

            return list;
        }

        /// <summary>
        /// Updates the EnergyMeter table with the delta values from the row preceeding each row
        /// </summary>
        /// <param name="location"></param>
        public void UpdateEnergyTableWithDeltaValues(string location)
        {
            populateListOfEnergyMeterRowsWithNullDeltas(location);

            // at this point 
            // energyMeterList contains all of the records with null as the delta fields

            populateDeltas(location);
        }

        /// <summary>
        /// Inserts a row into the EnergyMeter table
        /// </summary>
        /// <param name="dataArray"></param>
        public void InsertEnergyMeterToSQLServer(object[,] dataArray, string location, string fileName)
        {
            int arrayLength0 = dataArray.GetUpperBound(0);
            int arrayLength1 = dataArray.GetUpperBound(1);

            //if a cell is null, then replace it with a spaces
            for (int i = 0; i < arrayLength0 + 1; i++)
            {
                for (int j = 0; j < arrayLength1 + 1; j++)
                {
                    if (dataArray[i, j] == null)
                    {
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
                        if (dataArray[i, 0].ToString().ToUpper().Substring(0, 4) == "TIME") { continue; }

                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "EnergyMeterApp_Insert";
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("FileName", fileName);

                        DateTime dateTimeToUse = parseDateFromFileNameAndTimeField(dataArray, fileName, i);
                        command.Parameters.AddWithValue("@DateTime1", dateTimeToUse);

                        decimal valueDecimal = 0;
                        //int valuedInt = 0;
                        decimal resultDecimal = 0;
                        //int resultInt = 0;

                        //tryParse fields and throw exception if needed

                        //tryParseIntAndThrowError(dataArray[i, 1].ToString(), "MegaWattHoursDelv", fileName, location);
                        //tryParseIntAndThrowError(dataArray[i, 2].ToString(), "MegaVARHoursDelv", fileName, location);

                        //02/15/2015 changed these 2 fields above to decimals instead of int
                        tryParseDecimalAndThrowError(dataArray[i, 1].ToString(), "MegaWattHoursDelv", fileName, location);
                        tryParseDecimalAndThrowError(dataArray[i, 2].ToString(), "MegaVARHoursDelv", fileName, location);


                        tryParseDecimalAndThrowError(dataArray[i, 3].ToString(), "AvgValueMegaWatt", fileName, location);
                        tryParseDecimalAndThrowError(dataArray[i, 4].ToString(), "AvgValueMegaVAR", fileName, location);
                        tryParseDecimalAndThrowError(dataArray[i, 5].ToString(), "InstantAvgValueMegaWatt", fileName, location);
                        tryParseDecimalAndThrowError(dataArray[i, 6].ToString(), "InstantAvgValueMegaVAR", fileName, location);

                        /*
                        resultInt = int.TryParse(dataArray[i, 1].ToString(), out valuedInt) ? valuedInt : 0;
                        command.Parameters.AddWithValue("@MegaWattHoursDelv", resultInt);
                        int megaWattHoursDelvToBeInserted = resultInt;

                        resultInt = int.TryParse(dataArray[i, 2].ToString(), out valuedInt) ? valuedInt : 0;
                        command.Parameters.AddWithValue("@MegaVARHoursDelv", resultInt);
                        int megaVARHoursDelvToBeInserted = resultInt;
                        */

                        //02/15/2015 changed these 2 fields above to decimals instead of int

                        resultDecimal = decimal.TryParse(dataArray[i, 1].ToString(), out valueDecimal) ? valueDecimal : 0;
                        command.Parameters.AddWithValue("@MegaWattHoursDelv", resultDecimal);
                        decimal megaWattHoursDelvToBeInserted = resultDecimal;

                        resultDecimal = decimal.TryParse(dataArray[i, 2].ToString(), out valueDecimal) ? valueDecimal : 0;
                        command.Parameters.AddWithValue("@MegaVARHoursDelv", resultDecimal);
                        decimal megaVARHoursDelvToBeInserted = resultDecimal;


                        resultDecimal = decimal.TryParse(dataArray[i, 3].ToString(), out valueDecimal) ? valueDecimal : 0;
                        command.Parameters.AddWithValue("@AvgValueMegaWatt", resultDecimal);

                        resultDecimal = decimal.TryParse(dataArray[i, 4].ToString(), out valueDecimal) ? valueDecimal : 0;
                        command.Parameters.AddWithValue("@AvgValueMegaVAR", resultDecimal);

                        resultDecimal = decimal.TryParse(dataArray[i, 5].ToString(), out valueDecimal) ? valueDecimal : 0;
                        command.Parameters.AddWithValue("@InstantAvgValueMegaWatt", resultDecimal);

                        resultDecimal = decimal.TryParse(dataArray[i, 6].ToString(), out valueDecimal) ? valueDecimal : 0;
                        command.Parameters.AddWithValue("@InstantAvgValueMegaVAR", resultDecimal);

                        //Eric Comment out the powerfactor processing
                        //string powerFactorString1 = dataArray[i, 7].ToString();
                        //if (powerFactorString1 == "0.0") { powerFactorString1 = "0.0e-1"; }
                        //if (powerFactorString1.Length < 4)
                        //{
                        //    throw new Exception("FileName " + fileName + " contains a bad power factor field.  " + powerFactorString1 + " is not in expected format of 9.99613e-1");
                        //}
                        //string powerFactorString2 = powerFactorString1.Substring(0, powerFactorString1.Length - 3);
                        //decimal powerFactorDecimal = decimal.Parse(powerFactorString2) / 10;

                        command.Parameters.AddWithValue("@PowerFactor", 0.000);

                        //Deltas
                        command.Parameters.AddWithValue("@MegaWattHoursDelvDelta", DBNull.Value);
                        command.Parameters.AddWithValue("@MegaVARHoursDelvDelta", DBNull.Value);

                        //If MegaWattHoursDelv = 0 
                        // or
                        //If AvgValueMegaVAR = 0
                        //  Then don't insert the row.
                        if (megaWattHoursDelvToBeInserted == 0 || megaVARHoursDelvToBeInserted == 0)
                        {
                            //dont insert
                        }
                        else
                        {
                            //insert
                            command.ExecuteNonQuery();                            
                        }
                        command.Parameters.Clear();
                    }
                }
            }
        }

        #endregion

        #region private methods

        private void populateDeltas(string location)
        {
            foreach (EnergyMeter item in energyMeterList)
            {
                populateDeltasInList(location, item);
                insertUpdatesToDeltas(item);
            }
        }

        /// <summary>
        /// Call Stored Proc and write deltas to database
        /// </summary>
        /// <param name="item"></param>
        private void insertUpdatesToDeltas(EnergyMeter item)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "EnergyMeterApp_UpdateByID";
                    command.Parameters.AddWithValue("@id", item.id);

                    //Deltas
                    command.Parameters.AddWithValue("@MegaWattHoursDelvDelta", item.MegaWattHoursDelvDelta);
                    command.Parameters.AddWithValue("@MegaVARHoursDelvDelta", item.MegaVARHoursDelvDelta);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// puts the deltas in the objects in the list by reading the previos values and getting the difference
        /// </summary>
        /// <param name="location"></param>
        /// <param name="item"></param>
        private void populateDeltasInList(string location, EnergyMeter item)
        {
            DataTable dt = new DataTable();
            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "EnergyMeterApp_GetPreviousMeterValuesByDateAndLocation";
            command.Parameters.AddWithValue("@Location", location);
            command.Parameters.AddWithValue("@date1", item.date1);

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //we now have the previous value
            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    item.MegaWattHoursDelvDelta = item.MegaWattHoursDelv - bindObj.ToDecimal("MegaWattHoursDelv");
                    item.MegaVARHoursDelvDelta = item.MegaVARHoursDelv - bindObj.ToDecimal("MegaVARHoursDelv");
                }
            }
            else
            {
                item.MegaWattHoursDelvDelta = 0;
                item.MegaVARHoursDelvDelta = 0;
            }

            //set null reference to uneeded objects
            dt = null;
        }

        private void populateListOfEnergyMeterRowsWithNullDeltas(string location)
        {
            DataTable dt = new DataTable();
            energyMeterList.Clear();

            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand command = new SqlCommand();

            //populate the command object
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "EnergyMeterApp_GetRowsByLocationWhereDeltaIsNull";
            command.Parameters.AddWithValue("@Location", location);

            //execute command
            dt = executeSQLCommandGetDataTable(command, connection);

            //load Lists from DataTable if at least 1 row returned
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);
                    int id = bindObj.ToInteger("ID");
                    DateTime date1 = bindObj.ToDate("DateTime1");
                    EnergyMeter em = new EnergyMeter();
                    em.id = bindObj.ToInteger("ID");
                    em.date1 = bindObj.ToDate("DateTime1");
                    em.MegaWattHoursDelv = bindObj.ToDecimal("MegaWattHoursDelv");
                    em.MegaVARHoursDelv = bindObj.ToDecimal("MegaVARHoursDelv");
                    energyMeterList.Add(em);
                }
            }

            //set null reference to uneeded objects
            dt = null;
        }

        private static DateTime parseDateFromFileNameAndTimeField(object[,] dataArray, string fileName, int i)
        {
            DateTime result = DateTime.Today;
            DateTime result1 = DateTime.Today;
            DateTime timeToUse = DateTime.Today;
            DateTime dateTimeToUse = DateTime.Today;

            // check to see if the time part is valid from the file
            if (DateTime.TryParse(dataArray[i, 0].ToString(), out result) == false)
            {
                throw new Exception("FileName " + fileName + " contains a bad date field.  " + dataArray[i, 0].ToString() + " is not a valid date and time");
            }
            timeToUse = result;

            string datePart = fileName.Substring(0, 4) + "-" + fileName.Substring(4, 2) + "-" + fileName.Substring(6, 2);
            string timePart = dataArray[i, 0].ToString();

            string timeToUseHour = timeToUse.Hour.ToString();
            string timeToUseMinute = timeToUse.Minute.ToString();

            // check to see if the date part is valid from the file
            string fullDateTime = datePart + " " + timeToUseHour + ":" + timeToUseMinute;
            if (DateTime.TryParse(fullDateTime, out result1) == false)
            {
                throw new Exception("FileName " + fileName + " contains a bad date field.  " + fullDateTime + " is not a valid date and time");
            }
            dateTimeToUse = result1;

            // The file comes in with the times for 11:30pm thru 11:55PM (23:30 - 23:55) on the next file.
            // In other words the data on
            // 20150124_0000_Rainbow.csv
            // contains the data for the times 2015-01-23 23:35:00.000 thru 2015-01-23 23:55:00.000
            //
            // This means that Thurday night's data comes in on a file named for the date of Friday.
            // To make up for this, we have to change the DateTime on these 5 rows to the prior day.

            if ((dateTimeToUse.Hour == 23) && (dateTimeToUse.Minute >= 31) && (dateTimeToUse.Minute <= 59))
            {
                dateTimeToUse = dateTimeToUse.AddDays(-1);
            }

            return dateTimeToUse;
        }

        private void tryParseIntAndThrowError(string value, string field, string file, string location)
        {
            int result = 0;
            if (int.TryParse(value, out result) == false)
            {
                throw new Exception("FileName " + file + " contains a bad field.  " + field + " is not a valid integer.  Location is " + location);
            }
        }

        private void tryParseDecimalAndThrowError(string value, string field, string file, string location)
        {
            decimal result = 0;
            if (decimal.TryParse(value, out result) == false)
            {
                throw new Exception("FileName " + file + " contains a bad field.  " + field + " is not a valid decimal.  Location is " + location);
            }
        }

        #endregion

    }
}
