using System.Text.Json.Serialization;

namespace Merchanter.Classes {
	public class Attribute {
		public int id { get; set; }
		public int customer_id { get; set; }
		public string attribute_name { get; set; }
		public string? attribute_title { get; set; } = null;
		public AttributeTypes type { get; set; }
		public DateTime update_date { get; set; }
	}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AttributeTypes {
		Text = 0,
		Price = 1,
		Image = 2,
		Bool = 3,
		Color = 4,
		Select = 5,
		MultiSelect = 6
	}
}
