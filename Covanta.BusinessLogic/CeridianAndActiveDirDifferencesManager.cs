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
    public class CeridianAndActiveDirDifferencesManager
    {
        #region private variables

        string _dbConnection = null;

        #endregion



        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public CeridianAndActiveDirDifferencesManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the CeridianAndActiveDirDifferencesManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region public methods
        public List<CeridianAndADSyncInputRow> GetCeridianAndADSyncInputRowList()
        {
            List<CeridianAndADSyncInputRow> fullList = null;
            try
            {
                DALCeridianADSync dal = new DALCeridianADSync(_dbConnection);
                fullList = dal.GetCeridianAndADSyncInputRowList();
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return fullList;
        }

        public List<CeridianAndADSyncProposedSAMwithNoConflict> GetCeridianAndADSyncProposedSAMwithNoConflictList()
        {
            List<CeridianAndADSyncProposedSAMwithNoConflict> list = new List<CeridianAndADSyncProposedSAMwithNoConflict>();
            try
            {
                DALCeridianADSync dal = new DALCeridianADSync(_dbConnection);
                list = dal.GetCeridianAndADProposedSAMwithNoConflicts();
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return list;
        }

        public void WriteOutputRowsToDatabase(List<CeridianAndADSyncOututRow> list)
        {
            DALCeridianADSync dal = new DALCeridianADSync(_dbConnection);
            dal.TruncateActiveDirAndCeridianDifferencesTable();
            dal.InsertCeridianAndADSyncOutputRow(list);
        }

        public void InsertIntoProposedSamAccountNamesTable()
        {
            DALCeridianADSync dal = new DALCeridianADSync(_dbConnection);
            dal.InsertIntoProposedSamAccountNamesTable();
        }

        public void WriteCeridianAndADSyncProposedSAMwithNoConflictListToCSV(List<CeridianAndADSyncProposedSAMwithNoConflict> list, string CSVPathAndFile)
        {           
          //  FileInfo fileToDelete = new FileInfo(@"c:\BrianTest\ImportedCustomObjects.csv");
            FileInfo fileToDelete = new FileInfo(CSVPathAndFile);
            if (fileToDelete.Exists) { fileToDelete.Delete(); }

            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            //Instantiate a new Workbook
            Workbook book = new Workbook();
            //Clear all the worksheets
            book.Worksheets.Clear();
            //Add a new Sheet "Data";
            Worksheet sheet = book.Worksheets.Add("Data");

            //We pick a few columns not all to import to the worksheet
            ImportTableOptions x = new ImportTableOptions();
            x.InsertRows = false;
            sheet.Cells.ImportCustomObjects((System.Collections.ICollection)list, 0, 0, x);

            //Auto-fit all the columns
            book.Worksheets[0].AutoFitColumns();
            //Save the Excel file
          //  book.Save(@"c:\BrianTest\ImportedCustomObjects.xls");
            book.Save(CSVPathAndFile);
        }
        #endregion

        #region private methods

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
