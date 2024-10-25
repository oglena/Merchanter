﻿using Merchanter.Classes;
using RestSharp;

namespace Merchanter {
    public class Executioner :IDisposable {

        RestClient restClient = new RestClient();

        public virtual string? Execute( string _url, Method _method, object? _json, string? _token ) {
            try {
                restClient = new RestClient();
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
                        Log.It( "Executed!.. M:" + _method.ToString() + " | URL:'" + _url + "'" );
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if( _method == Method.Get ) {
                    var response = restClient.Execute( request );
                    if( response.StatusCode == System.Net.HttpStatusCode.OK ) {
                        Log.It( "Executed!.. M:" + _method.ToString() + " | URL:'" + _url + "'" );
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if( _method == Method.Delete ) {
                    var response = restClient.Execute( request );
                    if( response.StatusCode == System.Net.HttpStatusCode.OK ) {
                        Log.It( "Executed!.. M:" + _method.ToString() + " | URL:'" + _url + "'" );
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
                Log.It( ex );
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
