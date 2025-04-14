namespace Merchanter.Classes {
    public class ProductImage {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public string? sku { get; set; } = null;
        public ImageTypes type { get; set; } = ImageTypes.url;
        public string? image_name { get; set; } = null;
        public string? image_url { get; set; } = null;
        public string? image_base64 { get; set; } = null;
        public bool is_default { get; set; } = false;
        public DateTime update_date { get; set; }
    }

    public enum ImageTypes {
        url = 0,
        base64 = 1,
    }
}
