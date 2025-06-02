using MerchanterHelper;
using System.Text.Json;

namespace MerchanterHelpers {
    public partial class HB {
        public string MerchantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private string Token { get; set; }
        public string base_url { get; set; } = "https://oms-external.hepsiburada.com";

        public HB(string _merchant_id, string _username, string _password) {
            MerchantId = _merchant_id;
            Token = _username + ":" + _password;
            UserName = _username;
            Password = _password;
        }

        public HBOrder? GetOrders(string _order_status, int _page_size = 100, int _current_page = 0) {
            using Executioner executioner = new Executioner();
            string? value = executioner.ExecuteHB(
                base_url + "/packages/merchantid/" + MerchantId +
                (!string.IsNullOrWhiteSpace(_order_status) ? "/" + _order_status : "") +
                "?offset=" + _current_page.ToString() + "&limit=" + _page_size.ToString(),
                RestSharp.Method.Get, null, Token
            );

            if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "[]") {
                return JsonSerializer.Deserialize<HBOrder>(value);
            }
            else {
                return null;
            }
        }
    }
}

public class HBOrder {
    public int totalCount { get; set; }
    public int limit { get; set; }
    public int offset { get; set; }
    public int pageCount { get; set; }
    public HBItem[] items { get; set; }
}

public class HBItem {
    public string Id { get; set; }
    public string Barcode { get; set; }
    public string PackageNumber { get; set; }
    public string OrderNumber { get; set; }
    public string MerchantId { get; set; }
    public object ShippedDate { get; set; }
}
