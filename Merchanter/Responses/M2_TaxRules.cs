namespace Merchanter.Responses {
    public record class M2_TaxRules {
        public int id { get; set; }
        public string code { get; set; }
        public int priority { get; set; }
        public int position { get; set; }
        public int[] customer_tax_class_ids { get; set; }
        public int[] product_tax_class_ids { get; set; }
        public int[] tax_rate_ids { get; set; }
        public bool calculate_subtotal { get; set; }
    }
}