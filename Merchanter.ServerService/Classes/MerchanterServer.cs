using Merchanter.Classes;
using Merchanter.ServerService.Models;

namespace Merchanter.ServerService.Classes {
    public class MerchanterServer {
        public int PID { get; set; }
        public int customer_id { get; set; }
        public Customer? customer { get; set; }
    }
}
