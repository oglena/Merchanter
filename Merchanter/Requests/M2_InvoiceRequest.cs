public class M2_InvoiceRequest {
    public bool capture { get; set; }
    public bool notify { get; set; }
    public bool appendComment { get; set; }
    public List<M2_InvoiceItems> items { get; set; }
    public M2_InvoiceComment comment { get; set; }
    public M2_InvoiceArguments arguments { get; set; }
}

public class M2_InvoiceItems {
    public M2_Invoice_Extension_Attributes extension_attributes { get; set; }
    public int order_item_id { get; set; }
    public int qty { get; set; }
}

public class M2_InvoiceComment {
    public M2_Invoice_Extension_Attributes extension_attributes { get; set; }
    public string comment { get; set; }
    public int is_visible_on_front { get; set; }
}

public class M2_InvoiceArguments {
    public M2_Invoice_Extension_Attributes extension_attributes { get; set; }
}

public class M2_Invoice_Extension_Attributes {

}