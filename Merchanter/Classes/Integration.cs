using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    public class Integration {
        public int id { get; set; }
        public int customer_id { get; set; }
        public bool is_active { get; set; } = false;
		public Work work { get; set; }

        public Integration( int _id, int _customer_id,bool _is_active, Work _work ) { 
            id = _id;
            customer_id = _customer_id;
			is_active = _is_active;
			work = _work;
        }

        [JsonConstructor]
        public Integration() { }
    }
}
