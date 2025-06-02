using MerchanterHelper.Classes;
using RestSharp;
using System;
using System.IO;
using System.Xml.Serialization;

namespace MerchanterHelper {
    public class PENTA {
        public string base_url { get; set; }
        public string penta_api_customerid { get; set; }

        public PENTA( string _base_url, string _penta_api_customerid ) {
            base_url = _base_url;
            penta_api_customerid = _penta_api_customerid;
        }

        public XProducts_PENTA? GetProducts( out string _status ) {
            try {
                using( Executioner executioner = new Executioner() ) {
                    string? result_penta_catalog = executioner.ExecutePENTA( base_url + penta_api_customerid, Method.Get );
                    if( result_penta_catalog != null ) {
                        var penta_catalog = DeserializeObject<XProducts_PENTA>( result_penta_catalog );
                        if( penta_catalog != null && penta_catalog.Stok != null) {
                            _status = "success";
                            return penta_catalog;
                        }
                    }
                    _status = "fail";
                    return null;
                }
            }
            catch( Exception _ex ) {
                _status = "error" + _ex.ToString();
                return null;
            }
        }

        private T DeserializeObject<T>( string xml ) {
            try {
                var serializer = new XmlSerializer( typeof( T ) );
                using( var tr = new StringReader( xml ) ) {
                    return (T)serializer.Deserialize( tr );
                }
            }
            catch( Exception ex ) {
                throw new Exception( "An error occurred", ex );
            }
        }
    }
}
