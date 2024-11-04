using System.Diagnostics;

namespace Merchanter {
    public static partial class Helper {
        #region ENTEGRA
        /// <summary>
        /// Get the currency rates from Entegra
        /// </summary>
        /// <returns>The currency rates</returns>
        public static ENT_CurrencyRates? GetENTCurrencyRates() {
            try {
                using( Executioner executioner = new Executioner() ) {
                    var entegraapi_token = new { username = Helper.global.entegra.api_username, password = Helper.global.entegra.api_password };
                    var json_ent_auth = executioner.Execute( Helper.global.entegra.api_url + "api/Auth/Login", RestSharp.Method.Post, entegraapi_token, null );
                    if( json_ent_auth != null ) {
                        var ent_token = Newtonsoft.Json.JsonConvert.DeserializeObject<ENT_TokenResponse>( json_ent_auth );
                        if( ent_token != null ) {
                            var json_ent_currency_rates = executioner.Execute( Helper.global.entegra.api_url + "api/Product/GetCurrencyRates", RestSharp.Method.Post, null, ent_token.authToken );
                            if( json_ent_currency_rates != null ) {
                                return Newtonsoft.Json.JsonConvert.DeserializeObject<ENT_CurrencyRates>( json_ent_currency_rates );
                            }
                            else {
                                Console.WriteLine( "connection_error" + " " + "EntegraUser: " + entegraapi_token.username + " cannot get currency rates." );
                            }
                        }

                    }
                    else {
                        Console.WriteLine( "connection_error" + " " + "EntegraUser: " + entegraapi_token.username + " cannot login." );
                    }
                    return null;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }

        /// <summary>
        /// Get the products from Entegra
        /// </summary>
        /// <returns>The products</returns>
        public static List<ENT_Product>? GetENTProducts() {
            try {
                using( Executioner executioner = new Executioner() ) {
                    var entegraapi_token = new { username = Helper.global.entegra.api_username, password = Helper.global.entegra.api_password };
                    var json_ent_auth = executioner.Execute( Helper.global.entegra.api_url + "api/Auth/Login", RestSharp.Method.Post, entegraapi_token, null );
                    if( json_ent_auth != null ) {
                        var ent_token = Newtonsoft.Json.JsonConvert.DeserializeObject<ENT_TokenResponse>( json_ent_auth );
                        if( ent_token != null ) {
                            var json_ent_currency_rates = executioner.Execute( Helper.global.entegra.api_url + "api/Product/GetCurrencyRates", RestSharp.Method.Post, null, ent_token.authToken );
                            if( json_ent_currency_rates != null ) {
                                var ent_currency_rates = Newtonsoft.Json.JsonConvert.DeserializeObject<ENT_CurrencyRates>( json_ent_currency_rates );
                                if( ent_currency_rates != null ) {
                                    var json_ent_products = executioner.Execute( Helper.global.entegra.api_url + "api/Product/GetProducts", RestSharp.Method.Post, null, ent_token.authToken );
                                    if( json_ent_products != null ) {
                                        return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ENT_Product>>( json_ent_products );
                                    }
                                    else {
                                        Console.WriteLine( "connection_error" + " " + "EntegraUser: " + entegraapi_token.username + " cannot get products." );
                                    }
                                }
                            }
                            else {
                                Console.WriteLine( "connection_error" + " " + "EntegraUser: " + entegraapi_token.username + " cannot get currency rates." );
                            }
                        }

                    }
                    else {
                        Console.WriteLine( "connection_error" + " " + "EntegraUser: " + entegraapi_token.username + " cannot login." );
                    }
                    return null;
                }
            }
            catch( Exception ex ) {
                Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + ex.ToString() );
                return null;
            }
        }
        #endregion
    }
}
