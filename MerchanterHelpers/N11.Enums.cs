namespace MarketplaceHelpers {
    public partial class N11 {

        public enum OrderList_Order_OrderStatuses {
            İşlem_Bekliyor = 1,
            İşlemde = 2,
            İptal_Edilmiş = 3,
            Geçersiz = 4,
            Tamamlandı = 5
        }

        public enum OrderList_Order_PaymentTypes {
            Kredi_Kartı = 1,
            BKMEXPRESS = 2,
            AKBANKDIREKT = 3,
            PAYPAL = 4,
            MallPoint = 5,
            GARANTIPAY = 6,
            GarantiLoan = 7,
            MasterPass = 8,
            ISBANKPAY = 9,
            PAYCELL = 10,
            COMPAY = 11,
            YKBPAY = 12,
            FIBABANK = 13,
            Other = 14
        }

        public enum OrderList_Order_OrderItemList_OrderItem_DeliveryFeeTypes {
            N11_Öder = 1,
            Alıcı_Öder = 2,
            Mağaza_Öder = 3,
            Şartlı_Kargo_Alıcı_Öder = 4,
            Şartlı_Kargo_Ücretsiz_Satıcı_Öder = 5,
        }

        public enum OrderList_Order_OrderItemList_OrderItem_OrderItemStatuses {
            İşlem_Bekliyor = 1,
            Ödendi = 2,
            Geçersiz = 3,
            İptal_Edilmiş = 4,
            Kabul_Edilmiş = 5,
            Kargoda = 6,
            Teslim_Edilmiş = 7,
            Reddedilmiş = 8,
            İade_Edildi = 9,
            Tamamlandı = 10,
            İade_İptal_Değişim_Talep_Edildi = 11,
            İade_İptal_Değişim_Tamamlandı = 12,
            Kargoda_İade = 13,
            Kargo_Yapılması_Gecikmiş = 14,
            Kabul_Edilmiş_Ama_Zamanında_Kargoya_Verilmemiş = 15,
            Teslim_Edilmiş_İade = 16,
            Tamamlandıktan_Sonra_İade = 17
        }

        public enum OrderDetail_PaymentTypes {
            Kredi_Kartı = 1,
            BKMEXPRESS = 2,
            AKBANKDIREKT = 3,
            PAYPAL = 4,
            MallPoint = 5,
            GARANTIPAY = 6,
            GarantiLoan = 7,
            MasterPass = 8,
            ISBANKPAY = 9,
            PAYCELL = 10,
            COMPAY = 11,
            YKBPAY = 12,
            Other = 13,
        }

        public enum OrderDetail_ServiceItemList_ServiceItem_OrderItemTypes {
            Ürün_Sipariş_Kalemi = 1,
            Servis_Sipariş_Kalemi = 2,
            Gsm_Sipariş_Kalemi = 3,
            Gezi_Sipariş_Kalemi = 4
        }
    }
}
