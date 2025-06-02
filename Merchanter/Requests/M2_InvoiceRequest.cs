namespace Merchanter.Requests {
    public record class M2_InvoiceRequest {
        public bool capture { get; set; }
        public bool notify { get; set; }
        public bool appendComment { get; set; }
        public List<M2_InvoiceRequestInvoiceItems> items { get; set; }
        public M2_InvoiceRequestInvoiceComment comment { get; set; }
        public M2_InvoiceRequestInvoiceArguments arguments { get; set; }
    }

    public record class M2_InvoiceRequestInvoiceItems {
        public M2_InvoiceRequestInvoice_Extension_Attributes extension_attributes { get; set; }
        public int order_item_id { get; set; }
        public int qty { get; set; }
    }

    public record class M2_InvoiceRequestInvoiceComment {
        public M2_InvoiceRequestInvoice_Extension_Attributes extension_attributes { get; set; }
        public string comment { get; set; }
        public int is_visible_on_front { get; set; }
    }

    public record class M2_InvoiceRequestInvoiceArguments {
        public M2_InvoiceRequestInvoice_Extension_Attributes extension_attributes { get; set; }
    }

    public record class M2_InvoiceRequestInvoice_Extension_Attributes {

    }
}