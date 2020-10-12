using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using Covanta.Entities;
using Covanta.BusinessLogic;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.C4RForecastExtract
{
    public class C4RExtractParser
    {
        #region private variables

        string _covmetadataConnection;
        string _extractTempPath;
        string _extractOutputPath;
        string _extractOutputFileName;
        List<C4RForecastExtractSourceFileInfo> _C4RForecastExtractSourceFileInfo = null;

        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public C4RExtractParser()
        {
            _covmetadataConnection = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
            _extractTempPath = ConfigurationManager.AppSettings["C4RExtractTempPath"];
            _extractOutputPath = ConfigurationManager.AppSettings["C4RExtractOutputPath"];
            _extractOutputFileName = ConfigurationManager.AppSettings["C4RExtractOutputFileName"]; 
        }

        #endregion

        #region public methods

        /// <summary>
        /// Main method of the class.  Only public method.
        /// </summary>
        public void ProcessJob()
        {
            getC4RForecastFileInfoList();
            copyFilesToLocalMachine();
            processForecastFiles();
            deleteFilesFromLocalMachine();
            sendCompletedEmail();
        }
              
        #endregion

        #region private methods

        private void copyFilesToLocalMachine()
        {
            foreach (C4RForecastExtractSourceFileInfo item in _C4RForecastExtractSourceFileInfo)
            {
                string remoteUri = item.Path;
                string fileName = item.FileName, myStringWebResource = null;

                //delete file if it already exists
                FileInfo fileToDelete = new FileInfo(_extractTempPath + fileName);
                if (fileToDelete.Exists) { fileToDelete.Delete(); }

                // Create a new WebClient instance.
                WebClient myWebClient = new WebClient();
                myWebClient.UseDefaultCredentials = true;

                // Concatenate the domain with the Web resource filename.
                myStringWebResource = remoteUri + fileName;

                try
                {
                    myWebClient.DownloadFile(myStringWebResource, _extractTempPath + fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception("Error while copying file " + myStringWebResource, ex);
                }
            }
        }

        private void deleteFilesFromLocalMachine()
        {
            foreach (C4RForecastExtractSourceFileInfo item in _C4RForecastExtractSourceFileInfo)
            {
                FileInfo fileToDelete = new FileInfo(_extractTempPath + item.FileName);
                if (fileToDelete.Exists) { fileToDelete.Delete(); }
            }
        }

        private void getC4RForecastFileInfoList()
        {
            C4RForecastExtractManager manager = new C4RForecastExtractManager(_covmetadataConnection);
            _C4RForecastExtractSourceFileInfo = manager.GetC4RForecastExtractSourceFileInfo();
            manager = null;
        }

        private void processForecastFiles()
        {
            C4RForecastExtractManager manager = new C4RForecastExtractManager(_covmetadataConnection);

            //this list will contain all of the BU's rows combined
            List<C4RForecastExtractRow> fullList = new List<C4RForecastExtractRow>();

            //this list will be one BU at a time
            List<C4RForecastExtractRow> list = new List<C4RForecastExtractRow>();

            //extract data from each file 1 file at a time
            foreach (C4RForecastExtractSourceFileInfo item in _C4RForecastExtractSourceFileInfo)
            {
                list.Clear();
                //populate list with data from the forecast file.                
                manager.ExtractParseData(_extractTempPath, list, item);
                foreach (C4RForecastExtractRow row in list)
                {
                    fullList.Add(row);
                }                
            }

            //Write to Excel
            manager.WriteToEXCEL(_extractOutputPath, _extractOutputFileName, fullList);
            fullList = null;
            manager = null;

        }

        private void sendCompletedEmail()
        {
            EmailHelper.SendEmail("C4R Extract Parser completed");
        }

       

        #endregion
    }
}
