using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.C4RForecastApprovals
{
    public class C4RForecastApprovalParser
    {
        #region private variables
      
        string _extractInputPath;
        string _extractInputFileName;
        string _extractOutputPath;
        string _extractOutputFileName;

        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public C4RForecastApprovalParser()
        {
            _extractInputFileName = ConfigurationManager.AppSettings["C4RForecastApprovalsFromSharePointInputFileName"];
            _extractInputPath = ConfigurationManager.AppSettings["C4RForecastApprovalsFromSharePointInputPath"];

            _extractOutputFileName = ConfigurationManager.AppSettings["C4RForecastApprovalsFromSharePointOutputFileName"];
            _extractOutputPath = ConfigurationManager.AppSettings["C4RForecastApprovalsFromSharePointOutputPath"];
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

        //private void sendCompletedEmail()
        //{
        //    EmailHelper.SendEmail("C4R Forecast Approvals Num Parser completed");
        //}

        #endregion
    }
}
