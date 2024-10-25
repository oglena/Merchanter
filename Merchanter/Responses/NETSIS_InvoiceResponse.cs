namespace Merchanter.Responses {
    public class NETSIS_InvoiceResponse {
        public string FATURANO { get; set; }
        public string SIPARISNO { get; set; }
        public string GIB_FATIRS_NO { get; set; }
        public string CARIKODU { get; set; }
        public string? CARIGRUP { get; set; }
        public decimal GENELTOPLAM { get; set; }
        public decimal BRUTTUTAR { get; set; }
        public decimal KDV { get; set; }
        public int FATKALEM_ADEDI { get; set; }
        public bool KDV_DAHILMI { get; set; }
        public string? EKACK1 { get; set; } = null;
        public string? EKACK2 { get; set; } = null;
        public DateTime? TARIH { get; set; }
        public List<NETSIS_InvoiceItemResponse> KALEMS { get; set; }
    }
}
