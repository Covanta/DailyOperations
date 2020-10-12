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
    public class WorkDayPayManager
    {

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public WorkDayPayManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the WorkDayPayManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region private variables

        string _dbConnection = null;

        List<Workday_MonthlyAccrualDays> _workday_MonthlyAccrualDaysList = new List<Workday_MonthlyAccrualDays>();     

        #endregion

        #region public methods

        public void Process()
        {
            getMonthlyAccrualDatesList();


        }

        private void getMonthlyAccrualDatesList()   
        {
            _workday_MonthlyAccrualDaysList.Clear();
            try
            {
                DALWorkDayPayAccrualETL dal = new DALWorkDayPayAccrualETL(_dbConnection);
                _workday_MonthlyAccrualDaysList = dal.GetWorkday_MonthlyAccrualDaysList();          
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }         
        }
      

      

        #endregion

        #region private methods
                     
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
