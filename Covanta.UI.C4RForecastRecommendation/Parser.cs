using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using Covanta.Utilities.Helpers;
using Covanta.BusinessLogic;

namespace Covanta.UI.C4RForecastRecommendation
{
    public class Parser
    {
        #region private variables
      
        string _extractInputPath;
        string _extractInputFileName;
        string _extractOutputPath;
        string _extractOutputFileName;
        string _connStringCovIntegration = ConfigurationManager.ConnectionStrings["covIntegrationConnString"].ConnectionString;


        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public Parser()
        {
            _extractInputFileName = ConfigurationManager.AppSettings["SharePointInputFileName"];
            _extractInputPath = ConfigurationManager.AppSettings["SharePointInputPath"];

            _extractOutputFileName = ConfigurationManager.AppSettings["SharePointOutputFileName"];
            _extractOutputPath = ConfigurationManager.AppSettings["SharePointOutputPath"];
        }

        #endregion

        #region public methods

        /// <summary>
        /// Main method of the class.  Only public method.
        /// </summary>
        public void ProcessJob()
        {
            copyFileToOutputPath();
            //sendCompletedEmail();
            parseDataToDatabase();
        }

      

        #endregion

        #region private methods

        private void copyFileToOutputPath()
        {
            FileInfo outFile = new FileInfo(_extractOutputPath + _extractOutputFileName);
            if (outFile.Exists) { outFile.Delete(); }
            
            string remoteUri = _extractInputPath;
            string fileName = _extractInputFileName, myStringWebResource = null;

            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            myWebClient.UseDefaultCredentials = true;

            // Concatenate the domain with the Web resource filename.
            myStringWebResource = remoteUri + fileName;

            try
            {
                myWebClient.DownloadFile(myStringWebResource, _extractOutputPath + _extractOutputFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error while copying file " + myStringWebResource, ex);
            }
        }

        private void parseDataToDatabase()
        {
            C4RForecastRecommendationManager manager = new C4RForecastRecommendationManager(_connStringCovIntegration);
            manager.ProcessFiles(_extractOutputPath, _extractOutputFileName);
        }

        //private void sendCompletedEmail()
        //{
        //    EmailHelper.SendEmail("C4R Forecast Approvals Num Parser completed");
        //}

        #endregion
    }
}
