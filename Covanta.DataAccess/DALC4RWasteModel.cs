using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALC4RWasteModel : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALC4RWasteModel(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods

        /// <summary>
        /// Calls Stored Procedure to delete all rows from the C4RWasteModel table
        /// </summary>
        public void DeleteAllFromC4RWasteModel()
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
                        command.CommandText = DALSqlStatements.C4RWasteModel.SQLSP_C4RWasteModelApp_DeleteAllRows;

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

        /// <summary>
        /// Parses thru a data table of C4R Waste Model data and parses it into a list of objects
        /// </summary>
        /// <param name="dt">A data table populated with C4R Waste Model data</param>
        /// <param name="list">A list of C4R Waste Model objects</param>
        /// <param name="location">Covanta Location</param>
        public void ParseC4RWasteModel_ActualDataTable(DataTable dt, List<C4RWasteModel> list, string location)
        {
            string columnDATE = "DATE";
            string columnDay = "Day";
            string columnBaseCity = "Base (City)";
            string columnPrem = "Prem";
            string columnSpot = "Spot";
            string columnRaysprem = "Ray's (prem)";
            string columncustomer1 = "customer 1";
            string columncustomer2 = "customer 2";
            string columncustomer3 = "customer 3";
            string columncustomer4 = "customer 4";
            string columnSpecialSolid = "Special - Solid";
            string columnSpecialLDI = "Special - LDI";
            string columnMSWRecvd = "MSW Recv'd";
            string columnTotalSolidRecvd = "Total  Solid Recv'd";
            string columnSteamGenerated = "Steam Generated";
            string columnSteamFactor = "Steam Factor";
            string columnSteamRate = "Steam Rate";
            string columnBoilersonline = "Boilers online";
            string columnTotalSolidProcessed = "Total Solid Processed";
            string columnModelCalculatedPitInventory = "Model Calculated Pit Inventory"; 
            string columnPlantEstdInventory = "Plant Est'd Inventory";
            string columnModelvsPlantVariance = "Model vs. Plant Variance";
            
            dt.Columns[0].ColumnName = columnDATE;
            dt.Columns[1].ColumnName = columnDay;
            dt.Columns[2].ColumnName = columnBaseCity;
            dt.Columns[3].ColumnName = columnPrem;
            dt.Columns[4].ColumnName = columnSpot;
            dt.Columns[5].ColumnName = columnRaysprem;
            dt.Columns[6].ColumnName = columncustomer1;
            dt.Columns[7].ColumnName = columncustomer2;
            dt.Columns[8].ColumnName = columncustomer3;
            dt.Columns[9].ColumnName = columncustomer4;
            dt.Columns[10].ColumnName = columnSpecialSolid;
            dt.Columns[11].ColumnName = columnSpecialLDI;
            dt.Columns[12].ColumnName = columnMSWRecvd;
            dt.Columns[13].ColumnName = columnTotalSolidRecvd;
            dt.Columns[14].ColumnName = columnSteamGenerated;
            dt.Columns[15].ColumnName = columnSteamFactor;
            dt.Columns[16].ColumnName = columnSteamRate;
            dt.Columns[17].ColumnName = columnBoilersonline;
            dt.Columns[18].ColumnName = columnTotalSolidProcessed;
            dt.Columns[19].ColumnName = columnModelCalculatedPitInventory;
            dt.Columns[20].ColumnName = columnPlantEstdInventory;
            dt.Columns[21].ColumnName = columnModelvsPlantVariance;
          

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                C4RWasteModel obj = new C4RWasteModel();
                DateTime dateTest = DateTime.Now;
                string dateStringTest = populateString(bindObj, columnDATE);

                if (DateTime.TryParse(dateStringTest, out dateTest))
                {
                    obj.Location = location;
                    obj.Date1 = DateTime.Parse(populateString(bindObj, columnDATE));
                    obj.Base = (int)populateDecimal(bindObj, columnBaseCity);
                    obj.Premium = (int)populateDecimal(bindObj, columnPrem);
                    obj.RaysPremium = (int)populateDecimal(bindObj, columnRaysprem);
                    obj.Spot = (int)populateDecimal(bindObj, columnSpot);
                    obj.SpecialSolid = (int)populateDecimal(bindObj, columnSpecialSolid);
                    obj.SpecialLDI = (int)populateDecimal(bindObj, columnSpecialLDI);
                    obj.MSWReceived = (int)populateDecimal(bindObj, columnMSWRecvd);
                    obj.TotalSolidReceived = (int)populateDecimal(bindObj, columnTotalSolidRecvd);
                    obj.SteamGenerated = (int)populateDecimal(bindObj, columnSteamGenerated);
                    obj.SteamFactor = (int)populateDecimal(bindObj, columnSteamFactor);
                    obj.SteamRate = (int)populateDecimal(bindObj, columnSteamRate);
                    obj.BoilersOnline = (int)populateDecimal(bindObj, columnBoilersonline);
                    obj.TotalSolidProcessed = (int)populateDecimal(bindObj, columnTotalSolidProcessed);
                    obj.ModelCalcPitInventory = (int)populateDecimal(bindObj, columnModelCalculatedPitInventory);
                    obj.PlantEstInventory = (int)populateDecimal(bindObj, columnPlantEstdInventory);
                    obj.ModelVsPlantVariance = (int)populateDecimal(bindObj, columnModelvsPlantVariance);

                    //obj.Location = location;
                    //obj.Date1 = DateTime.Parse(populateString(bindObj, "DATE"));
                    //obj.Base = (int)populateDecimal(bindObj, "Base (City)");
                    //obj.Premium = (int)populateDecimal(bindObj, "Prem");
                    //obj.RaysPremium = (int)populateDecimal(bindObj, "Ray's (prem)");

                    ////  obj.RaysPremium =  dr[3].ToString());
                    //obj.Spot = (int)populateDecimal(bindObj, "Spot");
                    //obj.SpecialSolid = (int)populateDecimal(bindObj, "Special - Solid");
                    //obj.SpecialLDI = (int)populateDecimal(bindObj, "Special - LDI");
                    //obj.MSWReceived = (int)populateDecimal(bindObj, "MSW Recv'd");
                    //obj.TotalSolidReceived = (int)populateDecimal(bindObj, "Total  Solid Recv'd");
                    //obj.SteamGenerated = (int)populateDecimal(bindObj, "Steam Generated");
                    //obj.SteamFactor = (int)populateDecimal(bindObj, "Steam Factor");
                    //obj.SteamRate = (int)populateDecimal(bindObj, "Steam Rate");
                    //obj.BoilersOnline = (int)populateDecimal(bindObj, "Boilers online");
                    //obj.TotalSolidProcessed = (int)populateDecimal(bindObj, "Total Solid Processed");
                    //obj.ModelCalcPitInventory = (int)populateDecimal(bindObj, "Model Calculated Pit Inventory");
                    //obj.PlantEstInventory = (int)populateDecimal(bindObj, "Plant Est'd Inventory");
                    //obj.ModelVsPlantVariance = (int)populateDecimal(bindObj, "Model vs. Plant Variance");

                    list.Add(obj);
                }
            }
        }


        /// <summary>
        /// Inserts a list of C4RWasteModel objects into the database
        /// </summary>
        /// <param name="list">List of C4RWasteModel actuals</param>
        public void InsertC4RWasteModelListToDatabase(List<C4RWasteModel> list)
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            foreach (C4RWasteModel obj in list)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_dbConnection))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = DALSqlStatements.C4RWasteModel.SQLSP_C4RWasteModelApp_InsertAllRows;
                            command.Parameters.AddWithValue("@Location", obj.Location);
                            command.Parameters.AddWithValue("@Date1", obj.Date1);
                            command.Parameters.AddWithValue("@Base", obj.Base);
                            command.Parameters.AddWithValue("@Spot", obj.Spot);
                            command.Parameters.AddWithValue("@Premium", obj.Premium);
                            command.Parameters.AddWithValue("@RaysPremium", obj.RaysPremium);
                            command.Parameters.AddWithValue("@SpecialSolid", obj.SpecialSolid);
                            command.Parameters.AddWithValue("@SpecialLDI", obj.SpecialLDI);
                            command.Parameters.AddWithValue("@MSWReceived", obj.MSWReceived);
                            command.Parameters.AddWithValue("@TotalSolidReceived", obj.TotalSolidReceived);
                            command.Parameters.AddWithValue("@SteamGenerated", obj.SteamGenerated);
                            command.Parameters.AddWithValue("@SteamFactor", obj.SteamFactor);
                            command.Parameters.AddWithValue("@SteamRate", obj.SteamRate);
                            command.Parameters.AddWithValue("@BoilersOnline", obj.BoilersOnline);
                            command.Parameters.AddWithValue("@TotalSolidProcessed", obj.TotalSolidProcessed);
                            command.Parameters.AddWithValue("@ModelCalcPitInventory", obj.ModelCalcPitInventory);
                            command.Parameters.AddWithValue("@PlantEstInventory", obj.PlantEstInventory);
                            command.Parameters.AddWithValue("@ModelVsPlantVariance", obj.ModelVsPlantVariance);                           

                            command.Parameters.AddWithValue("@isWithin_3_Weeks", obj.isWithin_Next_3_Weeks ? 1 : 0);
                            command.Parameters.AddWithValue("@isWithin_6_Weeks", obj.isWithin_Next_6_Weeks ? 1 : 0);
                            command.Parameters.AddWithValue("@isBelowMinTons", obj.isBelowMinTons ? 1 : 0);
                            command.Parameters.AddWithValue("@MinTons", obj.MinTons);
                            command.Parameters.AddWithValue("@isDateInPast", obj.isDateInPast ? 1 : 0);
                            command.Parameters.AddWithValue("@isDateToday", obj.isDateToday ? 1 : 0);
                            command.Parameters.AddWithValue("@NextDateBelowMinTons", obj.NextDateBelowMinTons);
                            command.Parameters.AddWithValue("@Next_3_WeeksHasValueBelowMin", obj.Next_3_WeeksHasValueBelowMin ? 1 : 0);

                            command.Parameters.AddWithValue("@NextTonsBelowMinTons", obj.NextTonsBelowMinTons);
                            command.Parameters.AddWithValue("@DocumentLastModifiedDate", obj.DocumentLastModifiedDate);

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
                    resultString = "-" +  resultString;
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
