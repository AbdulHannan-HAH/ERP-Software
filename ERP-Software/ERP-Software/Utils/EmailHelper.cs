using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace ERP_Software.Utils
{
    internal class EmailHelper
    {
        public static void SendCredentialsEmail(string toEmail, string username, string password,string role)
        {
            MailMessage mail = new MailMessage("smartreadselibrary@gmail.com", toEmail);
            mail.Subject = "Your ERP Account Credentials";
            mail.Body = $"Dear User,\n\n" +
                                        $"Your account has been created in the ERP system.\n\n" +
                                        $"👤 Username: {username}\n" +
                                        $"🔐 Password: {password}\n" +
                                        $"🎯 Role: {role}\n\n" +
                                        $"Please login and change your password after first login.\n\n" +
                                        $"Thank you.";
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("smartreadselibrary@gmail.com", "cwqm zxqx fdue xpje"),
                EnableSsl = true
            };

            client.Send(mail);
        }
    }
}
