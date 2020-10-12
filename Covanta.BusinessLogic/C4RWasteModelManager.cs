using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;
using Aspose.Cells;
using System.IO;
using System.Configuration;
using System.Data;
using System.Net;

namespace Covanta.BusinessLogic
{
    public class C4RWasteModelManager
    {

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public C4RWasteModelManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the C4RWasteModelManager was null or empty");
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
        string _sheetName1 = ConfigurationManager.AppSettings["SheetName1"];
                
        string _C4RWasteModelOriginalPath = string.Empty;
        string _wasteModelFileName = string.Empty;
        string _minTonsString = string.Empty;
        string _currentPlantCode = string.Empty;
        int _minTonsInt = 0;
        DateTime _documentLastModifiedDate = DateTime.Today;
        

        enum WeeksAheadFlag
        {
            Three,
            Six
        };


        List<C4RWasteModel> _wasteModelList = new List<C4RWasteModel>();

        #endregion

        #region public methods

        public void ProcessFiles()
        {

            deleteWasteModelDataFromDatabase();

            //INDY
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathINDY"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameINDYY"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_INDYY"];
            _currentPlantCode = "INDYY";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //SEMASS
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathSEMASS"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameSEMASS"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_SEMASS"];
            _currentPlantCode = "SEMASS";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();


            //PITTS
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathPITTS"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNamePITTS"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_PITTS"];
            _currentPlantCode = "PITTS";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //SPRIN
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathSPRIN"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameSPRIN"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_SPRIN"];
            _currentPlantCode = "SPRIN";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //NIAGA
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathNIAGA"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameNIAGA"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_NIAGA"];
            _currentPlantCode = "NIAGA";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //TULSA
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathTULSA"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameTULSA"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_TULSA"];
            _currentPlantCode = "TULSA";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //HAVER
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathHAVER"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameHAVER"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_HAVER"];
            _currentPlantCode = "HAVER";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //SECON
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathSECON"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameSECON"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_SECON"];
            _currentPlantCode = "SECON";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();

            //WALLI
            _C4RWasteModelOriginalPath = ConfigurationManager.AppSettings["C4RWasteModelOriginalPathWALLI"];
            _wasteModelFileName = ConfigurationManager.AppSettings["WasteModelFileNameWALLI"];
            _minTonsString = ConfigurationManager.AppSettings["Min_Tons_WALLI"];
            _currentPlantCode = "WALLI";
            _minTonsInt = tryparseMinTons(_minTonsString) ? Convert.ToInt32(_minTonsString) : 0;
            ProcessEachFile();
            
        }
        
        #endregion

        #region private methods
        
        private void processActuals()
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            // load Excel into a sheet collection    
            Workbook wbSource = new Workbook(_tempPath + _wasteModelFileName);
            WorksheetCollection wsc = wbSource.Worksheets;

            _documentLastModifiedDate = wsc.BuiltInDocumentProperties.LastSavedTime;
            //Load _wasteModelList with data from excel sheet
            parseExcelSheetIntoWasteList(wsc);

            // Do trasformations to add additional columns to the object.
            // for example:  isWithin6Weeks

            addAdditionalColumnsToWasteModelObject();

            // Insert list into the database
            insertListToDatabase();
        }

        private void ProcessEachFile()
        {
            //delete temp files and copy source files to temp location
            deleteFiles(_tempPath, _wasteModelFileName);

            copyFilesToTempLocation(_C4RWasteModelOriginalPath, _wasteModelFileName, _tempPath);

            processActuals();

        }


        private void addAdditionalColumnsToWasteModelObject()
        {            
            addPriorMonday();
            addWithinXWeeksFlags();
            addNextDateAtBelowMinTons();
            setIsDateInPastFlag();
            setIsDateTodayFlag();
            addNextTonsAtBelowMinTons();
            addLastModifiesDate(); 
        }
        
        private bool tryparseMinTons(string x)
        {
            int value = 0;
            return int.TryParse(x, out value);
        }
           
        private void setIsDateInPastFlag()
        {
            foreach (C4RWasteModel model in _wasteModelList)
            {
                model.isDateInPast = DateTime.Today > model.Date1 ? true : false;
            }
        }

        private void setIsDateTodayFlag()
        {

            foreach (C4RWasteModel model in _wasteModelList)
            {
                model.isDateToday = DateTime.Today == model.Date1 ? true : false;
            }
        }

        private void addNextTonsAtBelowMinTons()
        {
            DateTime nextMinDate = DateTime.Parse("01/01/2025");
            int nextMinTons = 0;
            bool foundMinDate = false;
            foreach (C4RWasteModel model in _wasteModelList)
            {
                if ((model.isDateInPast == false) && (model.Next_3_WeeksHasValueBelowMin) && (foundMinDate == false))
                {
                    nextMinDate = model.NextDateBelowMinTons;
                    foundMinDate = true;
                }
            }
            for (int i = 0; i < 364; i++)
            {
                if (_wasteModelList[i].Date1 == nextMinDate)
                {
                    nextMinTons = _wasteModelList[i].ModelCalcPitInventory;
                    i = 500;
                }
            }
            foreach (C4RWasteModel model in _wasteModelList)
            {
                model.NextTonsBelowMinTons = nextMinTons;
            }
        }

        private void addLastModifiesDate()
        {
            foreach (C4RWasteModel model in _wasteModelList)
            {
                model.DocumentLastModifiedDate = _documentLastModifiedDate;
            }
        }

