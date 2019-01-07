using iTextSharp.text;
using iTextSharp.text.pdf;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Services
{
    public class EmailService
    {
        private SmtpClient _client;

        public EmailService()
        {
            var username = ConfigurationManager.AppSettings["MailUsername"];
            var password = ConfigurationManager.AppSettings["MailPassword"];

            _client = new SmtpClient();
            _client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
            _client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["MailUseDefaultCredentials"]);
            _client.Credentials = new NetworkCredential(username, password);
            _client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["MailEnableSsl"]);
            _client.Host = ConfigurationManager.AppSettings["MailHost"];
            _client.Timeout = 10000;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public void GeneratePDF(string path, 
            string name,
            DateTimeRange range,
            int numberOfVisitors
            )
        {
            using (System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                document.Open();

                Paragraph paragraph1 = new Paragraph();
                paragraph1.SpacingBefore = 10;
                paragraph1.SpacingAfter = 10;

                paragraph1.Font.SetStyle("bold");
                paragraph1.Add("Toegangsbewijs voor De Eekhoorn Dumpwinkel");
                document.Add(paragraph1);

                Paragraph paragraph2 = new Paragraph();
                paragraph2.SpacingBefore = 10;
                paragraph2.SpacingAfter = 10;
                paragraph2.Add("Beste " + name + ",");
                document.Add(paragraph2);

                Paragraph paragraph3 = new Paragraph();
                paragraph3.Add("Bedankt voor je registratie!");
                document.Add(paragraph3);
                Phrase phrase1 = new Phrase("Hierbij jouw toegangsbewijs voor de winkel:");
                document.Add(phrase1);

                Paragraph paragraph4 = new Paragraph();
                paragraph4.SpacingBefore = 10;
                paragraph4.SpacingAfter = 0;
                paragraph4.Font.SetStyle("bold");
                paragraph4.Add("Tijd: " + range.ToString());
                document.Add(paragraph4);
                Phrase phrase2 = new Phrase("Aantal bezoekers: " + numberOfVisitors);
                phrase2.Font.SetStyle("bold");
                document.Add(phrase2);

                Paragraph paragraph5 = new Paragraph();
                paragraph5.SpacingBefore = 10;
                paragraph5.SpacingAfter = 0;
                paragraph5.Add("Met vriendelijke groet,");
                document.Add(paragraph5);
                Phrase phrase3 = new Phrase("De Eekhoorn Dumpwinkel");
                document.Add(phrase3);

                document.Close();
               
                fileStream.Close();
                
            }
                

        }

        public void SendMail(string mailFrom, string mailTo, string subject, string body, string attachment = null)
        {
            var addresses = mailTo.Split(';');

            MailMessage mm = new MailMessage();
            mm.Subject = subject;
            mm.Body = body;
            mm.From = new MailAddress(mailFrom);
            mm.IsBodyHtml = true;
            foreach (var address in addresses)
            {
                mm.To.Add(new MailAddress(address));
            }
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                if (attachment != null)
                {
                    Attachment data = new Attachment(attachment);
                    mm.Attachments.Add(data);
                }
                _client.Send(mm);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
