using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Xml.Serialization;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace ITTT_Final
{
    [XmlInclude(typeof(ITTTActionSendMail)), XmlInclude(typeof(ITTTActionShowWindow))]
    [Serializable]
    public abstract class ITTTAction
    {
        [Key]
        public int Id {get; set; }
        public string Address { get; set; }
        
        public abstract void ExecuteAction(string fileName, string msg);
        
        public override string ToString()
        {
            return string.Format("adres e-mail: {0}", Address);
        }
    }
    [Serializable]
    public class ITTTActionSendMail : ITTTAction
    {
        public override void ExecuteAction(string fileName, string msg)
        {
            MailMessage mail = new MailMessage("net.art.pwr@gmail.com", Address);
            mail.Subject = "ITTTActionSendMail";
            mail.Body = msg;
            mail.Attachments.Add(new Attachment(fileName));
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Timeout = 10000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("net.art.pwr@gmail.com", "netartpwr");
            smtp.Send(mail);
            mail.Dispose();
            smtp.Dispose();
        }

        public override string ToString()
        {
            return string.Format("adres e-mail: {0}", Address);
        }
    }
    [Serializable]
    public class ITTTActionShowWindow: ITTTAction
    {
        public override void ExecuteAction(string fileName, string msg)
        {
            Form2 form = new Form2();

            form.PrepareForm(fileName, msg);
            form.ShowDialog();
        }
        public override string ToString()
        {
            return string.Format("wyświetl w oknie");
        }
    }
}
