namespace Merchanter.Classes {
    public class Integration {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string direction { get; set; }
        public bool is_active { get; set; }
        public Work? work { get; set; } = null;

        public Integration( int _id, int _customer_id, string _name, string _type, string _direction, bool _is_active /*, Work? _work*/ ) { //TODO: Work dummies change
            id = _id;
            customer_id = _customer_id;
            name = _name;
            type = _type;
            direction = _direction;
            is_active = _is_active;
            //work = _work;
            work = Work.Works().Find( w => w.id == _id );
        }
    }
}
