public class M2_Products {
    public M2_Product[] items { get; set; }
    public SearchCriteria search_criteria { get; set; }
    public int total_count { get; set; }
}

public class M2_Product {
    public int id { get; set; }
    public string sku { get; set; }
    public string name { get; set; }
    public int attribute_set_id { get; set; }
    public decimal price { get; set; }
    public int status { get; set; }
    public int visibility { get; set; }
    public string type_id { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public float weight { get; set; }
    public M2_ProductExtensionAttributes extension_attributes { get; set; }
    public M2_ProductProductLinks[] product_links { get; set; }
    public object[] options { get; set; }
    public M2_ProductMediaGalleryEntries[] media_gallery_entries { get; set; }
    public object[] tier_prices { get; set; }
    public M2_ProductCustomAttributes[] custom_attributes { get; set; }
}

public class M2_ProductExtensionAttributes {
    public int[] website_ids { get; set; }
    public M2_ProductCategoryLinks[] category_links { get; set; }
    public M2_StockItem stock_item { get; set; }
    public M2_BundleOption[] bundle_product_options { get; set; }
    public M2_ConfigurableOption[] configurable_product_options { get; set; }
    public int[] configurable_product_links { get; set; }
}

public class M2_ProductCategoryLinks {
    public int position { get; set; }
    public string category_id { get; set; }
}

public class M2_ProductProductLinks {
    public string sku { get; set; }
    public string link_type { get; set; }
    public string linked_product_sku { get; set; }
    public string linked_product_type { get; set; }
    public int position { get; set; }
}

public class M2_ProductMediaGalleryEntries {
    public int id { get; set; }
    public string media_type { get; set; }
    public object label { get; set; }
    public int position { get; set; }
    public bool disabled { get; set; }
    public string[] types { get; set; }
    public string file { get; set; }
    public M2_ProductExtensionAttributesVideo extension_attributes { get; set; }
}

public class M2_ProductExtensionAttributesVideo {
    public M2_ProductVideoContent video_content { get; set; }
}

public class M2_ProductVideoContent {
    public string media_type { get; set; }
    public object video_provider { get; set; }
    public string video_url { get; set; }
    public string video_title { get; set; }
    public string video_description { get; set; }
    public object video_metadata { get; set; }
}

public class M2_ProductCustomAttributes {
    public string attribute_code { get; set; }
    public string? value { get; set; }
}


public class M2_BundleOption {
    public int option_id { get; set; }
    public int position { get; set; }
    public string sku { get; set; }
    public string title { get; set; }
    public string type { get; set; }
    public bool required { get; set; }
    public M2_Product_Links[] product_links { get; set; }
}

public class M2_Product_Links {
    public string sku { get; set; }
    public int option_id { get; set; }
    public int qty { get; set; }
    public int position { get; set; }
    public bool is_default { get; set; }
    public decimal price { get; set; }
    public object price_type { get; set; }
    public int can_change_quantity { get; set; }
}

public class M2_ConfigurableOption {
    public int id { get; set; }
    public string attribute_id { get; set; }
    public string label { get; set; }
    public int position { get; set; }
    public M2_ConfigurableOptionValue[] values { get; set; }
    public int product_id { get; set; }
}

public class M2_ConfigurableOptionValue {
    public int? value_index { get; set; }
}

public class M2_ProductType {
    public string name { get; set; }
    public string label { get; set; }
}
