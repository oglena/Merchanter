namespace Merchanter.Classes.Settings {
    public class SettingsMagento {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string base_url { get; set; } = @"https://";
        public string? token { get; set; }
        public int root_category_id { get; set; } = 1;
        public string brand_attribute_code { get; set; } = "brand";
        public string barcode_attribute_code { get; set; } = "barcode";
        public string order_processing_comment { get; set; } = "Siparişiniz hazırlanıyor.";
        public string is_xml_enabled_attribute_code { get; set; } = "xml_from_dists";
        public string xml_sources_attribute_code { get; set; } = "xml_sources";
        public string customer_tc_no_attribute_code { get; set; } = "tc_no";
        public string customer_firma_ismi_attribute_code { get; set; } = "firma_ismi";
        public string customer_firma_vergidairesi_attribute_code { get; set; } = "firma_vergidairesi";
        public string customer_firma_vergino_attribute_code { get; set; } = "firma_vergino";
    }
}
