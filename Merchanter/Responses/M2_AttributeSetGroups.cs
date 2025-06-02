namespace Merchanter.Responses {
    public record class M2_AttributeSetGroups {
        public M2_AttributeSetGroup[] items { get; set; }
        public M2_SearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }
    }

    public record class M2_AttributeSetGroup {
        public int attribute_group_id { get; set; }
        public string attribute_group_name { get; set; }
        public int attribute_set_id { get; set; }
        public object[] extension_attributes { get; set; }
    }
}