using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;
using Covanta.DataAccess;

namespace Covanta.BusinessLogic
{
    public class WorkdayOperatingUnitsManager
    {
        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public WorkdayOperatingUnitsManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the WorkdayOperatingUnitsManager was null or empty");
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
        string _tempFile = ConfigurationManager.AppSettings["TempFile"];
        string _tempFileExtension = ConfigurationManager.AppSettings["TempFileExtension"];

        #endregion

        #region public methods
        public void ProcessFiles()
        {
            //Instantiate an instance of license and set the license file through its path
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Total.lic");

            string fullName = _tempPath + _tempFile + _tempFileExtension;

            TxtLoadOptions loadOptions = new TxtLoadOptions(LoadFormat.CSV);
            loadOptions.ConvertNumericData = false;
            Workbook workbook = new Workbook(fullName, loadOptions);
            Worksheet sheet = workbook.Worksheets[0];
            object[,] dataArray = sheet.Cells.ExportArray(0, 0, sheet.Cells.MaxDataRow + 1, sheet.Cells.MaxDataColumn + 1);

            DALWorkdayOperatingUnits dal = new DALWorkdayOperatingUnits(_dbConnection);
            dal.DeleteWorkdayOperatingUnitsFromSQLServer();
            dal.InsertWorkdayOperatingUnitsToSQLServer(dataArray);
        }
        #endregion

    }
}
