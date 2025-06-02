namespace MerchanterHelper {
    public partial class TY {
        public string SupplierId { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string Token { get; set; }
        public string UserAgent { get; set; }
        public string base_url { get; set; } = "https://api.trendyol.com/sapigw";

        public TY( string _supplierid, string _apikey, string _apisecret ) {
            SupplierId = _supplierid;
            ApiKey = _apikey;
            ApiSecret = _apisecret;
            Token = ApiKey + ":" + ApiSecret;
            UserAgent = _supplierid + " - " + "SelfIntegration";
        }

        public void GetOrders() {
            using( Executioner executioner = new Executioner() ) {
                executioner.ExecuteTY( base_url + "/suppliers/" + SupplierId + "/orders?page=0&size=100", RestSharp.Method.Get, null, Token );
            }
        }
    }
}
