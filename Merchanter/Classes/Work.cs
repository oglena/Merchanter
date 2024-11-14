namespace Merchanter.Classes {
    public class Work {
        public int id { get; set; }
        public Platform platform { get; set; }
        public WorkType type { get; set; }
        public WorkDirection direction { get; set; }
        public bool status { get; set; } = false;
        public string version { get; set; } = "v0";

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

        public Work( int _id, Platform _platform, WorkType _type, WorkDirection _direction, bool _status, string _version ) {
            id = _id;
            platform = _platform;
            type = _type;
            direction = _direction;
            status = _status;
            version = _version;
        }

        public static List<Work> Works() {
            return new List<Work>() {
                new Work( 1, Platform.Dummies()[5], WorkType.PRODUCT, WorkDirection.MAIN_SOURCE, false, "v1" ), // MERCHANTER
                new Work( 4, Platform.Dummies()[0], WorkType.PRODUCT, WorkDirection.MAIN_SOURCE, true, "v1" ), // ENTEGRA
                new Work( 5, Platform.Dummies()[1], WorkType.PRODUCT, WorkDirection.SOURCE, false, "v1" ), // NETSIS
                new Work( 6, Platform.Dummies()[4], WorkType.ORDER, WorkDirection.SOURCE, true, "v1" ), // MAGENTO2
                new Work( 7, Platform.Dummies()[9], WorkType.PRODUCT, WorkDirection.SOURCE, true, "v1" ), // PENTA
                new Work( 8, Platform.Dummies()[10], WorkType.PRODUCT, WorkDirection.SOURCE, true, "v1" ), // KOYUNCU
                new Work( 9, Platform.Dummies()[11], WorkType.PRODUCT, WorkDirection.SOURCE, true, "v1" ), // BOGAZICI
                new Work( 10, Platform.Dummies()[12], WorkType.PRODUCT, WorkDirection.SOURCE, true, "v1" ), // OKSID
                new Work( 11, Platform.Dummies()[13], WorkType.PRODUCT, WorkDirection.SOURCE, true, "v1" ), // FSP
                new Work( 12, Platform.Dummies()[2], WorkType.ORDER, WorkDirection.MAIN_TARGET, true, "v1" ), // NETSIS
                new Work( 13, Platform.Dummies()[3], WorkType.PRODUCT, WorkDirection.TARGET, true, "v1" ), // MAGENTO2
                new Work( 14, Platform.Dummies()[6], WorkType.SHIPMENT, WorkDirection.BOTH, true, "v1" ), // YURTİÇİ_KARGO
                new Work( 15, Platform.Dummies()[7], WorkType.SHIPMENT, WorkDirection.BOTH, false, "v1" ), // MNG_KARGO
                new Work( 16, Platform.Dummies()[8], WorkType.SHIPMENT, WorkDirection.BOTH, false, "v1" ), // ARAS_KARGO
                new Work( 18, Platform.Dummies()[14], WorkType.ORDER, WorkDirection.MAIN_TARGET, false, "v1" ), // MERCHANTER
                new Work( 19, Platform.Dummies()[15], WorkType.PRODUCT, WorkDirection.MAIN_SOURCE, false, "v1" ), // NETSIS
                //TODO: ID is provided by the database and must be added to the list
            };
        }
    }
}
