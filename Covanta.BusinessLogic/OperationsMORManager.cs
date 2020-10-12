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

namespace Covanta.BusinessLogic
{
    public class OperationsMORManager
    {

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public OperationsMORManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the OperationsMORManager was null or empty");
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
        string _operationsMORPath = ConfigurationManager.AppSettings["OperationsMORPath"];       
        string _actualsfileName = ConfigurationManager.AppSettings["OperationsMORActualsfile"];
        string _budgetfileName = ConfigurationManager.AppSettings["OperationsMORBudgetfile"];        
        List<OperationsMOR_Actual> _MOR_ActualsList = new List<OperationsMOR_Actual>();     

        #endregion

        #region public methods

        public void ProcessFiles()
        {
            //delete temp files and copy source files (Budget and Actuals) to temp location
            deleteFiles(_tempPath, _actualsfileName);
            deleteFiles(_tempPath, _budgetfileName);
            copyFilesToTempLocation(_operationsMORPath, _actualsfileName, _tempPath);
            copyFilesToTempLocation(_operationsMORPath, _budgetfileName, _tempPath);

           // process Actuals
            processActuals();
            
        }

        private void processActuals()
        {
            // this stored proc need to change (currently doesnt do anything)
            deleteMORActualsFromDatabase();

            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            // read Excel into a sheet collection    
            Workbook wbSource = new Workbook(_tempPath + _actualsfileName);
            WorksheetCollection wsc = wbSource.Worksheets;


            // Parse the cells from each sheet to a OperationsMOR_Actual object and then add that object to a list of OperationsMOR_Actual objects
           
            DALOperationsData dal = new DALOperationsData(_dbConnection);
            
            // call data layer and parse data table into a list of OperationsMOR_Actual objects
            
            _MOR_ActualsList.Clear();
            foreach (Worksheet sheet in wsc)
            {
                DataTable dt = new DataTable();               
                dt = sheet.Cells.ExportDataTable(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1, true);              
                dal.ParseOperationsMOR_ActualDataTable(dt, _MOR_ActualsList);               
            }

            // Insert list of MOR Actuals into the database
            dal.InsertOperationsMOR_Actuals(_MOR_ActualsList);           
        }      

        private void deleteMORActualsFromDatabase()
        {
            DALOperationsData dal = new DALOperationsData(_dbConnection);
            dal.DeleteAllFromOperations_MORActuals();
        }

        #endregion

        #region private methods
                     
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
