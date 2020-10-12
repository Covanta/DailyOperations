using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;
using Aspose.Cells;
using System.IO;

namespace Covanta.BusinessLogic
{
    public class C4RForecastExtractManager
    {
        #region private variables

        string _dbConnection = null;

        #endregion

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public C4RForecastExtractManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the C4RForecastExtractManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region public methods

        /// <summary>
        /// Creates a list of files and details about the files which will be extracted
        /// </summary>
        /// <returns>list is files names and meta data about the files</returns>
        public List<C4RForecastExtractSourceFileInfo> GetC4RForecastExtractSourceFileInfo()
        {
            List<C4RForecastExtractSourceFileInfo> list = null;
            try
            {
                DALC4RForecastExtract dal = new DALC4RForecastExtract(_dbConnection);
                list = dal.GetC4RForecastExtractSourceFileInfoList();
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return list;
        }
      
        /// <summary>
        ///  Parses thru the appropriate file (thru path and name) and extracts the data from it into a list of objects containg the data
        /// </summary>
        /// <param name="pathAndFile">path and file of file whose data will be extracted</param>
        /// <param name="list">list of objects whose properties match the extracted data</param>
        public void ExtractParseData(string path, List<C4RForecastExtractRow> list, C4RForecastExtractSourceFileInfo fileInfo)
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            //Get Business Unit from tab - Tons
            Workbook workbook = new Workbook(path + fileInfo.FileName);
            Worksheet worksheetTons = workbook.Worksheets["Tons"];
            string PeopleSoft5DigitCode = worksheetTons.Cells[1, 2].StringValue;

            if (fileInfo.FileName == "Plymouth 2012 Forecast.xls") { PeopleSoft5DigitCode = "MNTLP"; }
            if (fileInfo.FileName == "LongBeach 2012 Forecast.xls") { PeopleSoft5DigitCode = "LGBCH"; }

            //Get Forecast Info From tab  - Forecast Recommendation
            Worksheet worksheetTonsForecast = workbook.Worksheets["Forecast Recommendation"];
            if (worksheetTonsForecast == null)
            {
                worksheetTonsForecast = workbook.Worksheets["Forecast Recommedation"];
            }
            if (worksheetTonsForecast == null)
            {
                worksheetTonsForecast = workbook.Worksheets["Summary"];
            }
           
            string DIM1_1 = worksheetTonsForecast.Cells[fileInfo.DIM1_RowStart - 1, 0].StringValue;
            
            parseGroup(worksheetTonsForecast, list, PeopleSoft5DigitCode, DIM1_1, fileInfo.DIM2_Part1_RowStart, fileInfo.DIM2_Part1_RowEnd);
            parseGroup(worksheetTonsForecast, list, PeopleSoft5DigitCode, DIM1_1, fileInfo.DIM2_Part2_RowStart, fileInfo.DIM2_Part2_RowEnd);
            parseGroup(worksheetTonsForecast, list, PeopleSoft5DigitCode, DIM1_1, fileInfo.DIM2_Part3_RowStart, fileInfo.DIM2_Part3_RowEnd);
            parseGroup(worksheetTonsForecast, list, PeopleSoft5DigitCode, DIM1_1, fileInfo.DIM2_Part4_RowStart, fileInfo.DIM2_Part4_RowEnd);  

            worksheetTons = null;
            worksheetTons = null;
            workbook = null;

            return;
        }

        /// <summary>
        /// Creates an Excel document from the list of objects passed it (uses properties as columns in the Excel document)
        /// </summary>
        /// <param name="path">Path of output file</param>
        /// <param name="outputExcelFileName">Name of output file</param>
        /// <param name="fullList">data outputed into excel document</param>
        public void WriteToEXCEL(string path, string outputExcelFileName, List<C4RForecastExtractRow> fullList)
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            string pathAndFileName = path + outputExcelFileName;
            //Instantiate a new Workbook
            Workbook book = new Workbook();
            //Clear all the worksheets
            book.Worksheets.Clear();
            //Add a new Sheet "Data";
            Worksheet sheet = book.Worksheets.Add("Data");           
            sheet.Cells.ImportCustomObjects(fullList,
            new string[] { "PeopleSoft5DigitCode", "DIM1", "DIM2", "DIM3", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Total", "Budget", "Variance" },    
            true, 0, 0, fullList.Count, true, "dd/mm/yyyy", true);

            //Auto-fit all the columns
            book.Worksheets[0].AutoFitColumns();

            //Delete file if it already exists
            FileInfo fileToDelete = new FileInfo(pathAndFileName);
            if (fileToDelete.Exists) { fileToDelete.Delete(); }

            //Save the Excel file
            book.Save(pathAndFileName);

        }
       
        #endregion

        #region private methods

        private void parseGroup(Worksheet worksheetTonsForecast, List<C4RForecastExtractRow> list, string PeopleSoft5DigitCode, string DIM1, int start, int end)
        {
            if (start == 0) { return; }

            //load rows with DIM2 group 1
            for (int i = start; i <= end; i++)
            {
                int j = i - 1;
                C4RForecastExtractRow extractRow = new C4RForecastExtractRow();
                extractRow.PeopleSoft5DigitCode = PeopleSoft5DigitCode;
                extractRow.DIM1 = DIM1;
                extractRow.DIM2 = worksheetTonsForecast.Cells[start - 1, 1].StringValue;
                extractRow.DIM3 = worksheetTonsForecast.Cells[j, 2].StringValue;
                extractRow.Jan = worksheetTonsForecast.Cells[j, 3].StringValue;
                extractRow.Feb = worksheetTonsForecast.Cells[j, 4].StringValue;
                extractRow.Mar = worksheetTonsForecast.Cells[j, 5].StringValue;
                extractRow.Apr = worksheetTonsForecast.Cells[j, 6].StringValue;
                extractRow.May = worksheetTonsForecast.Cells[j, 7].StringValue;
                extractRow.Jun = worksheetTonsForecast.Cells[j, 8].StringValue;
                extractRow.Jul = worksheetTonsForecast.Cells[j, 9].StringValue;
                extractRow.Aug = worksheetTonsForecast.Cells[j, 10].StringValue;
                extractRow.Sep = worksheetTonsForecast.Cells[j, 11].StringValue;
                extractRow.Oct = worksheetTonsForecast.Cells[j, 12].StringValue;
                extractRow.Nov = worksheetTonsForecast.Cells[j, 13].StringValue;
                extractRow.Dec = worksheetTonsForecast.Cells[j, 14].StringValue;
                extractRow.Total = worksheetTonsForecast.Cells[j, 15].StringValue;
                extractRow.Budget = worksheetTonsForecast.Cells[j, 16].StringValue;
                extractRow.Variance = worksheetTonsForecast.Cells[j, 17].StringValue;

                list.Add(extractRow);
                extractRow = null;
            }
        }

        /// <summary>
        /// Formats the exception message
        /// </summary>      
        /// <param name="e">The exception which was thrown</param>
        /// <returns>Formatted text describing the exception</returns>
        private string formatExceptionText(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("An Exception has occurred.");
            sb.Append("\n\n");
            sb.Append("Message: " + e.Message);
            sb.Append("\n");
            sb.Append("Source: " + e.Source);

            return sb.ToString();
        }

        #endregion

    }
}
