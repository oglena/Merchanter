using Google.Apis.Auth.OAuth2;
using Google.Apis.Json;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

namespace Merchanter.Classes {
    public static class GMailOAuth2 {
        public static bool Send(string user, string senderName, string clientId, string clientSecret, string emailSender, string recipientEmail, string subject, string body) {
            try {
                string accessToken = string.Empty;
                // Check if the access token is already stored
                string tokenPath = Path.Combine(Environment.CurrentDirectory, "GmailTokenStore", "Google.Apis.Auth.OAuth2.Responses.TokenResponse-" + user);
                if (File.Exists(tokenPath)) {
                    // Load the existing token
                    using (var stream = new FileStream(tokenPath, FileMode.Open, FileAccess.Read)) {
                        var tokenResponse = NewtonsoftJsonSerializer.Instance.Deserialize<TokenResponse>(stream);
                        if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken)) {
                            if (tokenResponse.IsExpired(SystemClock.Default)) {
                                // Token is expired, refresh it
                                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer {
                                    ClientSecrets = new ClientSecrets {
                                        ClientId = clientId,
                                        ClientSecret = clientSecret
                                    },
                                    Scopes = ["https://mail.google.com/"]
                                });
                                var credential = new UserCredential(flow, user, tokenResponse);
                                credential.RefreshTokenAsync(CancellationToken.None).Wait();
                                Console.WriteLine("Access token refreshed.");
                                accessToken = credential.Token.AccessToken;
                                // Save new token
                                using var fileStream = new FileStream(tokenPath, FileMode.OpenOrCreate, FileAccess.Write);
                                var newTokenResponse = new TokenResponse {
                                    AccessToken = accessToken,
                                    RefreshToken = credential.Token.RefreshToken,
                                    ExpiresInSeconds = credential.Token.ExpiresInSeconds,
                                    Scope = credential.Token.Scope,
                                    TokenType = credential.Token.TokenType,
                                    IdToken = credential.Token.IdToken,
                                    IssuedUtc = credential.Token.IssuedUtc
                                };
                                NewtonsoftJsonSerializer.Instance.Serialize(newTokenResponse, fileStream);
                            }
                            else {
                                // Use the existing token
                                Console.WriteLine("Using existing access token.");
                                accessToken = tokenResponse.AccessToken;
                            }
                        }
                    }
                }
                else {
                    // Token file does not exist, create a new one
                    var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer {
                        ClientSecrets = new ClientSecrets {
                            ClientId = clientId,
                            ClientSecret = clientSecret,
                        },
                        Scopes = ["https://mail.google.com/"]
                    });
                    var authorizationCode = GetAuthorizationCode(flow, user);
                    var tokenResponse = flow.ExchangeCodeForTokenAsync(user, authorizationCode, "urn:ietf:wg:oauth:2.0:oob:auto", CancellationToken.None).Result;
                    accessToken = tokenResponse.AccessToken;
                    // Save the token to a file
                    using var fileStream = new FileStream(tokenPath, FileMode.Create, FileAccess.Write);
                    NewtonsoftJsonSerializer.Instance.Serialize(tokenResponse, fileStream);
                }

                // Create the email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, emailSender));
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

        private static string? GetAuthorizationCode(GoogleAuthorizationCodeFlow flow, string user) {
            // Implement a method to retrieve the authorization code from the user
            // This could involve displaying a URL to the user and asking them to paste the code
            // they receive after authorizing the application.
            // For example:
            // 1. Generate the authorization URL
            var authorizationUrl = flow.CreateAuthorizationCodeRequest("urn:ietf:wg:oauth:2.0:oob").Build();
            Console.WriteLine("Please visit the following URL to authorize the application:");
            Console.WriteLine(authorizationUrl);
            // 2. Ask the user to enter the authorization code
            Console.WriteLine("Enter the authorization code:");
            string? authorizationCode = Console.ReadLine();
            // 3. Return the authorization code
            return authorizationCode;
        }
    }
}