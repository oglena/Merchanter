public class M2_Orders {
    public M2_Order[] items { get; set; }
    public M2_Search_Criteria search_criteria { get; set; }
    public int total_count { get; set; }
}

public class M2_Search_Criteria {
    public M2_Filter_Groups[] filter_groups { get; set; }
    public int page_size { get; set; }
    public int current_page { get; set; }
}

public class M2_Filter_Groups {
    public M2_Filter[] filters { get; set; }
}

public class M2_Filter {
    public string field { get; set; }
    public string value { get; set; }
    public string condition_type { get; set; }
}

public class M2_Order {
    public string base_currency_code { get; set; }
    public float base_discount_amount { get; set; }
    public float base_grand_total { get; set; }
    public float base_discount_tax_compensation_amount { get; set; }
    public float base_shipping_amount { get; set; }
    public float base_shipping_discount_amount { get; set; }
    public float base_shipping_incl_tax { get; set; }
    public float base_shipping_tax_amount { get; set; }
    public float base_subtotal { get; set; }
    public float base_tax_amount { get; set; }
    public float base_total_due { get; set; }
    public float base_to_global_rate { get; set; }
    public float base_to_order_rate { get; set; }
    public int billing_address_id { get; set; }
    public string created_at { get; set; }
    public string customer_email { get; set; }
    public string customer_firstname { get; set; }
    public int customer_group_id { get; set; }
    public int customer_id { get; set; }
    public int customer_is_guest { get; set; }
    public string customer_lastname { get; set; }
    public int customer_note_notify { get; set; }
    public float discount_amount { get; set; }
    public int email_sent { get; set; }
    public int entity_id { get; set; }
    public string global_currency_code { get; set; }
    public float grand_total { get; set; }
    public float discount_tax_compensation_amount { get; set; }
    public string increment_id { get; set; }
    public int is_virtual { get; set; }
    public string order_currency_code { get; set; }
    public string protect_code { get; set; }
    public int quote_id { get; set; }
    public string remote_ip { get; set; }
    public float shipping_amount { get; set; }
    public string shipping_description { get; set; }
    public float shipping_discount_amount { get; set; }
    public float shipping_discount_tax_compensation_amount { get; set; }
    public float shipping_incl_tax { get; set; }
    public float shipping_tax_amount { get; set; }
    public string state { get; set; }
    public string status { get; set; }
    public string store_currency_code { get; set; }
    public int store_id { get; set; }
    public string store_name { get; set; }
    public float store_to_base_rate { get; set; }
    public float store_to_order_rate { get; set; }
    public float subtotal { get; set; }
    public float subtotal_incl_tax { get; set; }
    public float tax_amount { get; set; }
    public float total_due { get; set; }
    public int total_item_count { get; set; }
    public int total_qty_ordered { get; set; }
    public string updated_at { get; set; }
    public float weight { get; set; }
    public M2_Order_Item[] items { get; set; }
    public M2_Billing_Address billing_address { get; set; }
    public M2_Payment payment { get; set; }
    public M2_Status_Histories[] status_histories { get; set; }
    public M2_Extension_Attributes extension_attributes { get; set; }
}

