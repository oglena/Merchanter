namespace Merchanter.Requests {
    public record class M2_PriceRequest {
        public M2_PriceRequest() {
        }

        public decimal price { get; set; }
        public int store_id { get; set; }
        public string sku { get; set; }
        public M2_PriceRequestPriceExtensionAttributes[] extension_attributes { get; set; }
    }
}