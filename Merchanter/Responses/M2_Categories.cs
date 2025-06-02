
namespace Merchanter.Responses {
    public record class M2_Categories {
        public M2_Category[] items { get; set; }
        public M2_SearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }
    }

    public record class M2_Category {
        public int id { get; set; }
        public int parent_id { get; set; }
        public int position { get; set; }
        public int level { get; set; }
        public string children { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string path { get; set; }
        public string[] available_sort_by { get; set; }
        public M2_CustomAttributes[] custom_attributes { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
        public bool include_in_menu { get; set; }
    }

    public record class M2_CustomAttributes {
        public string attribute_code { get; set; }
        public string value { get; set; }
    }
}