using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public class PaymentMethod {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string payment_name { get; set; }
        public string payment_code { get; set; }
        public string platform { get; set; }
        public string platform_payment_code { get; set; }

        public PaymentMethod(int _id, int _customer_id, string _payment_name, string _payment_code, string _platform, string _platform_payment_code) {
            id = _id;
            customer_id = _customer_id;
            payment_name = _payment_name;
            payment_code = _payment_code;
            platform = _platform;
            platform_payment_code = _platform_payment_code;
        }

        [JsonConstructor]
        public PaymentMethod() { }

        public static string GetPaymentMethodOf(string _code, string _platform = "") {
            if (Helper.global.payment_methods == null) return string.Empty;
            return Helper.global.payment_methods.Where(x => x.platform_payment_code == _code && x.platform == _platform).First().payment_code;
        }
    }
}
