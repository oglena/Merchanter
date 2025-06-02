namespace Merchanter.Responses {
    public record class M2_ConfigurableChild {
        public int id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public int attribute_set_id { get; set; }
        public decimal price { get; set; }
        public int status { get; set; }
        public int visibility { get; set; }
        public string type_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public M2_ProductProductLinks[] product_links { get; set; }
        public object[] tier_prices { get; set; }
        public M2_ProductCustomAttributes[] custom_attributes { get; set; }
    }
}