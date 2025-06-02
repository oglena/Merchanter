namespace Merchanter.Responses {
    public record class M2_AttributeSet {
        public int attribute_set_id { get; set; }
        public string attribute_set_name { get; set; }
        public int sort_order { get; set; }
        public int entity_type_id { get; set; }
    }

    public record class M2_AttributeSets {
        public List<M2_AttributeSet> items { get; set; }
        public M2_SearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }
    }
}