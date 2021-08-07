using PWC.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Configuration;

namespace PWC.Common.Helpers
{
    public class EmailHelper
    {
        #region Constants

        private const string SMTP_EMAIL_PASSWORD = "SMTP_EMAIL_PASSWORD";
        private const string SMTP_EMAIL_ADDRESS = "SMTP_EMAIL_ADDRESS";
        private const string SMTP_SERVER = "SMTP_SERVER";
        private const string SMTP_PORT = "SMTP_PORT";

        #endregion

        #region Variables

        private static string smtpEmailPassword = WebConfigurationManager.AppSettings[SMTP_EMAIL_PASSWORD].ToString();
        private static string smtpEmailAddress = WebConfigurationManager.AppSettings[SMTP_EMAIL_ADDRESS].ToString();
        private static string smtpServer = WebConfigurationManager.AppSettings[SMTP_SERVER].ToString();
        private static string smtpPort = WebConfigurationManager.AppSettings[SMTP_PORT].ToString();
        #endregion

        #region Methods

        public static bool SendMail(string recipientEmails, string msgSubject, string msgBody, string bccEmails = "")
        {
            bool isEmailSent = false;
            try
            {
                MailMessage oMail = new MailMessage();
                SmtpClient oSmtpClient = new SmtpClient(smtpServer);

                oMail.From = new MailAddress(smtpEmailAddress);

                if (recipientEmails.Contains(";"))
                {
                    List<string> emailRecipients = recipientEmails.Split(';').ToList();
                    foreach (string emailRecipient in emailRecipients)
                    {
                        oMail.To.Add(new MailAddress(emailRecipient));
                    }
                }
                else
                {
                    oMail.To.Add(recipientEmails);
                }

                if (!string.IsNullOrEmpty(bccEmails))
                {
                    if (bccEmails.Contains(";"))
                    {
                        List<string> emailBccs = bccEmails.Split(';').ToList();
                        foreach (string emailBcc in emailBccs)
                        {
                            oMail.Bcc.Add(new MailAddress(emailBcc));
                        }
                    }
                    else
                    {
                        oMail.Bcc.Add(bccEmails);
                    }
                }

                oMail.Subject = msgSubject;
                oMail.Body = msgBody;
                oMail.IsBodyHtml = true;

                oSmtpClient.Port = int.Parse(smtpPort);
                oSmtpClient.Credentials = new System.Net.NetworkCredential(smtpEmailAddress, smtpEmailPassword);
                oSmtpClient.EnableSsl = false;
                oSmtpClient.Send(oMail);

                isEmailSent = true;
            }
            catch (Exception ex)
            {
                isEmailSent = false;
            }

            return isEmailSent;
        }

        public static bool SendMail(string recipientEmails, string emailFrom, string msgSubject, string msgBody, int smtpClientPort, ICredentialsByHost mailCredentials, SmtpClient oSmtpClient, bool isHtmlTemp = false)
        {
            bool isEmailSent = false;
            try
            {
                MailMessage oMail = new MailMessage();
                oMail.From = new MailAddress(emailFrom);

                if (recipientEmails.Contains(";"))
                {
                    List<string> emailRecipients = recipientEmails.Split(';').ToList();
                    foreach (string emailRecipient in emailRecipients)
                    {
                        oMail.To.Add(new MailAddress(emailRecipient));
                    }
                }
                else
                {
                    oMail.To.Add(recipientEmails);
                }

                oMail.Subject = msgSubject;
                oMail.Body = msgBody;
                oMail.IsBodyHtml = isHtmlTemp;

                oSmtpClient.Port = smtpClientPort;
                //oSmtpClient.UseDefaultCredentials = false;
                //oSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                oSmtpClient.Credentials = mailCredentials;
                //oSmtpClient.EnableSsl = true;

                oSmtpClient.Send(oMail);
                isEmailSent = true;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return isEmailSent;

        }

        public static bool SendMailWithLogoHeader(string recipientEmails, string msgSubject, string msgBody, string HeaderCid, string LogoCid)
        {
            bool isEmailSent = false;
            try
            {
                MailMessage oMail = new MailMessage();
                SmtpClient oSmtpClient = new SmtpClient(smtpServer);

                var HeaderStream = new MemoryStream(new WebClient().DownloadData("http://almanhal.com/images/EmailHeader.gif"));
                var LogoStream = new MemoryStream(new WebClient().DownloadData("http://almanhal.com/images/EmailLogoIcon.gif"));


                Attachment oHeaderAttachment = new Attachment(HeaderStream, "");
                Attachment oLogoAttachment = new Attachment(LogoStream, "");

                oMail.Attachments.Add(oHeaderAttachment);
                oMail.Attachments.Add(oLogoAttachment);

                oHeaderAttachment.ContentDisposition.Inline = true;
                oHeaderAttachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                oLogoAttachment.ContentDisposition.Inline = true;
                oLogoAttachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                oLogoAttachment.ContentId = LogoCid;
                oHeaderAttachment.ContentId = HeaderCid;

                oMail.From = new MailAddress(smtpEmailAddress);
                if (recipientEmails.Contains(";"))
                {
                    List<string> emailRecipients = recipientEmails.Split(';').ToList();
                    foreach (string emailRecipient in emailRecipients)
                    {
                        oMail.To.Add(new MailAddress(emailRecipient));
                    }
                }
                else
                {
                    oMail.To.Add(recipientEmails);
                }

                oMail.Subject = msgSubject;
                oMail.Body = msgBody;
                oMail.IsBodyHtml = true;

                oSmtpClient.Port = int.Parse(smtpPort);
                oSmtpClient.Credentials = new NetworkCredential(smtpEmailAddress, smtpEmailPassword);
                oSmtpClient.EnableSsl = false;
                oSmtpClient.Send(oMail);

                isEmailSent = true;
            }
            catch (Exception ex)
            {
                isEmailSent = false;
            }

            return isEmailSent;
        }

        #endregion
    }
}
