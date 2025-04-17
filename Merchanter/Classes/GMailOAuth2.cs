using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Merchanter.Classes {
    public static class GMailOAuth2 {
        public static bool Send(string clientId, string clientSecret, string emailSender, string recipientEmail, string subject, string body) {
            try {
                string[] scopes = { "https://mail.google.com/" };
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    },
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("TokenStore", true)).Result;

                string accessToken = credential.Token.AccessToken;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailSender, emailSender));
                message.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient()) {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate using the OAuth2 access token
                    client.Authenticate(new SaslMechanismOAuth2(emailSender, accessToken));

                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}