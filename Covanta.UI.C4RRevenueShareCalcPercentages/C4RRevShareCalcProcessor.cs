using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Covanta.BusinessLogic;
using Covanta.Entities;
using Covanta.Utilities.Helpers;
//using Covanta.BusinessLogic;

namespace Covanta.UI.C4RRevenueShareCalcPercentages
{
    public class C4RRevShareCalcProcessor
    {
        #region private variables

        string _covmetadataConnection;
        string _oracleConnection;
        List<C4RRevShareJournalData> _C4RRevShareJournalDataList = null;

        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public C4RRevShareCalcProcessor()
        {
            _covmetadataConnection = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
            _oracleConnection = ConfigurationManager.AppSettings["oracleConnString"];  
        }

        #endregion

        #region public methods

        /// <summary>
        /// Main method of the class.  Only public method.
        /// </summary>
        public void ProcessJob()
        {
            getJournal_Data_Rows();
            truncateCovmetatableWildcardC4RRevSharePercentages();
            insertJournalRowsToSQLServer();

            sendCompletedEmail();
        }

       
     

        #endregion

        #region private methods

        private void getJournal_Data_Rows()
        {
            C4RRevShareCalcPercentageManager  manager = new C4RRevShareCalcPercentageManager(_covmetadataConnection, _oracleConnection);
            _C4RRevShareJournalDataList = manager.GetC4RRevShareJournalData();
            manager = null;
        }

        private void truncateCovmetatableWildcardC4RRevSharePercentages()
        {
            C4RRevShareCalcPercentageManager manager = new C4RRevShareCalcPercentageManager(_covmetadataConnection, _oracleConnection);
            manager.TruncateCovmetatableWildcardC4RRevSharePercentages();
        }

        private void insertJournalRowsToSQLServer()
        {
            C4RRevShareCalcPercentageManager manager = new C4RRevShareCalcPercentageManager(_covmetadataConnection, _oracleConnection);
            manager.InsertJournalRowsToSQLServer(_C4RRevShareJournalDataList);
        }


        private void sendCompletedEmail()
        {
            EmailHelper.SendEmail("C4RRevenueShareCalcPercentages processor completed.");               
        }     

        #endregion
    }
}
