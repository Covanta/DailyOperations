using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.ServiceNowExtractVIP
{
    class Program
    {
        static string _url = ConfigurationManager.AppSettings["url"];
        static string _savePath = ConfigurationManager.AppSettings["savePath"];
        static string _id = ConfigurationManager.AppSettings["ID"];
        static string _password = ConfigurationManager.AppSettings["Password"];
                
        static string _emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
        static string _emailSubject = ConfigurationManager.AppSettings["EmailSubject"];
        static string _emailTo = ConfigurationManager.AppSettings["EmailTo"];

        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }

           

            try
            {
                //Download File from Service Now URL
                int read = DownloadFile(_url, _savePath);

                //Write Fields to Database
                Processor process = new Processor();
                process.processData();
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail(_emailFrom, _emailTo, _emailSubject, "Error while processing ServiceNowExtractVIP job. " + ex.Message);
                throw ex;
            }            
        }

        public static int DownloadFile(String url, String localFilename)
        {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            int bytesProcessed = 0;
            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;
            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try
            {
                // Create a request for the specified remote file name
                WebRequest request = WebRequest.Create(url);
                // Create the credentials required for Basic Authentication
                System.Net.ICredentials cred = new System.Net.NetworkCredential(_id, _password);
                // Add the credentials to the request
                request.Credentials = cred;
                if (request != null)
                {
                    // Send the request to the server and retrieve the
                    // WebResponse object 
                    response = request.GetResponse();
                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();
                        // Create the local file
                        localStream = File.Create(localFilename);
                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        // Simple do/while loop to read from stream until
                        // no bytes are returned
                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);
                            // Increment total bytes processed
                            bytesProcessed += bytesRead;
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                EmailHelper.SendEmail(_emailFrom, _emailTo, _emailSubject, "Error while processing ServiceNowExtractVIP job. " + e.Message);
            }
            finally
            {
                // Close the response and streams objects here 
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }
            // Return total bytes processed to caller.
            return bytesProcessed;
        }

        private static bool isAppAlreadyRunning()
        {
            Process[] appProc;
            string strModName;
            string strProcName;
            strModName = Process.GetCurrentProcess().MainModule.ModuleName;
            strProcName = System.IO.Path.GetFileNameWithoutExtension(strModName);
            appProc = Process.GetProcessesByName(strProcName);
            if (appProc.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
