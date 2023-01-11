using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CovantaWebForms.Classes.Helpers
{
    public class EmailHelper
    {
        /// <summary>
        /// Send Email.
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendEmail(string recipients, string subject, string body, string attachment)
        {
            try
            {
                string from = ConfigurationManager.AppSettings["EmailFrom"];
                MailMessage message = new MailMessage(from, recipients, subject, body);
                if (!string.IsNullOrWhiteSpace(attachment))
                {
                    Attachment mailAttachment = new Attachment(attachment);
                    message.Attachments.Add(mailAttachment);
                }
                message.IsBodyHtml = true;

                SendEmail(message);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="message"></param>
        private static void SendEmail(MailMessage message)
        {
            try
            {
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                NetworkCredential credential = new NetworkCredential();
                client.Credentials = credential;
                client.Send(message);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLogging(ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Generate HTML for email body that includes details submitted by User.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="fullName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="company"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="covantaContact"></param>
        /// <param name="category"></param>
        /// <param name="subCategories"></param>
        /// <param name="region"></param>
        /// <param name="supplierDiversity"></param>
        /// <param name="fileUrl"></param>
        /// <param name="isForSubmitter"></param>
        /// <returns></returns>
        public static string GetSupplierFormHtml(
            string applicationId,
            string fullName,
            string phone,
            string email,
            string company,
            string street,
            string city,
            string state,
            string zipcode,
            string country,
            string covantaContact,
            string category,
            List<string> subCategories,
            string region,
            string supplierDiversity,
            //string fileUrl,
            bool isForSubmitter)
        {
            StringBuilder sb = new StringBuilder();

            if (isForSubmitter)
            {
                sb.Append(DisclaimerForSubmitter());
            }

            sb.Append("<table><tbody>");
            sb.Append("<tr><td><b> Application ID: </b></td><td style ='padding:1px 25px;'>" + applicationId + "</td></tr>");
            sb.Append("<tr><td><b> Full Name: </b></td><td style ='padding:1px 25px;'>" + fullName + "</td></tr>");
            sb.Append("<tr><td><b> Phone: </b></td><td style ='padding:1px 25px;'>" + phone + "</td></tr>");
            sb.Append("<tr><td><b> Email: </b></td><td style ='padding:1px 25px;'>");
            sb.Append("<a href='mailto:" + email + "' target='_blank' rel='noopener noreferrer' data-auth='NotApplicable' style='color:#428dcd; text-decoration:none'>" + email + "</a>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td><b> Company: </b></td><td style ='padding:1px 25px;'>" + company + "</td></tr>");
            sb.Append("<tr><td><b> Street: </b></td><td style ='padding:1px 25px;'>" + street + "</td></tr>");
            sb.Append("<tr><td><b> City: </b></td><td style ='padding:1px 25px;'>" + city + "</td></tr>");
            sb.Append("<tr><td><b> State: </b></td><td style ='padding:1px 25px;'>" + state + "</td></tr>");
            sb.Append("<tr><td><b> Zip/Postal Code: </b></td><td style ='padding:1px 25px;'>" + zipcode + "</td></tr>");
            sb.Append("<tr><td><b> Country: </b></td><td style ='padding:1px 25px;'>" + country + "</td></tr>");
            sb.Append("<tr><td><b> Covanta Contact: </b></td><td style ='padding:1px 25px;'>" + covantaContact + "</td></tr>");
            sb.Append("<tr><td><b> Category: </b></td><td style ='padding:1px 25px;'>" + category + "</td></tr>");
            sb.Append("<tr><td valign='top'><b> Sub-Category: </b></td><td style ='padding:1px 25px;'>");
            int count = 1;
            foreach (string subCategory in subCategories)
            {
                sb.Append(subCategory);
                if (subCategories.Count != count)
                {
                    sb.Append("<br/>");
                }
            }
            sb.Append("</td></tr>");
            sb.Append("<tr><td><b> Region: </b></td><td style ='padding:1px 25px;'>" + region + "</td></tr>");
            sb.Append("<tr><td><b> Supplier Diversity: </b></td><td style ='padding:1px 25px;'>" + supplierDiversity + "</td></tr>");

            //if (!isForSubmitter)
            //{
            //    sb.Append("<tr><td><b> Signed NDA </b></td><td style ='padding:1px 25px;'>" + string.Format("<a href='{0}'>View Attachment</a>", fileUrl) + "</td></tr>");
            //}

            sb.Append("</tbody></table>");
            
            return sb.ToString();
        }

        /// <summary>
        /// Disclaimer text only for Submitter Mail.
        /// </summary>
        /// <returns></returns>
        public static string DisclaimerForSubmitter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<p>-------------------DO NOT REPLY TO THIS EMAIL ----------------------------</br>");
            sb.Append("Thank you contacting us.Your application will be reviewed, and we’ll get back to you shortly.</p>");
            sb.Append("<p>Please find below a summary of the information you have submitted.</p> ");
            return sb.ToString();
        }
    }
}