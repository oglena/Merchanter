using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Merchanter.Classes {
    public class Customer {
        [Required]
        public int customer_id { get; set; }
        [Required]
        public string user_name { get; set; }
        //[JsonIgnore]
        public string? password { get; set; }
        public bool status { get; set; } = true;
        public bool product_sync_status { get; set; } = false;
        public bool order_sync_status { get; set; } = false;
        public bool xml_sync_status { get; set; } = false;
        public bool invoice_sync_status { get; set; } = false;
        public bool notification_sync_status { get; set; } = false;
        public int order_sync_timer { get; set; } = 25;
        public int product_sync_timer { get; set; } = 60;
        public int xml_sync_timer { get; set; } = 14400;
        public int invoice_sync_timer { get; set; } = 9000;
        public int notification_sync_timer { get; set; } = 120;
        public DateTime? last_product_sync_date { get; set; }
        public DateTime? last_order_sync_date { get; set; }
        public DateTime? last_xml_sync_date { get; set; }
        public DateTime? last_invoice_sync_date { get; set; }
        public DateTime? last_notification_sync_date { get; set; }
        public bool is_productsync_working { get; set; } = false;
        public bool is_ordersync_working { get; set; } = false;
        public bool is_xmlsync_working { get; set; } = false;
        public bool is_invoicesync_working { get; set; } = false;
        public bool is_notificationsync_working { get; set; } = false;
    }
}
