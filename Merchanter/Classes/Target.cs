using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Merchanter.Classes {
    public class Target {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int target_id { get; set; }
        public string target_name { get; set; }
        public SyncStatus sync_status { get; set; } = SyncStatus.NotSynced;
        public DateTime update_date { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum SyncStatus {
            NotSynced = 0,
            Synced = 1,
            Syncing = 2,
            Error = 3
        }
    }
}
