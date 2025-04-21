using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter.Classes {
    public class Notification {
        public int id { get; set; }
        public int customer_id { get; set; }
        public NotificationTypes type { get; set; }
        public string? order_label { get; set; } = null;
        public string? product_sku { get; set; } = null;
        public string? xproduct_barcode { get; set; } = null;
        public string? invoice_no { get; set; } = null;
        public string? notification_content { get; set; }
        public bool is_notification_sent { get; set; } = false;
        public DateTime create_date { get; set; }
        public DateTime? notification_date { get; set; }

        public enum NotificationTypes {
            GENERAL = 0,
            NEW_ORDER = 1,
            OUT_OF_STOCK_PRODUCT_SOLD = 2,
            PRODUCT_IN_STOCK = 3,
            PRODUCT_OUT_OF_STOCK = 4,
            XML_PRODUCT_ADDED = 5,
            XML_PRODUCT_REMOVED = 6,
            XML_PRICE_CHANGED = 7,
            PRODUCT_PRICE_UPDATE_ERROR = 8,
            PRODUCT_SPECIAL_PRICE_UPDATE_ERROR = 9,
            PRODUCT_CUSTOM_PRICE_UPDATE_ERROR = 10,
            PRODUCT_QTY_UPDATE_ERROR = 11,
            XML_SYNC_ERROR = 12,
            XML_QTY_CHANGED = 13,
            XML_PRODUCT_REMOVED_BY_USER = 14,
            XML_SOURCE_FAILED = 15,
            NEW_INVOICE = 16,
            ORDER_COMPLETE = 17,
            ORDER_PROCESS = 18,
            ORDER_SHIPPED = 19,
            PRODUCT_UPDATE_ERROR = 20,
            NEW_ORDER_ERROR = 21,
        }
    }
}
