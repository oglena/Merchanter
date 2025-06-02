namespace Merchanter.Classes {
	public record class AttributeOption {
		public int id { get; set; }
		public int customer_id { get; set; }
		public int attribute_id { get; set; }
		public string option_name { get; set; }
		public string option_value { get; set; }
	}
}
