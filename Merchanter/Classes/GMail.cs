using System.Net;
using System.Net.Mail;

namespace Merchanter.Classes {
    public static class GMail {
        public static bool Send( string _email_account, string _email_pass, string _email_sender, string _email, string _email_subject, string _email_content ) {
            try {
                NetworkCredential cred = new( _email_account, _email_pass );
                MailMessage mail = new MailMessage {
                    From = new MailAddress( _email_account, _email_sender )
                };
                mail.To.Add( _email );
                mail.Subject = _email_subject;
                mail.IsBodyHtml = true;
                mail.Body = _email_content;

                SmtpClient smtp = new( "smtp.gmail.com", 587 );
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = cred;

                smtp.Send( mail );
                Console.WriteLine( "Email sent." + _email_account + Environment.NewLine + "Subject: " + _email_subject + Environment.NewLine + "Content: " + _email_content );
                return true;
            }
            catch {
                return false;
            }
        }

        public static bool Send( string _email_account, string _email_pass, string _email_sender, string[] _emails, string _email_subject, string _email_content ) {
            try {
                NetworkCredential cred = new( _email_account, _email_pass );

                MailMessage mail = new MailMessage {
                    From = new MailAddress( _email_account, _email_sender )
                };
                for( int i = 0; i < _emails.Length; i++ ) {
                    mail.To.Add( _emails[ i ] );
                }
                mail.Subject = _email_subject;
                mail.IsBodyHtml = true;
                mail.Body = _email_content;

                SmtpClient smtp = new( "smtp.gmail.com", 587 ) {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = cred
                };
                smtp.Send( mail );
                Console.WriteLine( "Email sent." + _email_account + Environment.NewLine + "Subject: " + _email_subject + Environment.NewLine + "Content: " + _email_content );
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
