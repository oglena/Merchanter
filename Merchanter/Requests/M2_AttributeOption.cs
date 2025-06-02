namespace Merchanter.Requests {
    public record class M2_AttributeOption {
        public string label { get; set; }
        public string value { get; set; }
        public int sort_order { get; set; }
        public bool is_default { get; set; }
        public List<M2_StoreLabel>? store_labels { get; set; }
    }
    public record class M2_StoreLabel {
        public int store_id { get; set; }
        public string label { get; set; }
    }
}