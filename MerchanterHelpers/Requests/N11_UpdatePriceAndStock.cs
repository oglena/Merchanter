namespace MerchanterHelpers.Requests {

    public class N11_UpdatePriceAndStock {
        public N11_Payload_Quick payload { get; set; }
    }

    public class N11_Payload_Quick {
        public string integrator { get; } = "MERCHANTER";
        public N11_Sku_Quick[] skus { get; set; }
    }

    public class N11_Sku_Quick {
        public string stockCode { get; set; }
        public float listPrice { get; set; }
        public float salePrice { get; set; }
        public int quantity { get; set; }
        public string currencyType { get; set; }
    }

}
