using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Merchanter.Classes {
    public class Admin {
        public int id { get; set; }
        public string name { get; set; }

        [JsonIgnore]
        public string password { get; set; }
        public int type { get; set; }
    }
}
