// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class M2_AttributeSet {
    public int attribute_set_id { get; set; }
    public string attribute_set_name { get; set; }
    public int sort_order { get; set; }
    public int entity_type_id { get; set; }
}

public class M2_AttributeSets {
    public List<M2_AttributeSet> items { get; set; }
    public SearchCriteria search_criteria { get; set; }
    public int total_count { get; set; }
}