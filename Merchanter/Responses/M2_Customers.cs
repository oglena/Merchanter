
public class M2_Customers {
    public M2_Customer[] items { get; set; }
    public M2_Search_Criteria search_criteria { get; set; }
    public int total_count { get; set; }
}

public class M2_Customer {
    public int id { get; set; }
    public int group_id { get; set; }
    public string default_billing { get; set; }
    public string default_shipping { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public string created_in { get; set; }
    public string email { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public int gender { get; set; }
    public int store_id { get; set; }
    public int website_id { get; set; }
    public int disable_auto_group_change { get; set; }
    public M2_Order_Address[] addresses { get; set; }
    public M2_Order_Extension_Attributes extension_attributes { get; set; }
    public M2_Order_Custom_Attributes[] custom_attributes { get; set; }
}

public class M2_Order_Extension_Attributes {
    public bool is_subscribed { get; set; }
}

public class M2_Order_Address {
    public int id { get; set; }
    public int customer_id { get; set; }
    public M2_Region region { get; set; }
    public int region_id { get; set; }
    public string country_id { get; set; }
    public string[] street { get; set; }
    public string telephone { get; set; }
    public string postcode { get; set; }
    public string city { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public bool default_shipping { get; set; }
    public bool default_billing { get; set; }
}

public class M2_Region {
    public string region_code { get; set; }
    public string region { get; set; }
    public int region_id { get; set; }
}

public class M2_Order_Custom_Attributes {
    public string attribute_code { get; set; }
    public string value { get; set; }
}
