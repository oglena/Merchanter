using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter.Classes {
    public class SyncMapping {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string attribute_source { get; set; }
        public string attribute_type { get; set; }
        public string variable_type { get; set; }
        public string product_attribute { get; set; }
        public string work_source { get; set; }
        public string source_attribute { get; set; }
        public string regex { get; set; }
        public bool is_active { get; set; }
        public DateTime? update_date { get; set; }
    }
}
