namespace MerchanterHelpers.Requests {

    public class N11_Update {
        public N11_Payload payload { get; set; }
    }

    public class N11_Payload {
        public string integrator { get; } = "MERCHANTER";
        public N11_Sku[] skus { get; set; }
    }

    public class N11_Sku {
        public string stockCode { get; set; }
        public string status { get; set; }
        public int preparingDay { get; set; }
        public string shipmentTemplate { get; set; }
        public bool deleteProductMainId { get; set; }
        public string productMainId { get; set; }
        public bool deleteMaxPurchaseQuantity { get; set; }
        public int maxPurchaseQuantity { get; set; }
        public string description { get; set; }
    }

}
