namespace Merchanter.Classes
{
    public class PaymentMethod {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string payment_name { get; set; }
        public string payment_code { get; set; }
        public string magento2_payment_code { get; set; }

        public PaymentMethod( int _id, int _customer_id, string _payment_name, string _payment_code, string _magento2_payment_code ) {
            id = _id;
            customer_id = _customer_id;
            payment_name = _payment_name;
            payment_code = _payment_code;
            magento2_payment_code = _magento2_payment_code;
        }

        public static string GetPaymentMethodOf( string _code, string _source = "" ) {
            if( Helper.global.payment_methods == null ) return string.Empty;
            if( _source == Constants.MAGENTO2 )
                return Helper.global.payment_methods.Where( x => x.magento2_payment_code == _code ).First().payment_code;
            else
                return Helper.global.payment_methods.Where( x => x.magento2_payment_code == _code ).First().magento2_payment_code;
        }
    }
}
