using Aspose.Cells;
using Covanta.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Covanta.BusinessLogic
{
    public class EnergyMeterManager
    {
        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public EnergyMeterManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the EnvironmentalEmissionsManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
                dal = new DALEnergyMeter(_dbConnection);
            }
        }
        #endregion

        #region private variables

        string _dbConnection = null;
        string _originalPath = string.Empty;
        string _tempPath = string.Empty;
        List<string> _fileOnDatabase = new List<string>();
        object[,] dataArray = null;
        DALEnergyMeter dal = null;

        #endregion

        #region public methods

        public void Process()
        {
            _originalPath = ConfigurationManager.AppSettings["OriginalPath"];
            _tempPath = ConfigurationManager.AppSettings["TempPath"];

            moveFilesToTempLocation();

            string location = "LAKEE";
            parseFilesAndLoadToDatabase(location);
            //records were inserted with null for the deltas.  Now go back and update the deltas.
            dal.UpdateEnergyTableWithDeltaValues(location);            
        }
        
        #endregion

        #region private methods

        private void moveFilesToTempLocation()
        {
            //delete temp files  
            Array.ForEach(Directory.GetFiles(_tempPath), File.Delete);

            //read database to get list of all files already processed.
            _fileOnDatabase = dal.GetProcessedFileList();

            //move unprocessed files to temp location.  Delete files which were already processed.           
            moveNewAndDeleteOldFiles(_originalPath, _tempPath);           
        }
        
        private void parseFilesAndLoadToDatabase(string location)
        {
            DirectoryInfo dir = new DirectoryInfo(_tempPath);
            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Destination Temp directory does not exist or could not be found: "
                    + _tempPath);
            }
            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                loadArrayUsingAspose(file.Name, location);
                //Inset to Database
                dal.InsertEnergyMeterToSQLServer(dataArray, location, file.Name);
            }
        }
        
        /// <summary>
        /// The FTP process will move all files again.  We only want to process files which we haven't processed yet. This method moves or deletes each file pulled from the FTP server.
        /// </summary>
        /// <param name="sourceDirName">Source path</param>
        /// <param name="destDirName">Dest Path</param>      
        private void moveNewAndDeleteOldFiles(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // if the database doesn't already contain the file, then copy it to the temp location
                if (_fileOnDatabase.Contains(file.Name) == false)
                {
                    // Create the path to the new copy of the file.
                    string temppath = Path.Combine(destDirName, file.Name);

                    // Copy the file.
                    //  file.CopyTo(temppath, false);
                    file.MoveTo(temppath);
                }
                else
                {
                    file.Delete();
                }
            }
        }

        private void loadArrayUsingAspose(string fileName, string location)
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");
            string fullName = _tempPath + fileName;
            TxtLoadOptions loadOptions = new TxtLoadOptions(LoadFormat.CSV);
            loadOptions.ConvertNumericData = false;
            Workbook workbook = new Workbook(fullName, loadOptions);
            Worksheet sheet = workbook.Worksheets[0];
            dataArray = sheet.Cells.ExportArray(0, 0, sheet.Cells.MaxDataRow + 1, sheet.Cells.MaxDataColumn + 1);           
        }

        #endregion

    }
}
