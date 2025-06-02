using MerchanterHelper.Classes;
using System;
using System.IO;
using System.Xml.Serialization;

namespace MerchanterHelper {
    public class OKSID {
        public string base_url { get; set; }

        public OKSID( string _url ) {
            base_url = _url;
        }

        public XProducts_OKSID? GetProducts( out string _status ) {
            try {
                using( Executioner executioner = new Executioner() ) {
                    string? result_oksid_catalog = executioner.ExecuteOKSID( base_url, RestSharp.Method.Post );
                    if( result_oksid_catalog != null ) {
                        var oksid_catalog = DeserializeObject<XProducts_OKSID>( result_oksid_catalog );
                        if( oksid_catalog != null && oksid_catalog.Stok != null ) {
                            _status = "success";
                            return oksid_catalog;
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
