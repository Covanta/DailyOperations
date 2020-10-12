using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Covanta.Common.Enums;
using Covanta.Entities;

namespace Covanta.Utilities.Helpers
{
    public class JobStatusHelper
    {
        #region private variables

        string _dbConnection = null;

        #endregion

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public JobStatusHelper(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the JobStatusHelper was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion
              
        #region public methods

        public void UpdateJobStatus(Enums.AutomatedJobNameEnum status)
        {
           
        }

        public void GetJobStatus(Enums.AutomatedJobNameEnum status)
        {

        }
        //public AutomatedJobDetails GetAutomatedJobDetails()
        //{
            
        //}

        #endregion

        #region private methods

        #endregion
    }
}
