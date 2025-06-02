namespace Merchanter.Responses {
    public record class M2_CategoryList {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
        public bool? is_active { get; set; }
        public int position { get; set; }
        public int level { get; set; }
        public int product_count { get; set; }
        public M2_CategoryList[] children_data { get; set; }
    }
}