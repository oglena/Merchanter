using MarketplaceHelpers;

namespace MerchanterHelpers.Responses {

    public class N11_ProductUpdate {
        public int id { get; set; }
        public N11.N11TaskType type { get; set; }
        public N11.N11_TaskStatus status { get; set; }
        public string[] reasons { get; set; }
    }
}
