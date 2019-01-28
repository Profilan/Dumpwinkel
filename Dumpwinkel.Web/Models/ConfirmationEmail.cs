using iTextSharp.text;
using iTextSharp.text.pdf;
using Postal;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class ConfirmationEmail : Email
    {
        public string To { get; set; }
        public string BarcodeUrl { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTill { get; set; }
        public int NumberOfVisitors { get; set; }
        public string RegistrationId { get; set; }

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
    }
}