using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using Aspose.Cells;
using System.Net;
using Covanta.Entities;
using System.Data;
using Covanta.DataAccess;

namespace Covanta.BusinessLogic
{
    public class EnvironmentalEmissionsManager
    {
        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public EnvironmentalEmissionsManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the EnvironmentalEmissionsManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region private variables

        string _dbConnection = null;
        string _tempPath = ConfigurationManager.AppSettings["TempPath"];
        string _sheetDioxin = ConfigurationManager.AppSettings["SheetDioxin"];
        string _sheetMercury = ConfigurationManager.AppSettings["SheetMercury"];
        string _sheetPM = ConfigurationManager.AppSettings["SheetPM"];
        string _sheetHCl = ConfigurationManager.AppSettings["SheetHCl"];
        string _sheetSOx = ConfigurationManager.AppSettings["SheetSOx"];
        string _sheetNOx = ConfigurationManager.AppSettings["SheetNOx"];
        string _sheetBaselineAnalysis = ConfigurationManager.AppSettings["SheetBaselineAnalysis"];
    
        string _originalPath = string.Empty;
        string _fileName = string.Empty;
        DateTime _documentLastModifiedDate;
        
        List<EnvEmissionData> _envEmissionDataList = new List<EnvEmissionData>();

        #endregion

        #region public methods

        public void ProcessFiles()
        {
            deleteDataFromDatabase();

            //_originalPath = ConfigurationManager.AppSettings["OriginalPath"];

            _originalPath = ConfigurationManager.AppSettings["EnvironmentalEmissionsExcel_URL"];
            _fileName = ConfigurationManager.AppSettings["EnvironmentalEmissionsExcelFile"];
           
            ProcessEachFile();
            
        }

        #endregion

        #region private methods

        private void ProcessEachFile()
        {
            //delete temp files and copy source files to temp location
            deleteFiles(_tempPath, _fileName);
            copyFilesToTempLocation(_originalPath, _fileName, _tempPath);
            processActuals();
        }


        private void processActuals()
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            // load Excel into a sheet collection    
            Workbook wbSource = new Workbook(_tempPath + _fileName);
            WorksheetCollection wsc = wbSource.Worksheets;

            _documentLastModifiedDate = wsc.BuiltInDocumentProperties.LastSavedTime;            
            parseExcelSheet(wsc);         
            insertListToDatabase();
        }
       
        private bool tryparseMinTons(string x)
        {
            int value = 0;
            return int.TryParse(x, out value);
        }
                  
        private void insertListToDatabase()
        {
            DALEnvironmental dal = new DALEnvironmental(_dbConnection);
            dal.InsertEnvironmentalDataListToDatabase(_envEmissionDataList);       
        }

        private void parseExcelSheet(WorksheetCollection wsc)
        {
            // Parse the cells from each sheet to a EnvEmissionData object and then add that object to a list of EnvEmissionData objects
            DALEnvironmental dal = new DALEnvironmental(_dbConnection);

            // call data layer and parse data table into a list of EnvEmissionData objects            
            _envEmissionDataList.Clear();
            foreach (Worksheet sheet in wsc)
            {
                if (sheet.Name == _sheetDioxin)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableEmissionTypes(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }
                if (sheet.Name == _sheetMercury)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableEmissionTypes(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }
                if (sheet.Name == _sheetPM)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableEmissionTypes(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }
                if (sheet.Name == _sheetHCl)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableEmissionTypes(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }
                if (sheet.Name == _sheetSOx)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableEmissionTypes(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }
                if (sheet.Name == _sheetNOx)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableEmissionTypes(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }

                // BaseLine Analysis
                if (sheet.Name == _sheetBaselineAnalysis)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    dal.ParseEnvironmentalDataTableBaseline(dt, _envEmissionDataList, sheet.Name, _documentLastModifiedDate);
                }
            }
        }

        private void deleteDataFromDatabase()
        {
            DALEnvironmental dal = new DALEnvironmental(_dbConnection);
            dal.DeleteAllFromEmissionsData();      
        }

        private void copyFilesToTempLocation(string sourcePath, string sourceFile, string tempPath)
        {
         //   File.Copy(sourcePath + sourceFile, tempPath + sourceFile);

            string remoteUri = sourcePath;
            string fileName = sourceFile, myStringWebResource = null;

            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            myWebClient.UseDefaultCredentials = true;

            // Concatenate the domain with the Web resource filename.
            myStringWebResource = remoteUri + fileName;

            try
            {
                myWebClient.DownloadFile(myStringWebResource, tempPath + sourceFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error while copying file " + myStringWebResource, ex);
            }
        }

        private void deleteFiles(string tempPath, string sourceFile)
        {
            FileInfo fileToDelete = new FileInfo(tempPath + sourceFile);
            if (fileToDelete.Exists)
            {
                fileToDelete.Delete();
            }

        }

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
