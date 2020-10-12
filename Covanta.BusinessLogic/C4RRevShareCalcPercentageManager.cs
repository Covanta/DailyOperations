using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;

namespace Covanta.BusinessLogic
{
    public class C4RRevShareCalcPercentageManager
    {
        #region private variables

        string _dbConnection = null;
        string _dbConnectionOracle = null;

        #endregion

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public C4RRevShareCalcPercentageManager(string dbConnection,string dbConnectionOracle)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the C4RRevShareCalcPercentageManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
                _dbConnectionOracle = dbConnectionOracle;
            }
        }
        #endregion

        #region public methods

        /// <summary>
        /// Creates a list of Journal Data objects
        /// </summary>
        /// <returns>list is Journal records</returns>
        public List<C4RRevShareJournalData> GetC4RRevShareJournalData()
        {
            List<C4RRevShareJournalData> list = null;
            try
            {
                DALC4RRevShareCalcPercentage dal = new DALC4RRevShareCalcPercentage(_dbConnection, _dbConnectionOracle);
                list = dal.GetC4RRevShareJournalDataListFromOracle();              
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return list;
        }

        public void TruncateCovmetatableWildcardC4RRevSharePercentages()
        {
            DALC4RRevShareCalcPercentage dal = new DALC4RRevShareCalcPercentage(_dbConnection, _dbConnectionOracle);
            dal.TruncateCovmetatableWildcardC4RRevSharePercentages();
        }

        public void InsertJournalRowsToSQLServer(List<C4RRevShareJournalData> list)
        {
            DALC4RRevShareCalcPercentage dal = new DALC4RRevShareCalcPercentage(_dbConnection, _dbConnectionOracle);
            dal.InsertJournalRowsToSQLServer(list);
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
