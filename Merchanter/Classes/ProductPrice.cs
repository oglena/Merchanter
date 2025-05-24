using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public class ProductPrice {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public string platform_name { get; set; }
        public decimal price1 { get; set; }
        public decimal price2 { get; set; } = 0;
        public Currency update_currency_as { get; set; } = Currency.GetCurrency("TL");
        public DateTime update_date { get; set; } = DateTime.Now;

        [JsonConstructor]
        public ProductPrice() {
        }
    }
}
