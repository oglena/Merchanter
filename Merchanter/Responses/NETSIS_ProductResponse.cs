namespace Merchanter.Responses {
    public record class NETSIS_ProductResponse {
        public NETSIS_Product[] products { get; set; }
    }

    public record class NETSIS_Product {
        public string Stok_Kodu { get; set; }
        public string Stok_Adi { get; set; }
        public string Grup_Kodu { get; set; }
        public string Kod_1 { get; set; }
        public string Kod_2 { get; set; }
        public string Kod_3 { get; set; }
        public string Kod_4 { get; set; }
        public string Kod_5 { get; set; }
        public int Sat_Dov_Tip { get; set; }
        public string Barkod1 { get; set; }
        public string Satici_Kodu { get; set; }
        public double? KDV_Orani { get; set; }
        public int MERKEZ { get; set; }
        public int SIPARIS { get; set; }
        public int BAKIYE { get; set; }
        public bool KULL5N { get; set; }
    }
}
