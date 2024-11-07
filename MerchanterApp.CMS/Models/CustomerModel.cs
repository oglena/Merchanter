using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace MerchanterApp.CMS.Models {
    public class CustomerModel {
        [Required, IntegerValidator]
        public int customer_id { get; set; }
        [Required]
        public string user_name { get; set; }
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
    }
}
