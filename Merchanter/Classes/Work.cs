using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public class Work {
        public int id { get; set; }
        public Platform platform { get; set; }
		public string name { get; set; }
		public WorkType type { get; set; }
        public WorkDirection direction { get; set; }
        public bool status { get; set; } = false;
        public string version { get; set; } = "v1";

        public enum WorkType {
            PRODUCT,
            ORDER,
            SHIPMENT
        }

        public enum WorkDirection {
            /// <summary>
            /// Only 1 accepted
            /// </summary>
            MAIN_SOURCE,
            SOURCE,
            /// <summary>
            /// Only 1 accepted
            /// </summary>
            MAIN_TARGET,
            TARGET,
            BOTH
        }

        public Work( int _id, Platform _platform,string _name, WorkType _type, WorkDirection _direction, bool _status, string _version ) {
            id = _id;
            platform = _platform;
			name = _name;
			type = _type;
            direction = _direction;
            status = _status;
            version = _version;
        }

        [JsonConstructor]
        public Work() { }
    }
}