public class M2_Billing_Address {
    public string address_type { get; set; }
    public string city { get; set; }
    public string country_id { get; set; }
    public string email { get; set; }
    public int entity_id { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public int parent_id { get; set; }
    public string postcode { get; set; }
    public string region { get; set; }
    public string region_code { get; set; }
    public string[] street { get; set; }
    public string telephone { get; set; }
}

public class M2_Payment {
    public object account_status { get; set; }
    public string[] additional_information { get; set; }
    public float amount_ordered { get; set; }
    public float base_amount_ordered { get; set; }
    public float base_shipping_amount { get; set; }
    public object cc_last4 { get; set; }
    public int entity_id { get; set; }
    public string method { get; set; }
    public int parent_id { get; set; }
    public float shipping_amount { get; set; }
}

public class M2_Extension_Attributes {
    public bool converting_from_quote { get; set; }
    public M2_Shipping_Assignments[] shipping_assignments { get; set; }
    public M2_Configurable_Item_Options[] configurable_item_options { get; set; }
    public M2_Bundle_Options[] bundle_options { get; set; }
    public M2_Payment_AdditionalInfo[] payment_additional_info { get; set; }
    public M2_Applied_Taxes[] applied_taxes { get; set; }
    public M2_ItemApplied_Taxes[] item_applied_taxes { get; set; }
}

public class M2_ItemApplied_Taxes {
    public string type { get; set; }
    public string item_id { get; set; }
    public M2_Applied_Taxes[] applied_taxes { get; set; }
}

public class M2_Applied_Taxes {
    public string code { get; set; }
    public string title { get; set; }
    public int percent { get; set; }
    public float amount { get; set; }
    public float base_amount { get; set; }
}

public class M2_Shipping_Assignments {
    public M2_Shipping shipping { get; set; }
    public M2_Order_Item[] items { get; set; }
}

public class M2_Payment_AdditionalInfo {
    public string key { get; set; }
    public string value { get; set; }
}

public class M2_Configurable_Item_Options {
    public int option_id { get; set; }
    public int option_value { get; set; }
}

public class M2_Bundle_Options {
    public int option_id { get; set; }
    public int option_qty { get; set; }
    public int[] option_selections { get; set; }
}

public class M2_Shipping {
    public M2_Address address { get; set; }
    public string method { get; set; }
    public M2_Total total { get; set; }
}

public class M2_Address {
    public string address_type { get; set; }
    public string city { get; set; }
    public string country_id { get; set; }
    public int customer_address_id { get; set; }
    public string email { get; set; }
    public int entity_id { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public int parent_id { get; set; }
    public string postcode { get; set; }
    public string region { get; set; }
    public string region_code { get; set; }
    public string[] street { get; set; }
    public string telephone { get; set; }
}

public class M2_Total {
    public float base_shipping_amount { get; set; }
    public float base_shipping_discount_amount { get; set; }
    public float base_shipping_incl_tax { get; set; }
    public float base_shipping_tax_amount { get; set; }
    public float shipping_amount { get; set; }
    public float shipping_discount_amount { get; set; }
    public float shipping_discount_tax_compensation_amount { get; set; }
    public float shipping_incl_tax { get; set; }
    public float shipping_tax_amount { get; set; }
}

public class M2_Order_Item {
    public float amount_refunded { get; set; }
    public float base_amount_refunded { get; set; }
    public float base_discount_amount { get; set; }
    public float base_discount_invoiced { get; set; }
    public float base_discount_tax_compensation_amount { get; set; }
    public float base_price { get; set; }
    public float base_price_incl_tax { get; set; }
    public float base_row_invoiced { get; set; }
    public float base_row_total { get; set; }
    public float base_row_total_incl_tax { get; set; }
    public float base_tax_amount { get; set; }
    public float base_tax_invoiced { get; set; }
    public string created_at { get; set; }
    public float discount_amount { get; set; }
    public float discount_invoiced { get; set; }
    public int discount_percent { get; set; }
    public int free_shipping { get; set; }
    public float discount_tax_compensation_amount { get; set; }
    public int is_qty_decimal { get; set; }
    public int is_virtual { get; set; }
    public int item_id { get; set; }
    public string name { get; set; }
    public int no_discount { get; set; }
    public int order_id { get; set; }
    public float original_price { get; set; }
    public float price { get; set; }
    public float price_incl_tax { get; set; }
    public int product_id { get; set; }
    public string product_type { get; set; }
    public int qty_canceled { get; set; }
    public int qty_invoiced { get; set; }
    public int qty_ordered { get; set; }
    public int qty_refunded { get; set; }
    public int qty_shipped { get; set; }
    public int quote_item_id { get; set; }
    public float row_invoiced { get; set; }
    public float row_total { get; set; }
    public float row_total_incl_tax { get; set; }
    public float row_weight { get; set; }
    public string sku { get; set; }
    public int store_id { get; set; }
    public float tax_amount { get; set; }
    public float tax_invoiced { get; set; }
    public int tax_percent { get; set; }
    public string updated_at { get; set; }
    public M2_Order_Item parent_item { get; set; }
    public M2_Product_Option product_option { get; set; }
}

public class M2_Product_Option {
    public M2_Extension_Attributes extension_attributes { get; set; }
}

public class M2_Status_Histories {
    public string comment { get; set; }
    public string created_at { get; set; }
    public int entity_id { get; set; }
    public string entity_name { get; set; }
    public object is_customer_notified { get; set; }
    public int is_visible_on_front { get; set; }
    public int parent_id { get; set; }
    public string status { get; set; }
}