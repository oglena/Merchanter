using MerchanterHelper.Classes;
using System.Xml.Serialization;

namespace MerchanterHelper {
    public class FSP {
        public string base_url { get; set; }

        public FSP( string _url ) {
            base_url = _url;
        }

        public XProducts_FSP? GetProducts( out string _status ) {
            try {
                using( Executioner executioner = new Executioner() ) {
                    string? result_fsp_catalog = executioner.ExecuteFSP( base_url, RestSharp.Method.Get );
                    if( result_fsp_catalog != null ) {
                        var fsp_catalog = DeserializeObject<XProducts_FSP>( result_fsp_catalog );
                        if( fsp_catalog != null && fsp_catalog.Product != null ) {
                            _status = "success";
                            return fsp_catalog;
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
