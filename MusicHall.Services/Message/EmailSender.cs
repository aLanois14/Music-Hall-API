﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using MusicHall.Core.Domain.Messages;

namespace MusicHall.Services.Message
{
    /// <summary>
    /// Email sender
    /// </summary>
    public partial class EmailSender : IEmailSender
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public EmailSender()
        {

        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="fromAddress">From address</param>
        /// <param name="fromName">From display name</param>
        /// <param name="toAddress">To address</param>
        /// <param name="toName">To display name</param>
        /// <param name="replyTo">ReplyTo address</param>
        /// <param name="replyToName">ReplyTo display name</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <param name="attachedDownloadId">Attachment download ID (another attachedment)</param>
        /// <param name="headers">Headers</param>
        public virtual void SendEmail(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
             string replyTo = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            string attachmentFilePath = null, string attachmentFileName = null,
            int attachedDownloadId = 0, IDictionary<string, string> headers = null)
        {
            var message = new MailMessage
            {
                //from, to, reply to
                From = new MailAddress(fromAddress, fromName)
            };
            message.To.Add(new MailAddress(toAddress, toName));
            if (!string.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            //BCC
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            //CC
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            //content
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            //headers
            if (headers != null)
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }

            //create the file attachment for this e-mail message
            if (!string.IsNullOrEmpty(attachmentFilePath) &&
                File.Exists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(attachmentFilePath);
                if (!string.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }
                message.Attachments.Add(attachment);
            }
            
            //send email
            using (var smtpClient = new SmtpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.Username, emailAccount.Password);
                smtpClient.Send(message);
            }
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }
    }
}