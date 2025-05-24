using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public class OrderStatus {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string status_name { get; set; }
        public string status_code { get; set; }
        public string platform { get; set; }
        public string platform_status_code { get; set; }
        public bool sync_status { get; set; } = false;
        public bool process_status { get; set; } = false;

        public OrderStatus(int _id, int _customer_id, string _status_name, string _status_code, string _platform, string _platform_status_code, bool _sync_status, bool _process_status) {
            id = _id;
            customer_id = _customer_id;
            status_name = _status_name;
            status_code = _status_code;
            platform = _platform;
            platform_status_code = _platform_status_code;
            sync_status = _sync_status;
            process_status = _process_status;
        }

        [JsonConstructor]
        public OrderStatus() { }

        public static string[] GetSyncEnabledCodes(string _platform = "") {
            if (Helper.global.order_statuses == null) return [];
            return [.. Helper.global.order_statuses.Where(x => x.sync_status == true && x.platform == _platform).Select(x => x.platform_status_code)];
        }

        public static string[] GetProcessEnabledCodes(string _platform = "") {
            if (Helper.global.order_statuses == null) return [];
            return [.. Helper.global.order_statuses.Where(x => x.process_status == true && x.platform == _platform).Select(x => x.status_code)];
        }

        public static string[] GetProcessEnabledCodes(string[] _platforms) {
            if (Helper.global.order_statuses == null) return [];
            return [.. Helper.global.order_statuses.Where(x => x.process_status == true && _platforms.Contains(x.platform)).Select(x => x.status_code)];
        }

        public static string GetStatusOf(string _status, string _platform = "") {
            if (Helper.global.order_statuses == null) return string.Empty;
            return Helper.global.order_statuses.Where(x => x.platform == _platform && x.platform_status_code == _status).First().status_code;
        }
    }
}
