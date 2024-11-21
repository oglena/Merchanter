namespace Merchanter.Classes {
    public class Platform {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; } = "no-logo.png";
        public List<PlatformType> available_types { get; set; } = new List<PlatformType>();
        public bool status { get; set; } = false;
        public Work.WorkType work_type { get; set; }
        public DateTime update_date { get; set; }

        public enum PlatformType {
            MAIN_SOURCE,
            SOURCE,
            MAIN_TARGET,
            TARGET,
            BOTH
        }

        public Platform( int _id, string _name, Work.WorkType _work_type, List<PlatformType> _available_types, bool _status, DateTime _update_date, string _image = "no-logo.png" ) {
            id = _id;
            name = _name;
            work_type = _work_type;
            available_types = _available_types;
            image = _image;
            status = _status;
            update_date = _update_date;
        }
    }
}
