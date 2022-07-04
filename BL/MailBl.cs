using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MailBl: IMailBl
    {
        public void SendEmail(string toAddress, string subject, string body)
        {

            MailMessage message = new MailMessage();
            message.From = new MailAddress("324857648@mby.co.il");
            message.To.Add(new MailAddress(toAddress));

            string mailbody = body;
            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("324857648@mby.co.il", "Student@264");
            client.Host = "smtp.office365.com";
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}

//string mailbody = "We are happy to tell you that your group is going to begin\n" + " ";
//message.Subject = "massage from pricetracker ";
