using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public class Admin {
        public int id { get; set; }
        public string name { get; set; }

        [JsonIgnore]
        public string password { get; set; }
        public int type { get; set; }
    }
}
