namespace Merchanter.Classes {
    public record class Category {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int source_category_id { get; set; }
        public int parent_id { get; set; }
        public string category_name { get; set; }
        public bool is_active { get; set; } = false;
    }
}
