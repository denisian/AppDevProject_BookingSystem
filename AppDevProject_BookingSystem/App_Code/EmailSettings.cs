using System;
using System.Net;
using System.Net.Mail;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Class to set up Email settings
    /// (с) Developed by Denis Klyucherov and Konstantin Khvan
    /// </summary>
    public class EmailSettings
    {
        SmtpClient smtp;
        NetworkCredential NetworkCred;

        // Email and Password of the reastaurant
        private string _email = "systemofbooking@gmail.com";
        private string password = "A6S4n60aQ";
        private string _message;

        public string Email { get { return _email; } }
        public string Message { get { return _message; } }

        public void SmtpClient(string sendTo, MailMessage mailMessage)
        {
            try
            {
                mailMessage.From = new MailAddress(_email);
                mailMessage.To.Add(new MailAddress(sendTo));

                smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCred = new NetworkCredential(_email, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mailMessage);

                _message = "Email has been sent!";
            }
            catch (Exception e)
            {
                _message = e.Message;
            }
        }
    }
}
