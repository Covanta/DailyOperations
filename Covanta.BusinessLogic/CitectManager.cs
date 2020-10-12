using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;
using Aspose.Cells;
using System.IO;
using System.Configuration;

namespace Covanta.BusinessLogic
{
    public class CitectManager
    {
        #region private variables

        string _excelPathOut = ConfigurationManager.AppSettings["ExcelPath"];
        string _excelFileOut = ConfigurationManager.AppSettings["ExcelFile"];

        string _tempPath = ConfigurationManager.AppSettings["TempPath"];

        //Hempstead
        string _hempsteadSourcePath = ConfigurationManager.AppSettings["HempsteadSourcePath"];
        string _hempsteadFile = ConfigurationManager.AppSettings["HempsteadFile"];
        //Babylon
        string _babylonSourcePath = ConfigurationManager.AppSettings["BabylonSourcePath"];
        string _babylonFile = ConfigurationManager.AppSettings["BabylonFile"];
        //Essex
        string _essexSourcePath = ConfigurationManager.AppSettings["EssexSourcePath"];
        string _essexFile = ConfigurationManager.AppSettings["EssexFile"];
        //Huntington
        string _huntingtonSourcePath = ConfigurationManager.AppSettings["HuntingtonSourcePath"];
        string _huntingtonFile = ConfigurationManager.AppSettings["HuntingtonFile"];
        //MacArthur
        string _macArthurPath = ConfigurationManager.AppSettings["MacArthurSourcePath"];
        string _macArthurFile = ConfigurationManager.AppSettings["MacArthurFile"];
        //Union
        string _unionPath = ConfigurationManager.AppSettings["UnionPath"];
        string _unionFile = ConfigurationManager.AppSettings["UnionFile"];

        #endregion

        #region public methods

        public void ProcessFiles()
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            Workbook wbDestination = new Workbook();
            File.Delete(_excelPathOut + _excelFileOut); ;

            //Delete files and Copy Files to Temp Paths
            deleteAndCopyFilesBeforeProcessing();

            //Insert Summary sheets to Destination Excel

            // Hempstead
            Workbook wbSource = new Workbook(_tempPath + _hempsteadFile);
            wbSource = new Workbook(_tempPath + _hempsteadFile);
            string facilityName = "Hempstead";
            copyToExcelWorkbook(wbSource, "Daily Summary", wbDestination, facilityName, facilityName);

            // Babylon
            wbSource = new Workbook(_tempPath + _babylonFile);
            facilityName = "Babylon";
            copyToExcelWorkbook(wbSource, "DAILY", wbDestination, facilityName, facilityName);

            // Essex           
            wbSource = new Workbook(_tempPath + _essexFile);
            facilityName = "Essex";
            copyToExcelWorkbook(wbSource, "Daily Summary", wbDestination, facilityName, facilityName);

            // Huntington            
            wbSource = new Workbook(_tempPath + _huntingtonFile);
            facilityName = "Huntington";
            copyToExcelWorkbook(wbSource, "DAILY", wbDestination, facilityName, facilityName);

            // MacArthur          
            wbSource = new Workbook(_tempPath + _macArthurFile);
            facilityName = "MacArthur";
            copyToExcelWorkbook(wbSource, "Daily", wbDestination, facilityName, facilityName);

            // Union           
            wbSource = new Workbook(_tempPath + _unionFile);
            facilityName = "Union";
            copyToExcelWorkbook(wbSource, "Daily", wbDestination, facilityName, facilityName);



            //Insert All Other Sheets to Destination Excel

            // Hempstead            
            wbSource = new Workbook(_tempPath + _hempsteadFile);
            facilityName = "Hempstead";
            copyToExcelWorkbook(wbSource, "Summary", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Boiler 1", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Boiler 2", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Boiler 3", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Turbine", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Fouling", wbDestination, facilityName);
            wbSource = null;

            // Babylon          
            wbSource = new Workbook(_tempPath + _babylonFile);
            facilityName = "Babylon";
            copyToExcelWorkbook(wbSource, "MONTHLY", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B1MONTH", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B2MONTH", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "BOPMONTH", wbDestination, facilityName);
            wbSource = null;

            // Essex           
            wbSource = new Workbook(_tempPath + _essexFile);
            facilityName = "Essex";
            copyToExcelWorkbook(wbSource, "Summary", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Boiler 1", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Boiler 2", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Boiler 3", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Turbine", wbDestination, facilityName);
            wbSource = null;

            // Huntington           
            wbSource = new Workbook(_tempPath + _huntingtonFile);
            facilityName = "Huntington";
            copyToExcelWorkbook(wbSource, "B1MONTH", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B2MONTH", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B3MONTH", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "TurbMonth", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "Monthly", wbDestination, facilityName);
            wbSource = null;

            // MacArthur          
            wbSource = new Workbook(_tempPath + _macArthurFile);
            facilityName = "MacArthur";
            copyToExcelWorkbook(wbSource, "Monthly", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B1Month", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B2Month", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "BOPMonth", wbDestination, facilityName);
            wbSource = null;

            // Union           
            wbSource = new Workbook(_tempPath + _unionFile);
            facilityName = "Union";
            copyToExcelWorkbook(wbSource, "B1Month", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "B2MONTH", wbDestination, facilityName);
            copyToExcelWorkbook(wbSource, "BOPMONTH", wbDestination, facilityName);
            wbSource = null;


            //Save WorkBook            
            wbDestination.Worksheets.ActiveSheetIndex = 1;                    
            wbDestination.Save(_excelPathOut + _excelFileOut);
            wbDestination.Save(_excelPathOut + _excelFileOut, SaveFormat.Xlsx);

            //Delete Temp Files
            deleteTempFiles();

        }

