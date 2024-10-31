
public class M2_AttributeOption {
    public M2_Option option { get; set; }
}

public class M2_Option {
    public string label { get; set; }
    public string value { get; set; }
    public int sort_order { get; set; }
    public bool is_default { get; set; }
    public M2_StoreLabels[] store_labels { get; set; }
}

public class M2_StoreLabels {
    public int store_id { get; set; }
    public string label { get; set; }
}
