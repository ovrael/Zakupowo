using MimeKit;
using System;
using System.Web;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MailKit;
using MailKit.Security;
using System.Diagnostics;

namespace ShopApp.Utility
{
    public class EmailManager
    {
        private static readonly string enter = Environment.NewLine;

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
            string messageBody = string.Empty;

            switch (emailType)
            {
                case EmailType.Registration:
                    messageBody = RegistrationText();
                    message.Subject = @"Successful registration!";
                    break;

                case EmailType.ChangePassword:
                    messageBody = ChangePasswordText();
                    message.Subject = @"Your password has been changed.";
                    break;
            }
            messageBody += EndOfEmail();

            message.Body = new TextPart("plain")
            {
                Text = messageBody
            };

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
                    Debug.WriteLine("EMAIL SENT!");
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
            string messageBody = string.Empty;

            switch (emailType)
            {
                case EmailType.Registration:
                    messageBody = RegistrationText();
                    message.Subject = @"Successful registration!";
                    break;

                case EmailType.ChangePassword:
                    messageBody = ChangePasswordText();
                    message.Subject = @"Your password has been changed.";
                    break;
            }
            messageBody += EndOfEmail();

            message.Body = new TextPart("plain")
            {
                Text = messageBody
            };

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
        private static string RegistrationText()
        {
            string message =
            "Thank you for registration!"
            + enter
            + enter
            + "Now you can:"
            + enter
            + "\t-Browse offers"
            + enter
            + "\t-Add your offer"
            + enter
            + "\t-Change your data";

            return message;
        }

        // A message sent when a password was changed.
        private static string ChangePasswordText()
        {
            string message =
            "Have you just changed your password?"
            + enter
            + enter
            + "If you don't recognize this activity, please contact us.";

            return message;
        }

        // The end of every message (can be made in gmail options too).
        private static string EndOfEmail()
        {
            string message =
            enter
            + enter
            + "-------------------------------------------"
            + enter
            + "Best regards,"
            + enter
            + "Zakupowo Team";

            return message;
        }
    }
}