        private void addNextDateAtBelowMinTons()
        {
            foreach (C4RWasteModel model in _wasteModelList)
            {
                model.NextDateBelowMinTons = DateTime.Parse("01/01/2025");
                model.NextTonsBelowMinTons = 0;
                model.MinTons = _minTonsInt;
                if (model.Location == _currentPlantCode)
                {
                    model.isBelowMinTons = (model.ModelCalcPitInventory < _minTonsInt) ? true : false;
                }
            }

            // get only a subset of the list (Just the one location)
            List<C4RWasteModel> list = _wasteModelList.FindAll(obj => obj.Location == _currentPlantCode);
            //sort the list by date
            list = list.OrderBy(o => o.Date1).ToList();
            if (list.Count == 0) { return; }

         
            // i represents each day on the list
            for (int i = 0; i < 364; i++)
            {
                //j represents each of the next 21 days               
                for (int j = 1; j < 22; j++)
                {
                    if (i + j < 365)
                    {
                        if (list[i + j].isBelowMinTons)
                        {
                            list[i].NextDateBelowMinTons = list[i + j].Date1;
                            list[i].Next_3_WeeksHasValueBelowMin = true;
                          
                            j = 500;
                        }
                    }
                }
            }

          
        }

        private void addWithinXWeeksFlags()
        {
            int multiplier = 7;
            foreach (C4RWasteModel model in _wasteModelList)
            {
                setWithinXWeeksFlag(model, multiplier * 3, WeeksAheadFlag.Three);
                setWithinXWeeksFlag(model, multiplier * 6, WeeksAheadFlag.Six);
            }
        }

        private void setWithinXWeeksFlag(C4RWasteModel model, int days, WeeksAheadFlag weeksAheadFlag)
        {
            //// if we are not in current year, then just return
            //if (model.PriorMonday.Year != model.Date1.Year)
            //{
            //    model.isWithin_3_Weeks = true;
            //    model.isWithin_6_Weeks = true;
            //    return;
            //}
                        
            //DateTime currentDate = DateTime.Today < DateTime.Parse("2014,01,01") ? DateTime.Parse("2014,01,01") : DateTime.Today;
                        
            DateTime currentDate = DateTime.Today;
            if (currentDate > model.Date1)
           //if (currentDate > model.PriorMonday)
            {
                model.isWithin_Next_3_Weeks = false;
                model.isWithin_Next_6_Weeks = false;
                return;
            }
                      
            TimeSpan t = model.Date1 - currentDate;
            //TimeSpan t = model.PriorMonday - currentDate;
            double numOfDays = t.TotalDays ;

            switch (weeksAheadFlag)
            {
                case WeeksAheadFlag.Three:
                    model.isWithin_Next_3_Weeks = numOfDays > days ? false : true;
                    break;
                case WeeksAheadFlag.Six:
                    model.isWithin_Next_6_Weeks = numOfDays > days ? false : true;
                    break;
                default:
                    return;
            }
        }

        private void addPriorMonday()
        {
            DateTime today = DateTime.Today;
            foreach (C4RWasteModel model in _wasteModelList)
            {
                model.PriorMonday = GetMonday(model.Date1);
            }
        }

        public static DateTime GetMonday(DateTime x)
        {
            if (x.DayOfWeek == DayOfWeek.Monday) return x;
            if (x.DayOfWeek == DayOfWeek.Tuesday) return x.AddDays(-1);
            if (x.DayOfWeek == DayOfWeek.Wednesday) return x.AddDays(-2);
            if (x.DayOfWeek == DayOfWeek.Thursday) return x.AddDays(-3);
            if (x.DayOfWeek == DayOfWeek.Friday) return x.AddDays(-4);
            if (x.DayOfWeek == DayOfWeek.Saturday) return x.AddDays(-5);
            return x.AddDays(-6);
        }

        private void insertListToDatabase()
        {
            DALC4RWasteModel dal = new DALC4RWasteModel(_dbConnection);
            dal.InsertC4RWasteModelListToDatabase(_wasteModelList);
        }

        private void parseExcelSheetIntoWasteList(WorksheetCollection wsc)
        {
            // Parse the cells from each sheet to a C4RWasteModel object and then add that object to a list of C4RWasteModel objects
            DALC4RWasteModel dal = new DALC4RWasteModel(_dbConnection);

            // call data layer and parse data table into a list of C4RWasteModel objects            
            _wasteModelList.Clear();
            foreach (Worksheet sheet in wsc)
            {
                if (sheet.Name == _sheetName1)
                {
                    DataTable dt = new DataTable();
                    sheet.Cells.DeleteBlankRows();
                    dt = sheet.Cells.ExportDataTableAsString(1, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);
                    //string location = getLocationCodeFromSheet(sheet);
                    string location = _currentPlantCode;
                    dal.ParseC4RWasteModel_ActualDataTable(dt, _wasteModelList, location);
                }
            }
        }
          
        private void deleteWasteModelDataFromDatabase()
        {
            DALC4RWasteModel dal = new DALC4RWasteModel(_dbConnection);
            dal.DeleteAllFromC4RWasteModel();
        }

        private void copyFilesToTempLocation(string sourcePath, string sourceFile, string tempPath)
        {
            //File.Copy(sourcePath + sourceFile, tempPath + sourceFile);

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
