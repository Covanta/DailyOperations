using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.DataAccess;
using Covanta.Entities;

namespace Covanta.BusinessLogic
{
   public class ServiceNowExtractVIPManager
    {
       #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
       public ServiceNowExtractVIPManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the ServiceNowExtractVIPManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
       #endregion

       #region private variables

       string _dbConnection = null;     

       #endregion

       #region public methods

       public void SaveServiceNowVIPData(List<ServiceNowVIP> list)
       {
           DALServiceNow dal = new DALServiceNow(_dbConnection);
           dal.InsertServiceNowVIPToDatabase(list);
       }
       public void DeleteServiceNowVIPData()
       {
           DALServiceNow dal = new DALServiceNow(_dbConnection);
           dal.DeleteAllServiceNowVIPData();
       }

       #endregion
    }
}
