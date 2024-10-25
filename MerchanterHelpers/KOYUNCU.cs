﻿using RestSharp;
using System;
using System.IO;
using System.Xml.Serialization;

namespace MerchanterHelpers {
    public class KOYUNCU {
        public string base_url { get; set; }

        public KOYUNCU( string _url ) {
            base_url = _url;
        }

        public XProducts_KOYUNCU? GetProducts( out string _status ) {
            try {
                using( Executioner executioner = new Executioner() ) {
                    string? result_koyuncu_catalog = executioner.ExecuteKOYUNCU( base_url, Method.Get );
                    if( result_koyuncu_catalog != null ) {
                        var koyuncu_catalog = DeserializeObject<XProducts_KOYUNCU>( result_koyuncu_catalog );
                        if( koyuncu_catalog != null && koyuncu_catalog.ProductDto != null ) {
                            _status = "success";
                            return koyuncu_catalog;
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
