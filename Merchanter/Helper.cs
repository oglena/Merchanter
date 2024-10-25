using Merchanter.Classes.Settings;
using System.Diagnostics;
using System.Net;

namespace Merchanter {
    public static partial class Helper {
        public static SettingsMerchanter global { get; set; }

        #region Helper Functions
        public static void PostPageAll( object _url ) {
            try {
                if( !string.IsNullOrWhiteSpace( _url.ToString() ) ) {
                    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Magento Indexer Started" );
                    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Magento Indexer Started" );
                    HttpWebRequest? webReq = WebRequest.Create( _url.ToString() ) as HttpWebRequest;
                    if( webReq != null ) {
                        try {
                            webReq.CookieContainer = new CookieContainer();
                            webReq.Method = "GET";
                            using( WebResponse response = webReq.GetResponse() ) {
                                using( Stream stream = response.GetResponseStream() ) {
                                    StreamReader reader = new StreamReader( stream );
                                    string res = reader.ReadToEnd();
                                }
                            }
                        }
                        catch( Exception ex ) {
                            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                            Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                        }
                    }
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
            } finally {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Magento Indexer Ended" );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Magento Indexer Ended" );
            }
        }

        private static string ConvertFriendly( string _str ) {
            return _str.Replace( "/", "&#47;" );
        }

        private static string ConvertDateToString( DateTime date, bool is_ts ) {
            DateTime utcTime;
            if( date.Kind == DateTimeKind.Local ) {
                utcTime = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    date.Hour,
                    date.Minute,
                    date.Second,
                    date.Millisecond,
                    DateTimeKind.Local ).ToUniversalTime();
            }
            else {
                utcTime = date;
            }
            if( is_ts )
                return utcTime.ToString( "yyyy-MM-dd\\THH:mm:ss.ff\\Z", System.Globalization.CultureInfo.InvariantCulture );
            else
                return utcTime.ToString( "yyyy-MM-dd\\T21:00:00.00\\Z", System.Globalization.CultureInfo.InvariantCulture );
        }

        public static string GetProductFinalPrice( decimal _price_value, ENT_CurrencyRates _ent_currency_rates, string _currency, bool _tax_included, int _tax ) {
            return Math.Round( _price_value * (_tax_included ? 1 : (1 + ((decimal)_tax / 100m))) * ((_currency == "USD") ? _ent_currency_rates.USD : ((_currency == "EUR") ? _ent_currency_rates.EUR : 1)), 2, MidpointRounding.AwayFromZero ).ToString() + " TL";
        }

        public static string GetProductFinalPrice( decimal _price_value, string _currency, bool _tax_included, int _tax ) {
            return Math.Round( _price_value * (_tax_included ? 1 : (1 + ((decimal)_tax / 100m))), 2, MidpointRounding.AwayFromZero ).ToString() + " " + _currency;
        }

        public static void UploadFileToFtp( string url, string filePath, string username, string password ) {
            var fileName = Path.GetFileName( filePath );
            var request = (FtpWebRequest)WebRequest.Create( url + fileName );

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential( username, password );
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using( var fileStream = File.OpenRead( filePath ) ) {
                using( var requestStream = request.GetRequestStream() ) {
                    fileStream.CopyTo( requestStream );
                    requestStream.Close();
                }
            }

            var response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine( "Upload done: {0}", response.StatusDescription );
            response.Close();
        }
        #endregion
    }
}
