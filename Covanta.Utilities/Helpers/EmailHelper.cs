using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Covanta.Utilities.Helpers
{
    public class EmailHelper
    {
        #region constructors

        public EmailHelper() { }

        #endregion

        #region private variables

        #endregion

        #region public methods

        public static void SendEmail(string messageBody)
        {           
            string EmailTo = ConfigurationManager.AppSettings["EmailTo"];
            string EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string EmailSubject = ConfigurationManager.AppSettings["EmailSubject"];
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(DateTime.Now.ToString());
          
            sb.AppendLine(messageBody);

            string EmailBody = sb.ToString();

            MailMessage message = new MailMessage(EmailFrom, EmailTo, EmailSubject, EmailBody);
            message.IsBodyHtml = true;           
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            NetworkCredential credential = new NetworkCredential();
            client.Credentials = credential;
         //   client.Send(message);
        }

        public static void SendEmail(string EmailFrom, string EmailTo, string EmailSubject, string EmailBody)
        {         
            MailMessage message = new MailMessage(EmailFrom, EmailTo, EmailSubject, EmailBody);
            message.IsBodyHtml = true;
			SendEmail(message);
        } 
        public static void SendEmailNotHTML(string EmailFrom, string EmailTo, string EmailSubject, string EmailBody)
        {
            MailMessage message = new MailMessage(EmailFrom, EmailTo, EmailSubject, EmailBody);
            message.IsBodyHtml = false;
            SendEmail(message);
        }

		public static void SendEmail(MailMessage message)
		{
			SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
			NetworkCredential credential = new NetworkCredential();
			client.Credentials = credential;          
			client.Send(message);
		}

        #endregion

        #region private methods

        #endregion
    }
}
