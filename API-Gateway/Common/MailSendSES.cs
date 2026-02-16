using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using Amazon;
using Nancy;
using Amazon.Runtime.Internal;
using MimeKit;

namespace API_Gateway.Common
{
    public class MailSendSES
    {
        private readonly IConfiguration _configuration;

        public MailSendSES(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> sendMail(string subject, string htmlBody, List<string> ReceiverEmail, List<string>? ReceiverCcEmaill = null, List<string>? ReceiverBccEmail = null, List<string>? Attachment = null)
        {
            string response = "";
            try
            {
                string access_key = _configuration.GetSection("AWS").GetSection("access_key").Value;
                string secret_key = _configuration.GetSection("AWS").GetSection("secret_key").Value;

                string SenderName = _configuration.GetSection("AdminMail").GetSection("SenderName").Value;
                string SenderMail = _configuration.GetSection("AdminMail").GetSection("SenderMail").Value;

                using (var client = new AmazonSimpleEmailServiceClient(access_key, secret_key, RegionEndpoint.APSouth1))
                using (var messageStream = new MemoryStream())
                {
                    var message = new MimeMessage();
                    var builder = new BodyBuilder();

                    message.Subject = subject;
                    message.From.Add(new MailboxAddress(SenderName, SenderMail));


                    foreach (var item in ReceiverEmail)
                    {
                        message.To.Add(new MailboxAddress("", item));
                    }


                    if (ReceiverCcEmaill != null)
                    {
                        foreach (var item in ReceiverCcEmaill)
                        {
                            message.Cc.Add(new MailboxAddress("", item));
                        }
                    }

                    if (ReceiverBccEmail != null)
                    {
                        foreach (var item in ReceiverBccEmail)
                        {
                            message.Bcc.Add(new MailboxAddress("", item));
                        }
                    }

                    if (!string.IsNullOrEmpty(htmlBody))
                    {
                        builder.HtmlBody = htmlBody;
                    }

                    if (Attachment != null)
                    {
                        foreach (var item in Attachment)
                        {
                            using (FileStream stream = File.Open(item, FileMode.Open)) builder.Attachments.Add(Path.GetFileName(item), stream);   
                        }
                    }

                    message.Body = builder.ToMessageBody();
                    message.WriteTo(messageStream);

                    var request = new SendRawEmailRequest()
                    {
                        RawMessage = new RawMessage() { Data = messageStream }
                    };

                    try
                    {
                        await client.SendRawEmailAsync(request);
                        response = "success";
                    }
                    catch (Exception ex)
                    {
                        response = "fail" + " Error message: " + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {

                response = "fail" + " Error message: " + ex.Message;
            }

            return response;
        }
    }
}
