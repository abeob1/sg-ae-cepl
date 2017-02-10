using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using AE_CleaningExpress_Common;

namespace Utils
{
    public class Emails
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>

        public static string SendEmail(string from, string[] tos, string subject, string body, Attachment[] attachments, string embeddedImage)
        {
            string sErrDesc = string.Empty;
            string sFuncName = string.Empty;

            const Int16 DEBUG_ON = 1;
            clsLog oLog = new clsLog();
            Int16 p_iDebugMode = DEBUG_ON;

            try
            {
                sFuncName = "SendEmail";
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                    smtp.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString()); ;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SMTPUser"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"].ToString());
                    smtp.Timeout = 900000;
                }
                MailMessage mail = new MailMessage();

                if (embeddedImage != null)
                {
                    AlternateView View;
                    LinkedResource resource;

                    View = AlternateView.CreateAlternateViewFromString(body.ToString(), null, "text/html");

                    resource = new LinkedResource(embeddedImage);

                    resource.ContentId = "Logo";

                    View.LinkedResources.Add(resource);

                    mail.AlternateViews.Add(View);
                }

                mail.From = new MailAddress(from);
                foreach (string to in tos)
                {
                    mail.To.Add(to);
                }

                string ccAddress = System.Configuration.ConfigurationManager.AppSettings["CCEmail"].ToString();
                MailAddress cc = new MailAddress(ccAddress);
                mail.CC.Add(cc);

                mail.Subject = subject;
                //mail.Body = body;
                if (attachments != null)
                {
                    foreach (Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
                return "Success";
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR", sFuncName);
                return ex.Message;
            }
            
        }
    }
}