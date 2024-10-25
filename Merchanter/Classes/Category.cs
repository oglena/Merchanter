namespace Merchanter.Classes {
    public class Category {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int parent_id { get; set; }
        public string category_name { get; set; } = string.Empty;
        public bool is_active { get; set; }
    }
}
