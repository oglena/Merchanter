﻿using System.Diagnostics;

namespace Merchanter.Classes {
    public class Log
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string worker { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public DateTime? update_date { get; set; }
    }
}