        #endregion

        #region private methods

        private void deleteAndCopyFilesBeforeProcessing()
        {
            // Hempstead
            deleteFiles(_tempPath, _hempsteadFile);
            copyFilesToTempLocation(_hempsteadSourcePath, _hempsteadFile, _tempPath);

            // Babylon
            deleteFiles(_tempPath, _babylonFile);
            copyFilesToTempLocation(_babylonSourcePath, _babylonFile, _tempPath);

            // Essex
            deleteFiles(_tempPath, _essexFile);
            copyFilesToTempLocation(_essexSourcePath, _essexFile, _tempPath);

            // Huntington
            deleteFiles(_tempPath, _huntingtonFile);
            copyFilesToTempLocation(_huntingtonSourcePath, _huntingtonFile, _tempPath);

            // MacArthur
            deleteFiles(_tempPath, _macArthurFile);
            copyFilesToTempLocation(_macArthurPath, _macArthurFile, _tempPath);

            // Union
            deleteFiles(_tempPath, _unionFile);
            copyFilesToTempLocation(_unionPath, _unionFile, _tempPath);
        }

        private void deleteTempFiles()
        {
            deleteFiles(_tempPath, _hempsteadFile);
            deleteFiles(_tempPath, _babylonFile);
            deleteFiles(_tempPath, _essexFile);
            deleteFiles(_tempPath, _huntingtonFile);
            deleteFiles(_tempPath, _macArthurFile);
            deleteFiles(_tempPath, _unionFile);
        }

        private void copyToExcelWorkbook(Workbook wbSource, string sheetName, Workbook wbDest, string facilityName)
        {
            copyToExcelWorkbook(wbSource, sheetName, wbDest, facilityName, string.Empty);
        }

        private void copyToExcelWorkbook(Workbook wbSource, string sheetName, Workbook wbDest, string facilityName, string newSheetName)
        {
            wbSource.RemoveMacro();
            Worksheet wsSource = wbSource.Worksheets[sheetName];
            Worksheet wsDest = null;
            //Add new sheet
            if (newSheetName == string.Empty)
            {
                wsDest = wbDest.Worksheets.Add(facilityName + " " + sheetName);
            }
            else
            {
                wsDest = wbDest.Worksheets.Add(newSheetName);
            }


            //Copy source sheet to Dest sheet           
            CopyOptions copyOption = new CopyOptions();
            copyOption.CopyInvalidFormulasAsValues = true;
            wsDest.Copy(wsSource, copyOption);

            //here
            //Get the protection in the sheet
            Protection protection = wsDest.Protection;

            //Restricting users to delete columns of the worksheet
            protection.AllowDeletingColumn = false;

            //Restricting users to delete row of the worksheet
            protection.AllowDeletingRow = false;

            //Restricting users to edit contents of the worksheet
            protection.AllowEditingContent = false;

            //Allowing users to edit objects of the worksheet
            protection.AllowEditingObject = true;

            //Allowing users to edit scenarios of the worksheet
            protection.AllowEditingScenario = true;

            //Restricting users to filter
            protection.AllowFiltering = false;

            //Allowing users to format cells of the worksheet
            protection.AllowFormattingCell = true;

            //Allowing users to format rows of the worksheet
            protection.AllowFormattingRow = true;

            //Allowing users to insert columns in the worksheet
            protection.AllowInsertingColumn = true;

            //Allowing users to insert hyperlinks in the worksheet
            protection.AllowInsertingHyperlink = true;

            //Allowing users to insert rows in the worksheet
            protection.AllowInsertingRow = true;

            //Allowing users to select locked cells of the worksheet
            protection.AllowSelectingLockedCell = true;

            //Allowing users to select unlocked cells of the worksheet
            protection.AllowSelectingUnlockedCell = true;

            //Allowing users to sort
            protection.AllowSorting = true;

            //Allowing users to use pivot tables in the worksheet
            protection.AllowUsingPivotTable = true;




            //here

         

            wsDest.ActiveCell = "A1";
            wsDest.FirstVisibleRow = 0;
            wsDest.FirstVisibleColumn = 0;
        }

        private void copyFilesToTempLocation(string sourcePath, string sourceFile, string tempPath)
        {
            DirectoryInfo dir = new DirectoryInfo(_hempsteadSourcePath);
            File.Copy(sourcePath + sourceFile, tempPath + sourceFile);
        }

        private void deleteFiles(string tempPath, string sourceFile)
        {
            FileInfo fileToDelete = new FileInfo(tempPath + sourceFile);
            if (fileToDelete.Exists)
            {
                fileToDelete.Delete();
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
