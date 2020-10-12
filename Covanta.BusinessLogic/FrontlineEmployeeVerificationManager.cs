using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;


namespace Covanta.BusinessLogic
{
    public class FrontlineEmployeeVerificationManager
    {
        #region private variables

        string _dbConnection = null;

        #endregion

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public FrontlineEmployeeVerificationManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the FrontlineEmployeeVerificationManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region public methods
                
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
