namespace Merchanter.Classes {
    public class Platform {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; } = "no-logo.png";
        public List<PlatformType> type { get; set; } = new List<PlatformType>();
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

        public Platform( int _id, string _name, Work.WorkType _work_type, List<PlatformType> _type, bool _status, DateTime _update_date, string _image = "no-logo.png" ) {
            id = _id;
            name = _name;
            work_type = _work_type;
            type = _type;
            image = _image;
            status = _status;
            update_date = _update_date;
        }

        public static List<Platform> Dummies() {
            return new List<Platform>() {
                /*0*/ new Platform( 1, Constants.ENTEGRA, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.MAIN_SOURCE, PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00"), "ENTEGRA.png" ),
                /*1*/ new Platform( 2, Constants.NETSIS, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.MAIN_SOURCE, PlatformType.SOURCE  }, true, Convert.ToDateTime("12.11.2024 00:00"), "NETSIS.png" ),
                /*2*/ new Platform( 3, Constants.NETSIS, Work.WorkType.ORDER, new List<PlatformType> { PlatformType.MAIN_TARGET }, true, Convert.ToDateTime("12.11.2024 00:00"), "NETSIS.png" ),
                /*3*/ new Platform( 4, Constants.MAGENTO2, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.TARGET }, true, Convert.ToDateTime("12.11.2024 00:00"), "MAGENTO.png" ),
                /*4*/ new Platform( 5, Constants.MAGENTO2, Work.WorkType.ORDER, new List<PlatformType> { PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00"), "MAGENTO.png" ),
                /*5*/ new Platform( 6, Constants.MERCHANTER, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.MAIN_SOURCE, PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00") ),
                /*6*/ new Platform( 7, Constants.YURTICI, Work.WorkType.SHIPMENT, new List<PlatformType> { PlatformType.BOTH }, true, Convert.ToDateTime("12.11.2024 00:00"), "YK.png" ),
                /*7*/ new Platform( 8, Constants.MNG, Work.WorkType.SHIPMENT, new List<PlatformType> { PlatformType.BOTH }, false, Convert.ToDateTime("12.11.2024 00:00"), "MNG.png" ),
                /*8*/ new Platform( 9, Constants.ARAS, Work.WorkType.SHIPMENT, new List<PlatformType> { PlatformType.BOTH }, false, Convert.ToDateTime("12.11.2024 00:00"), "ARAS.png" ),
                /*9*/ new Platform( 10, Constants.PENTA, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00") ),
                /*10*/ new Platform( 11, Constants.KOYUNCU, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00") ),
                /*11*/ new Platform( 12, Constants.BOGAZICI, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00") ),
                /*12*/ new Platform( 13, Constants.OKSID, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00") ),
                /*13*/ new Platform( 14, Constants.FSP, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.SOURCE }, true, Convert.ToDateTime("12.11.2024 00:00") ),
                /*14*/ new Platform( 15, Constants.MERCHANTER, Work.WorkType.ORDER, new List<PlatformType> { PlatformType.MAIN_TARGET }, false, Convert.ToDateTime("12.11.2024 00:00") ),
                /*15*/ new Platform( 16, Constants.NETSIS, Work.WorkType.PRODUCT, new List<PlatformType> { PlatformType.MAIN_SOURCE }, false, Convert.ToDateTime("12.11.2024 00:00"), "NETSIS.png" ),
            };
        }
    }
}
