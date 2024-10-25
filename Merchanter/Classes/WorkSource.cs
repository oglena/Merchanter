namespace Merchanter.Classes {
    public class WorkSource {
        public WorkSource() {

        }
        public WorkSource( int _id, int _customer_id, string _name, string _type, string _direction, bool _is_active ) {
            id = _id;
            customer_id = _customer_id;
            name = _name;
            type = _type;
            direction = _direction;
            is_active = _is_active;
        }
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string direction { get; set; } = string.Empty;
        public bool is_active { get; set; }
    }
}
