using MimeKit;
using System;
using System.Web;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MailKit;
using MailKit.Security;
using System.Diagnostics;
using System.Text;
using System.Web.Mvc;

namespace ShopApp.Utility
{
    public class EmailManager
    {
        public enum EmailType
        {
            Registration,
            ChangePassword
        }

        // Return true if email was successfuly sent to receiver
        public static async Task<bool> SendEmailAsync(EmailType emailType, string receiverFirstName, string receiverLastName, string receiverEmail)
        {
            bool result = false;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Zakupowo Team", "zakupowo2020@gmail.com"));
            message.To.Add(new MailboxAddress(receiverFirstName + " " + receiverLastName, receiverEmail));
            message.Subject = string.Empty;
            var builder = new BodyBuilder();
            string messageBody = string.Empty;

            switch (emailType)
            {
                case EmailType.Registration:
                    builder.HtmlBody = RegistrationText(receiverEmail);
                    message.Subject = @"Successful registration!";
                    break;

                case EmailType.ChangePassword:
                    builder.HtmlBody = ChangePasswordText(receiverEmail);
                    message.Subject = @"Your password has been changed.";
                    break;
            }
            builder.HtmlBody += EndOfEmail();

            message.Body = builder.ToMessageBody();


            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

                    await client.AuthenticateAsync("zakupowo2020", "Zakupowo2020$$$");

                    await client.SendAsync(message);
                    client.Disconnect(true);
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return result;
        }

        public static bool SendEmail(EmailType emailType, string receiverFirstName, string receiverLastName, string receiverEmail)
        {
            bool result = false;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Zakupowo Team", "zakupowo2020@gmail.com"));
            message.To.Add(new MailboxAddress(receiverFirstName + " " + receiverLastName, receiverEmail));

            message.Subject = string.Empty;
            var builder = new BodyBuilder();
            string messageBody = string.Empty;

            switch (emailType)
            {
                case EmailType.Registration:
                    builder.HtmlBody = RegistrationText(receiverEmail);
                    message.Subject = @"Successful registration!";
                    break;

                case EmailType.ChangePassword:
                    builder.HtmlBody = ChangePasswordText(receiverEmail);
                    message.Subject = @"Your password has been changed.";
                    break;
            }
            builder.HtmlBody += EndOfEmail();

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.Auto);

                    client.Authenticate("zakupowo2020", "Zakupowo2020$$$");

                    client.Send(message);
                    client.Disconnect(true);
                    result = true;
                    Debug.WriteLine("EMAIL SENT!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return result;
        }

        // A message sent to new registered accounts.
        private static string RegistrationText(string receiverEmail)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<pre>Thank you for registration!");
            message.AppendLine();
            message.AppendLine("Now you can:");
            message.AppendLine("    -Browse offers");
            message.AppendLine("    -Change your data");
            message.AppendLine();
            message.AppendLine("If you want more functionality:");
            message.AppendLine("    -Buy offers");
            message.AppendLine("    -Make offers");
            message.AppendLine();
            message.AppendLine("<a href=\"http://localhost:44000/User/ConfirmRegistration?email=" + receiverEmail + "\">Click here</a> to fully register your account!<pre>");

            return message.ToString();
        }

        // A message sent when a password was changed.
        private static string ChangePasswordText(string receiverEmail)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<pre>Have you just tried to change your password?");
            message.AppendLine("<a href=\"http://localhost:44000/UserPanel/ConfirmPasswordChange?=" + receiverEmail + "\">Click here</a> to confirm the change.");
            message.AppendLine();
            message.AppendLine();
            message.AppendLine("If you don't recognize this activity, please <a href=\"http://localhost:44000/Home/Contact\">contact us</a>.<pre>");

            return message.ToString();
        }

        // The end of every message (can be made in gmail options too).
        private static string EndOfEmail()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<pre>");
            message.AppendLine("-----------------------------------------------");
            message.AppendLine("");
            message.AppendLine("Best regards,");
            message.AppendLine("Zakupowo Team<pre>");

            return message.ToString();
        }
    }
}