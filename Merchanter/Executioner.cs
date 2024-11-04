using Merchanter.Classes;
using RestSharp;
using System.Diagnostics;

namespace Merchanter {
    public class Executioner :IDisposable {

        /// <summary>
        /// The rest client to execute the requests
        /// </summary>
        private RestClient restClient;

        public Executioner() {
            var httpClient = new HttpClient {
                Timeout = TimeSpan.FromMinutes( 3 ), // Set the desired timeout in minutes
                MaxResponseContentBufferSize = 256000000 // Set the desired max size in bytes (e.g., 256 MB)
            };
            restClient = new RestClient( httpClient );
        }

        /// <summary>
        /// Execute a request to the API
        /// </summary>
        /// <param name="_url">The URL to execute the request</param>
        /// <param name="_method">The method to execute the request</param>
        /// <param name="_json">The JSON object to send</param>
        /// <param name="_token">The token to authenticate the request</param>
        /// <returns>The response of the request</returns>
        public virtual string? Execute( string _url, Method _method, object? _json, string? _token ) {
            try {
                var request = new RestRequest( _url, _method );
                request.AddHeader( "Accept", "application/json" );
                request.AddHeader( "Content-Type", "application/json" );
                if( !string.IsNullOrWhiteSpace( _token ) ) request.AddHeader( "Authorization", "Bearer " + _token );

                if( _method == Method.Post ) {
                    if( _json != null ) {
                        request.AddJsonBody( _json );
                    }

                    var response = restClient.Execute( request );
                    if( response.StatusCode == System.Net.HttpStatusCode.OK ) {
                        Debug.WriteLine( "Executed!.. M:" + _method.ToString() + " | URL:'" + _url + "'" );
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if( _method == Method.Get ) {
                    var response = restClient.Execute( request );
                    if( response.StatusCode == System.Net.HttpStatusCode.OK ) {
                        Debug.WriteLine( "Executed!.. M:" + _method.ToString() + " | URL:'" + _url + "'" );
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if( _method == Method.Delete ) {
                    var response = restClient.Execute( request );
                    if( response.StatusCode == System.Net.HttpStatusCode.OK ) {
                        Debug.WriteLine( "Executed!.. M:" + _method.ToString() + " | URL:'" + _url + "'" );
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if( _method == Method.Put ) {
                    if( _json != null ) {
                        request.AddJsonBody( _json );
                    }
                    var response = restClient.Execute( request );
                    if( response.StatusCode == System.Net.HttpStatusCode.OK ) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else {
                    return null;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( ex );
                return null;
            }
        }

        public string ConvertDateToString( DateTime date, bool is_ts ) {
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

        public void Dispose() {
            restClient.Dispose();
        }
    }
}
