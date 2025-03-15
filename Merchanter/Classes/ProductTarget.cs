using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter.Classes
{
    public class ProductTarget {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public int target_id { get; set; }
        public string target_name { get; set; }
        public DateTime update_date { get; set; }
    }
}
