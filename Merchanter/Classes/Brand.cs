namespace Merchanter.Classes {
    public record class Brand {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string brand_name { get; set; }
        public bool status { get; set; } = false;
    }
}
