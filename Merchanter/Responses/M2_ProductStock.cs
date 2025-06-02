namespace Merchanter.Responses {
    public record class M2_ProductStocks {
        public M2_ProductStock[] items { get; set; }
        public M2_StockSearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }
    }

    public record class M2_StockSearchCriteria {
        public string mapper_interface_name { get; set; } = string.Empty;
        public object[] criteria_list { get; set; }
        public object[] filters { get; set; }
        public object[] orders { get; set; }
        public string[] limit { get; set; }
    }

    public record class M2_ProductStock {
        public int item_id { get; set; }
        public int product_id { get; set; }
        public string sku { get; set; } = string.Empty;
        public int stock_id { get; set; }
        public int qty { get; set; }
        public bool is_in_stock { get; set; }
        public bool is_qty_decimal { get; set; }
        public bool show_default_notification_message { get; set; }
        public bool use_config_min_qty { get; set; }
        public int min_qty { get; set; }
        public int use_config_min_sale_qty { get; set; }
        public int min_sale_qty { get; set; }
        public bool use_config_max_sale_qty { get; set; }
        public int max_sale_qty { get; set; }
        public bool use_config_backorders { get; set; }
        public int backorders { get; set; }
        public bool use_config_notify_stock_qty { get; set; }
        public int notify_stock_qty { get; set; }
        public bool use_config_qty_increments { get; set; }
        public int qty_increments { get; set; }
        public bool use_config_enable_qty_inc { get; set; }
        public bool enable_qty_increments { get; set; }
        public bool use_config_manage_stock { get; set; }
        public bool manage_stock { get; set; }
        public string low_stock_date { get; set; } = string.Empty;
        public bool is_decimal_divided { get; set; }
        public int stock_status_changed_auto { get; set; }
    }
}