using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.Utilities.Helpers;
using Covanta.BusinessLogic;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Covanta.UI.DailyOpsAutoEmail
{
    class DailyOpsEmailer
    {
        private static string covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        private static string covIntegrationConnString = ConfigurationManager.ConnectionStrings["covIntegrationConnString"].ConnectionString;
        private static DailyOpsBusiUnitManager busUnitManager = new DailyOpsBusiUnitManager(covmetadataConnString);
        private static DailyOpsManager dailyDataManager = new DailyOpsManager(covmetadataConnString);

        private const string FACILITY_MANAGER = "FM", CHIEF_ENGINEER = "CE";

        public static void sendFacilityManagerEmails(string offset)
        {
            sendAlertEmails(FACILITY_MANAGER, offset);
        }

        public static void sendFacilityManagerEmails()
        {
            sendFacilityManagerEmails(null);
        }

        public static void sendChiefEngineerEmails(string offset)
        {
            sendAlertEmails(CHIEF_ENGINEER, offset);
        }

        public static void sendChiefEngineerEmails()
        {
            sendChiefEngineerEmails(null);
        }

        public static void sendMorningExceptionsReportEmail()
        {
            DailyOpsManager manager = new DailyOpsManager(covIntegrationConnString);

            //To           
            //string to = "thoefler@covantaenergy.com, jlehr@covantaenergy.com";

            string to = ConfigurationManager.AppSettings["DailyExceptionReportEmailTo"];
            
            string from = ConfigurationManager.AppSettings["DailyExceptionReportEmailFrom"];
            string subject = ConfigurationManager.AppSettings["DailyExceptionReportEmailSubject"];

            string body = callGetExceptionsReport();

            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            //List<string> emailList = manager.GetDailyOpsExceptionEmailRecipientsList();

            //emailList = emailList.OrderBy(q => q).ToList();

            //// create 2 lists so we can send the email as 2 lists of 100 rather than 1 list of 200.
            //// email is limited to 200 recipients, so we break it up into 2.
            //List<string> emailList1 = new List<string>();
            //List<string> emailList2 = new List<string>();

            //int count = emailList.Count();
            //int halfcount = count / 2;

            ////separate to 2 lists here
            //int listCounter = 0;
            //foreach (string email in emailList)                
            //{
            //    listCounter++;
            //    if (listCounter <= halfcount)
            //    {
            //        emailList1.Add(email);
            //    }
            //    else
            //    {
            //        emailList2.Add(email);
            //    }            
            //}

            //prepare message 1
            //message.To.Clear();
            //message.CC.Clear();
            //message.Bcc.Clear();

            //message.To.Add(new MailAddress(ConfigurationManager.AppSettings["DailyExceptionReportEmailTo"]));
            //send to first half of recipients
            //foreach (string email in emailList1)
            //{
            //    message.Bcc.Add(new MailAddress(email));
            //}     

            EmailHelper.SendEmail(message);

            //prepare message 2
            //message.To.Clear();
            //message.CC.Clear();
            //message.Bcc.Clear();
            
            //message.To.Add(new MailAddress("jlehr@covanta.com"));
            ////send to second half of recipients
            //foreach (string email in emailList2)
            //{
            //    message.Bcc.Add(new MailAddress(email));
            //}

            //message.Bcc.Add(new MailAddress("blitwin@covanta.com"));
            //EmailHelper.SendEmail(message);
        }

        private static string callGetExceptionsReport()
        {
            string body = string.Empty;
            bool isCallSuccess = false;
            int maxTries = 5;

            // attempt to successfully call the getExceptionsReportHtml() method several times before giving up and throwng an exception.
            // The request has been tiiming out, so let's call it a few times and hope that by the last time the data has been cached.
            for (int i = 1; i <= maxTries; i++)
            {
                if (isCallSuccess == false)
                {
                    try
                    {
                        body = getExceptionsReportHtml();
                        isCallSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isCallSuccess = false;
                        // if after several attemps we have no success, then throw the exception.
                        if (i == maxTries)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return body;
        }

        private static void sendAlertEmails(string recipientType, string offset)
        {
            Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
            List<DailyOpsData> data = dailyDataManager.GetDailyOpsDataByDate(DateTime.Now.AddDays(-1).Date, ref status);
            bool disregardOffset = offset == null;

            foreach (DailyOpsData entry in data)
                if (entry.UserRowCreated == null)
                {
                    string facilityOffset = busUnitManager.GetDailyOpsBusinessUnitEasternTimeOffset(entry.FacilityID);
                    if (disregardOffset || facilityOffset.Equals(offset))
                    {
                        List<string> recipients = busUnitManager.GetDailyOpsBusinessUnitContactEmailList(entry.FacilityID, recipientType);
                        if (recipients.Count == 0)
                            return;

                        sendEmail(recipients, entry);
                    }
                }
        }

        private static void sendEmail(List<string> recipients, DailyOpsData data)
        {
            string facilityName = data.FaciltyDescription;
            string facilityId = data.FacilityID;
            string reportingDate = data.ReportingDate.ToShortDateString();

            string to = "";
            foreach (string r in recipients)
                to += r + ",";

            string from = "do-not-reply@opsreport.cov.corp";
            string subject = string.Format("Request for {0}'s Daily Operations Report for {1}", facilityName, reportingDate);
            string body = string.Format(@"The Daily Operations Report for {0} for {1} has not been filled out yet.
				Click <a href='http://opsreport.cov.corp/dailyops.aspx?facility={2}&date={3}'>here</a> to enter the report.",
                facilityName, reportingDate, facilityId, reportingDate.Replace('/', '-'));

            MailMessage message = new MailMessage(from, to, subject, body);
            //message.Bcc.Add(new MailAddress("blitwin@covantaenergy.com"));
            message.IsBodyHtml = true;

            EmailHelper.SendEmail(message);
        }

        /// <summary>
        /// Ultimately this method will save the Exceptions Report page as a PDF (used to send via email).
        /// </summary>
        private static string getExceptionsReportHtml()
        {
            // Source: http://www.aspose.com/docs/display/pdfnet/How+to+convert+HTML+to+PDF+using+InLineHTML+approach

            // The address of the web URL which you need to convert into PDF format
            string WebUrl = ConfigurationManager.AppSettings["ReportUrl"];//"http://opsreport.cov.corp/ExceptionsReportEmail.aspx";

            // create a Web Request object to connect to remote URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebUrl);

            // set the Web Request timeout
            //request.Timeout = 10000;     // 10 secs

            request.Credentials = CredentialCache.DefaultCredentials;

            // Retrieve request info headers
            HttpWebResponse localWebResponse = (HttpWebResponse)request.GetResponse();

            Stream stream = localWebResponse.GetResponseStream();

            // Windows default Code Page  (Include System.Text namespace in project)
            Encoding encoding = Encoding.GetEncoding(1252);

            // Read the contents of into StreamReader object
            StreamReader localResponseStream = new StreamReader(localWebResponse.GetResponseStream(), encoding);

            string html = localResponseStream.ReadToEnd();

            localWebResponse.Close();
            localResponseStream.Close();

            return html;
        }
    }
}