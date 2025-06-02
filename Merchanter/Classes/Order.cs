﻿namespace Merchanter.Classes
{
    public record class Order {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int order_id { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string order_label { get; set; }
        public string order_source { get; set; }
        public string payment_method { get; set; }
        public string shipment_method { get; set; }
        public string? comment { get; set; } = null;
        public string? order_shipping_barcode { get; set; } = null;
        public string? erp_no { get; set; } = null;
        public bool is_erp_sent { get; set; } = false;
        public float grand_total { get; set; }
        public float subtotal { get; set; }
        public float discount_amount { get; set; } = 0;
        public float installment_amount { get; set; } = 0;
        public float shipment_amount { get; set; } = 0;
        public string currency { get; set; }
        public string order_status { get; set; }
        public DateTime order_date { get; set; }
        public DateTime update_date { get; set; } = DateTime.Now;
        public List<OrderItem> order_items { get; set; }
        public BillingAddress billing_address { get; set; }
        public ShippingAddress shipping_address { get; set; } //TODO: shipping_amount
    }

    public record class OrderItem {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int order_id { get; set; }
        public int order_item_id { get; set; }
        public string sku { get; set; }
        public string? parent_sku { get; set; } = null;
        public float price { get; set; }
        public float tax_amount { get; set; }
        public int qty_ordered { get; set; }
        public int qty_invoiced { get; set; }
        public int qty_cancelled { get; set; }
        public int qty_refunded { get; set; }
        public int tax { get; set; } = 20;
        public bool tax_included { get; set; } = true;
    }

    public record class ShippingAddress {
        public int id { get; set; }
        public int shipping_id { get; set; }
        public int customer_id { get; set; }
        public int order_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string telephone { get; set; }
        public string street { get; set; }
        public string? region { get; set; } = null;
        public string city { get; set; }
    }

    public record class BillingAddress {
        public int id { get; set; }
        public int billing_id { get; set; }
        public int customer_id { get; set; }
        public int order_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string? telephone { get; set; } = null;
        public string street { get; set; }
        public string? region { get; set; } = null;
        public string city { get; set; }
        public bool is_corporate { get; set; } = false;
        public string? firma_ismi { get; set; } = null;
        public string? firma_vergidairesi { get; set; } = null;
        public string? firma_vergino { get; set; } = null;
        public string tc_no { get; set; } = Constants.DUMMY_TCNO;
    }
}
