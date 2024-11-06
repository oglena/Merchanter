using System.Diagnostics;

namespace Merchanter.Classes {
    public class Log {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string? worker { get; set; } = null;
        public string title { get; set; }
        public string message { get; set; }
        public string? thread_id { get; set; } = null;
        public DateTime update_date { get; set; }
    }
}
