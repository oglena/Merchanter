using Merchanter.Classes;

namespace MerchanterFrontend.Classes {
    public class MerchanterServer {
        public int PID { get; set; }
        public int customer_id { get; set; }
        public Customer? customer { get; set; }
    }
}
