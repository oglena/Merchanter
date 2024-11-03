namespace Merchanter.Classes.Settings {
    public class SettingsNetsis {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string? netopenx_user { get; set; }
        public string? netopenx_password { get; set; }
        public string? dbname { get; set; }
        public string? dbuser { get; set; }
        public string? dbpassword { get; set; }
        public string rest_url { get; set; } = @"https://";
        public string belgeonek_musterisiparisi { get; set; } = "C";
        public string siparis_carionek { get; set; } = "WEB-" + DateTime.Now.ToString( "yy" ) + "-";
        public string cari_siparis_grupkodu { get; set; } = "0=WEB|BANKA_TRANSFERİ=WEB-H";
        public string sipari_caritip { get; set; } = "A";
        public string siparis_muhasebekodu { get; set; } = "120-02-04";
        public string? siparis_cyedek1 { get; set; }
        public string? siparis_ekack4 { get; set; }
        public string? siparis_ekack15 { get; set; }
        public string? siparis_ekack10 { get; set; }
        public string? siparis_ekack11 { get; set; }
        public bool siparis_kdvdahilmi { get; set; } = false;
        public string? siparis_kod2 { get; set; }
        public int siparis_subekodu { get; set; } = 0;
        public int siparis_depokodu { get; set; } = 1;
        public string siparis_kargo_sku { get; set; } = "KARGO";
        public string siparis_taksitkomisyon_sku { get; set; } = "KMS-001";
        public bool is_rewrite_siparis { get; set; } = false;
        public string? ebelge_dizayn_earsiv { get; set; }
        public string? ebelge_dizayn_efatura { get; set; }
        public string ebelge_klasorismi { get; set; } = "NetsisFATURA";
        public string? efatura_belge_onek { get; set; }
        public string? earsiv_belge_onek { get; set; }
        public string fatura_cari_gruplari { get; set; } = "WEB,N11,HB,TY";
    }
}
