using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace Covanta.BusinessLogic
{
    public class ScorecardsToPDFManager
    {
        #region constructors

        public ScorecardsToPDFManager() { }
        #endregion

        #region private variables

        string _tempPath = ConfigurationManager.AppSettings["TempPath"];
        string _scorecardsPath = ConfigurationManager.AppSettings["ScorecardsPath"];
        string _scorecardfileName = ConfigurationManager.AppSettings["Scorecardfile"];
        string _scorecardfileExtension = ConfigurationManager.AppSettings["ScorecardfileExtension"];
        string _scorecardFileSaveExtension = ConfigurationManager.AppSettings["ScorecardFileSaveExtension"];

        string outFile_1 = ConfigurationManager.AppSettings["outFile_1"];
        string outFile_2 = ConfigurationManager.AppSettings["outFile_2"];
        string outFile_3 = ConfigurationManager.AppSettings["outFile_3"];
        string outFile_4 = ConfigurationManager.AppSettings["outFile_4"];
        string outFile_5 = ConfigurationManager.AppSettings["outFile_5"];
        string outFile_6 = ConfigurationManager.AppSettings["outFile_6"];
        string outFile_7 = ConfigurationManager.AppSettings["outFile_7"];
        string outFile_8 = ConfigurationManager.AppSettings["outFile_8"];
        string outFile_9 = ConfigurationManager.AppSettings["outFile_9"];
        string outFile_10 = ConfigurationManager.AppSettings["outFile_10"];
        string outFile_11 = ConfigurationManager.AppSettings["outFile_11"];
        string outFile_12 = ConfigurationManager.AppSettings["outFile_12"];


        #endregion

        #region public methods
        public void ProcessFiles()
        {
            //delete temp files and copy source files to temp location
            deleteFiles(_tempPath, _scorecardfileName + _scorecardfileExtension);           
            copyFilesToTempLocation(_scorecardsPath, _scorecardfileName + _scorecardfileExtension, _tempPath);

            // process Actuals
            processData();
        }
        #endregion

        #region private methods
        private void processData()
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            // read Excel into a sheet collection    
            Workbook wbSource = new Workbook(_tempPath + _scorecardfileName + _scorecardfileExtension);
            WorksheetCollection wsc = wbSource.Worksheets;
            int i = -1;

            foreach (Worksheet sheet in wsc)
            {
                i++;
                if (   (sheet.Name == outFile_1)
                    || (sheet.Name == outFile_2)
                    || (sheet.Name == outFile_3)
                    || (sheet.Name == outFile_4)
                    || (sheet.Name == outFile_5)
                    || (sheet.Name == outFile_6)
                    || (sheet.Name == outFile_7)
                    || (sheet.Name == outFile_8)
                    || (sheet.Name == outFile_9)
                    || (sheet.Name == outFile_10)
                    || (sheet.Name == outFile_11)
                    || (sheet.Name == outFile_12)                   
                    )
                {
                    deleteCopyAndSaveSheet(wbSource, i, sheet);
                }

                if (sheet.Name == outFile_2) { deleteCopyAndSaveSheet(wbSource, i, sheet); }
            }
        }

        private void deleteCopyAndSaveSheet(Workbook wbSource, int i, Worksheet sheet)
        {
            deleteFiles(_tempPath, _scorecardfileName + " " + sheet.Name + _scorecardFileSaveExtension);

            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            Workbook singleSheet = new Workbook();

            singleSheet.Worksheets[0].Copy(wbSource.Worksheets[i]);

            //Save the document in PDF format
            singleSheet.Save(_tempPath + _scorecardfileName + " " + sheet.Name + _scorecardFileSaveExtension, SaveFormat.Pdf);
        }

        private void copyFilesToTempLocation(string sourcePath, string sourceFile, string tempPath)
        {
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
        #endregion
    }
}
