namespace Merchanter.Responses {
    public class NETSIS_InvoiceItemResponse {
        public string FATURANO { get; set; }
        public string SIPARISNO { get; set; }
        public string STOKKODU { get; set; }
        public int MIKTAR { get; set; }
        public decimal FIYAT { get; set; }
        public int KDV_ORAN { get; set; }
        public List<string> SERILER { get; set; }
        public DateTime? TARIH { get; set; }
    }
}
