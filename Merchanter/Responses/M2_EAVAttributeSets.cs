
public class M2_EAVAttributeSets {
    public M2_EAVAttributeSet[] items { get; set; }
    public M2_SearchCriteria search_criteria { get; set; }
    public int total_count { get; set; }
}

public class M2_EAVAttributeSet {
    public int attribute_set_id { get; set; }
    public string attribute_set_name { get; set; }
    public int sort_order { get; set; }
    public int entity_type_id { get; set; }
}
