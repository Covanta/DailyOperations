using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using Covanta.Utilities.Helpers;
using Microsoft.SharePoint.Client;
using System.Security;
using File = Microsoft.SharePoint.Client.File;

namespace Covanta.UI.C4RWasteTypeExtractFromSharePoint
{
    public class C4RWasteTypeMasterApprovalNumParser
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
        public C4RWasteTypeMasterApprovalNumParser()
        {
            _extractInputFileName = ConfigurationManager.AppSettings["C4RWasteTypeExtractFromSharePointInputFileName"];
            _extractInputPath = ConfigurationManager.AppSettings["C4RWasteTypeExtractFromSharePointInputPath"];

            _extractOutputFileName = ConfigurationManager.AppSettings["C4RWasteTypeExtractFromSharePointOutputFileName"];
            _extractOutputPath = ConfigurationManager.AppSettings["C4RWasteTypeExtractFromSharePointOutputPath"];
        }

        #endregion

        #region public methods

        /// <summary>
        /// Main method of the class.  Only public method.
        /// </summary>
        public void ProcessJob()
        {
            bool downloadFileFromSharepointOnline = bool.Parse(ConfigurationManager.AppSettings["EnableFileDownloadFromSharepointOnline"]);
            if (downloadFileFromSharepointOnline)
            {
                DownloadFileToOutputPathFromSharepointOnline();
            }
            else
            {
                copyFileToOutputPath();
            }
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

        private void DownloadFileToOutputPathFromSharepointOnline()
        {
            using (ClientContext ctx = new ClientContext(ConfigurationManager.AppSettings["SharepointWebUrl"]))
            {
                string password = ConfigurationManager.AppSettings["SharepontOnline_Password"];
                string username = ConfigurationManager.AppSettings["SharepontOnline_UserName"];

                var secret = new SecureString();
                foreach (char c in password)
                {
                    secret.AppendChar(c);
                }
                ctx.Credentials = new SharePointOnlineCredentials(username, secret);
                ctx.Load(ctx.Web);
                ctx.ExecuteQuery();

                List list = ctx.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["SharepontLibraryName"]);

                FileCollection files = list.RootFolder.Folders.GetByUrl(ConfigurationManager.AppSettings["SharepontFolderToDownloadFileFrom"]).Files;

                ctx.Load(files);
                ctx.ExecuteQuery();

                foreach (Microsoft.SharePoint.Client.File file in files.Where(x=>x.Name.Contains(ConfigurationManager.AppSettings["C4RWasteTypeExtractFromSharePointOutputFileName"])))
                {
                    FileInformation fileinfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(ctx, file.ServerRelativeUrl);

                    ctx.ExecuteQuery();

                    using (FileStream filestream = new FileStream(ConfigurationManager.AppSettings["C4RWasteTypeExtractFromSharePointOutputPath"] + file.Name, FileMode.Create))
                    {
                        fileinfo.Stream.CopyTo(filestream);
                    }

                }
            };
        }

        private void sendCompletedEmail()
        {
            EmailHelper.SendEmail("C4R Waste Type Master Approval Num Parser completed");
        }

        #endregion
    }
}
