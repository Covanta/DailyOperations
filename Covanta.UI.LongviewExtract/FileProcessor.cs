using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using Covanta.BusinessLogic;
using Covanta.Utilities.Helpers;
using Covanta.Entities;
using Covanta.Common.Enums;

namespace Covanta.UI.LongviewExtract
{
    public class FileProcessor
    {
        public FileProcessor() { }

        #region Private variables

        string _connStringLongview = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;       
        string _longViewOPREVENUEFilesPath = ConfigurationManager.AppSettings["LongViewOPREVENUEFilesPath"];
        string _longViewTOTALTONSFilesPath = ConfigurationManager.AppSettings["LongViewTOTALTONSFilesPath"];
        string _longViewFINLSTATFilesPath = ConfigurationManager.AppSettings["LongViewFINLSTATFilesPath"];
        string _longViewSchema = ConfigurationManager.AppSettings["LongViewSchema"];
        bool factTableAlreadyCreated = false;
        string _currentWorkingPath = String.Empty;

        public enum enumFileBeingProcessed { LongViewRevenue, LongViewTons, FINLSTAT };

        string EmailTo = ConfigurationManager.AppSettings["EmailTo"];

        //
        #endregion

        #region Public methods

        /// <summary>
        /// This is the point of entry into this batch process
        /// </summary>
        public void ProcessFile(enumFileBeingProcessed fileBeingProcessed)
        {           
            string[] fileList = null;

            switch (fileBeingProcessed)
            {
                case enumFileBeingProcessed.LongViewRevenue:
                    _currentWorkingPath = _longViewOPREVENUEFilesPath;
                    break;

                    //this one is no longer used
                case enumFileBeingProcessed.LongViewTons:
                    _currentWorkingPath = _longViewTOTALTONSFilesPath;
                    break;


                case enumFileBeingProcessed.FINLSTAT:
                    _currentWorkingPath = _longViewFINLSTATFilesPath;
                    break;
                default:
                    EmailHelper.SendEmail("Longview Parser completed UnSuccessfully. Invalid File type requested.  Please choose Revenue or Tons.");
                    return;                    
            }
                       
            fileList = Directory.GetFiles(_currentWorkingPath);

            // Create LongviewExtractManager Object
            LongviewExtractManager extractManager = new LongviewExtractManager(_connStringLongview);

            //copy the files to an archive directory
            //copyFileToArchive(_currentWorkingPath);

            // Enumerate through the files and parse them out.
            foreach (string filepath in fileList)
            {
                #region comments
                // Use the name of the file to determine the "type" of file
                // File names are assumed to be of the form:
                //
                // DIMENSION FILE FORMAT = DIM_DimensionName.asc
                //      -  Example  DIM_ENTITY.asc
                // FACT TABLE FORMAT = FACT_OPREVENUE_TypeAndYearMonth_DateDataPulled.asc
                //      -  Example  FACT_OPREVENUE_F1201_2012-10-04.asc
                //
                // WHERE
                //     DIM
                //      - DimensionName = Dimension Name                 
                //     FACT               
                //      - TypeAndYearMonthData Represents = B or F or F2  + Year and Month
                //      - DateDataPulled = Date Longview exported data               

                #endregion


                string fileName = Path.GetFileName(filepath);
                string[] fileNameParts = fileName.Split('_');

                // Determine if this is a DIMension file or a FACT table file
                try
                {
                    switch (fileNameParts[0].ToUpper())
                    {
                        case "DIM":
                            Console.WriteLine("Processing DIM FILE: " + fileName);
                            extractManager.ParseDimFile(filepath);
                            break;
                        case "FACT":
                            Console.WriteLine("FACT FILE: " + fileName);
                            extractManager.ParseFactFile(filepath, _longViewSchema);
                            break;
                        default:
                            throw new Exception();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not figure out if this is a DIM file or a FACT file: " + fileName);
                }
            }
            Console.WriteLine("Press any key to continue...");
            
        }

        #endregion

        #region Private methods
               
        //private void copyFileToArchive(string path)
        //{
        //    DirectoryInfo dirInfo = new DirectoryInfo(path);
        //    FileInfo[] filesInDirectory = dirInfo.GetFiles();
        //    if (filesInDirectory.Length == 0) { return; }
        //    //Get date modified           
        //    DateTime modifiedDate = filesInDirectory[0].LastWriteTime;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(modifiedDate.Year.ToString());
        //    sb.Append("_");
        //    sb.Append(modifiedDate.Month.ToString());
        //    sb.Append("_");
        //    sb.Append(modifiedDate.Day.ToString());
        //    string newDirectory = path + "\\" + sb.ToString();

        //    if (Directory.Exists(newDirectory))
        //    {
        //        Directory.Delete(newDirectory, true);
        //    }
        //    Directory.CreateDirectory(newDirectory);

        //    foreach (FileInfo file in filesInDirectory)
        //    {
        //        File.Copy(file.FullName, newDirectory + "\\" + file.Name);
        //    }

        //}
    
        private void parseDimFile(string filePath)
        {
            int lineCount = 0;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Set the target table
                    string targetTableName = Path.GetFileNameWithoutExtension(filePath);
                    Console.WriteLine("Setting up the DB table: " + targetTableName);
                    //       DataAccess.SetDimensionTable(_connStringLongview, targetTableName);

                    // Parse each line in the file and put the information into the DB table
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;

                        //           DataAccess.InsertIntoDimensionTable(_connStringLongview, targetTableName, line, accountsList, businessUnitList);
                        if (lineCount % 100 == 0)
                        {
                            Console.WriteLine("\t- Processed " + lineCount + " lines");
                        }
                    }
                    Console.WriteLine("Trimming the DB table: " + targetTableName);
                    //          DataAccess.TrimDimensionTable(_connStringLongview, targetTableName);
                }
                Console.WriteLine("FINISHED! Processed " + lineCount + " lines");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while parsing file: " + filePath + " at line " + lineCount + " - ERROR: " + ex.Message);
            }
        }

        private void parseFactFile(string filePath)
        {
            int lineCount = 0;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Set the target table   (the name is FACT_????_??_?? Where ????_??_?? is the last 10 chars of the file name) 
                    string targetTableName = Path.GetFileNameWithoutExtension(filePath).Replace("-", "_");
                    targetTableName = "FACT_" + targetTableName.Substring(targetTableName.Length - 10);

                    //Only set up the FACT table once (all FACT tables are concatenated into one DB table)
                    if (!(factTableAlreadyCreated))
                    {
                        Console.WriteLine("Setting up the DB table: " + targetTableName);
                        //           DataAccess.SetFactTable(_connStringLongview, targetTableName);
                        factTableAlreadyCreated = true;
                    }

                    // Parse each line in the file and put the information into the DB table
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    //while (((line = sr.ReadLine()) != null) && (lineCount < 50))
                    {
                        lineCount++;
                        //              DataAccess.InsertIntoFactTable(_connStringLongview, targetTableName, line, lineCount);
                        if (lineCount % 2000 == 0)
                        {
                            Console.WriteLine("\t- Processed " + lineCount + " lines");
                            //break;
                        }
                    }
                }
                Console.WriteLine("FINISHED! Processed " + lineCount + " lines");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while parsing file: " + filePath + " at line " + lineCount + " - ERROR: " + ex.Message);
            }
        }

        //internal void sendCompletedEmail()
        //{
        //    EmailHelper.SendEmail("Longview Parser completed Successfully.");
        //}

        #endregion
    }
}
