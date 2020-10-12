using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.Data;
using Covanta.Entities;
using Covanta.DataAccess;
using System.Configuration;

namespace Covanta.BusinessLogic
{
    public class C4RForecastRecommendationManager
    {
        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public C4RForecastRecommendationManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the C4RForecastRecommendationManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region private variables

        string _dbConnection = null;
        string _path = string.Empty;
        string _file = string.Empty;
        List<ForecastRecommendationData> _list = new List<ForecastRecommendationData>();
        string _sheetName = ConfigurationManager.AppSettings["SheetNameForecast"];

        #endregion

        #region public methods

        public void ProcessFiles(string path, string file)
        {
            _path = path;
            _file = file;
            deleteAllRowsFromDatabase();
            populateListFromExcelDoc();
            insertToDatabase();
        }

        private void insertToDatabase()
        {
            DALC4RForecastRecommendation dal = new DALC4RForecastRecommendation(_dbConnection);
            dal.InsertForecastRecommendationListToDatabase(_list);
        }

        private void populateListFromExcelDoc()
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            // load Excel into a sheet collection    
            Workbook wbSource = new Workbook(_path + _file);
            WorksheetCollection wsc = wbSource.Worksheets;
            foreach (Worksheet sheet in wsc)
            {
                //if (sheet.Name == "2014 Forecast")
                if (sheet.Name == _sheetName)   //should be 'ForecastBudgetMaster'
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);

                    loadList(dt);
                }
            }
        }

        #region private methods

        private void loadList(DataTable dt)
        {
            _list.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ForecastRecommendationData fr = new ForecastRecommendationData();
                fr.Region = dr["F"].ToString();
                fr.Facility = dr["G"].ToString();
                fr.FacilityShortName = dr["H"].ToString();
                fr.AccountID = dr["I"].ToString();
                fr.SequenceNum = dr["J"].ToString();
                fr.AccountDesc = dr["K"].ToString();
                fr.RevShare = dr["L"].ToString();
                fr.Identifier = dr["M"].ToString();
               
                //Jan
                addMonthlyValues(dr, fr, "2014", "01", "N", "O", "P", "Q");
                //Feb
                addMonthlyValues(dr, fr, "2014", "02", "R", "S", "T", "U");
                //Mar
                addMonthlyValues(dr, fr, "2014", "03", "V", "W", "X", "Y");
                //Apr
                addMonthlyValues(dr, fr, "2014", "04", "Z", "AA", "AB", "AC");
                //May
                addMonthlyValues(dr, fr, "2014", "05", "AD", "AE", "AF", "AG");
                //Jun
                addMonthlyValues(dr, fr, "2014", "06", "AH", "AI", "AJ", "AK");
                //Jul
                addMonthlyValues(dr, fr, "2014", "07", "AL", "AM", "AN", "AO");
                //Aug
                addMonthlyValues(dr, fr, "2014", "08", "AP", "AQ", "AR", "AS");
                //Sep
                addMonthlyValues(dr, fr, "2014", "09", "AT", "AU", "AV", "AW");
                //Oct
                addMonthlyValues(dr, fr, "2014", "10", "AX", "AY", "AZ", "BA");
                //Nov
                addMonthlyValues(dr, fr, "2014", "11", "BB", "BC", "BD", "BE");
                //Dec
                addMonthlyValues(dr, fr, "2014", "12", "BF", "BG", "BH", "BI");


                //Jan
                addMonthlyValues(dr, fr, "2015", "01", "BN", "BO", "BP", "BQ");
                //Feb
                addMonthlyValues(dr, fr, "2015", "02", "BR", "BS", "BT", "BU");
                //Mar
                addMonthlyValues(dr, fr, "2015", "03", "BV", "BW", "BX", "BY");
                //Apr
                addMonthlyValues(dr, fr, "2015", "04", "BZ", "CA", "CB", "CC");
                //May
                addMonthlyValues(dr, fr, "2015", "05", "CD", "CE", "CF", "CG");
                //Jun
                addMonthlyValues(dr, fr, "2015", "06", "CH", "CI", "CJ", "CK");
                //Jul
                addMonthlyValues(dr, fr, "2015", "07", "CL", "CM", "CN", "CO");
                //Aug
                addMonthlyValues(dr, fr, "2015", "08", "CP", "CQ", "CR", "CS");
                //Sep
                addMonthlyValues(dr, fr, "2015", "09", "CT", "CU", "CV", "CW");
                //Oct
                addMonthlyValues(dr, fr, "2015", "10", "CX", "CY", "CZ", "DA");
                //Nov
                addMonthlyValues(dr, fr, "2015", "11", "DB", "DC", "DD", "DE");
                //Dec
                addMonthlyValues(dr, fr, "2015", "12", "DF", "DG", "DH", "DI");
            }
        }

        private void addMonthlyValues(DataRow dr, ForecastRecommendationData fr, string year, string Month1, string A, string B, string C, string D)
        {
            ForecastRecommendationData fr1 = new ForecastRecommendationData();
            fr1.Region = fr.Region;
            fr1.Facility = fr.Facility;
            fr1.FacilityShortName = fr.FacilityShortName;
            fr1.AccountID = fr.AccountID;
            fr1.SequenceNum = fr.SequenceNum;
            fr1.AccountDesc = fr.AccountDesc;
            fr1.RevShare = fr.RevShare;
            fr1.Identifier = fr.Identifier;
            fr1.Year = fr.Year;
            fr1.Year = year;
            fr1.Month = Month1;
            fr1.Budget = tryparseInt(replaceBadChars(dr[A].ToString())) ? Convert.ToInt32(replaceBadChars(dr[A].ToString())) : 0;
            fr1.Forecast = tryparseInt(replaceBadChars(dr[B].ToString())) ? Convert.ToInt32(replaceBadChars(dr[B].ToString())) : 0;
            fr1.Actual_EST = tryparseInt(replaceBadChars(dr[C].ToString())) ? Convert.ToInt32(replaceBadChars(dr[C].ToString())) : 0;
            fr1.Actual = tryparseInt(replaceBadChars(dr[D].ToString())) ? Convert.ToInt32(replaceBadChars(dr[D].ToString())) : 0;

            if ((fr1.FacilityShortName != string.Empty) && (fr1.FacilityShortName != "Facility Shortname"))
            {
                _list.Add(fr1);
            }

           
        }

        private string replaceBadChars(string x)
        {
            x = x.Replace(" ", "");
            x = x.Replace(",", "");
            return x;
        }

        private bool tryparseInt(string x)
        {
            int value = 0;
            return int.TryParse(x, out value);
        }

        private void deleteAllRowsFromDatabase()
        {
            DALC4RForecastRecommendation dal = new DALC4RForecastRecommendation(_dbConnection);
            dal.DeleteAllFromC4ForecastRecommendation();
        }
        #endregion



        #endregion
    }
}

