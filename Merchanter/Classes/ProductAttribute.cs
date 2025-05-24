namespace Merchanter.Classes {
	public class ProductAttribute {
		public int id { get; set; }
		public int customer_id { get; set; }
		public int product_id { get; set; }
		public string? sku { get; set; } = null;
        public int attribute_id { get; set; }
		public Attribute attribute { get; set; }
		public AttributeTypes type { get; set; }
		public string? value { get; set; } = null;
		public string? option_ids { get; set; } = null;
		public List<AttributeOption>? options { get; set; } = null;
		public DateTime update_date { get; set; } = DateTime.Now;
    }
}
