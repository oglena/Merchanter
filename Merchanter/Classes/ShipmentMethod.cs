namespace Merchanter.Classes
{
    public class ShipmentMethod {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string shipment_name { get; set; }
        public string shipment_code { get; set; }
        public string magento2_shipment_code { get; set; }

        public ShipmentMethod( int _id, int _customer_id, string _shipment_name, string _shipment_code, string _magento2_shipment_code ) {
            id = _id;
            customer_id = _customer_id;
            shipment_name = _shipment_name;
            shipment_code = _shipment_code;
            magento2_shipment_code = _magento2_shipment_code;
        }

        public static string GetShipmentMethodOf(  string _code, string _source = "" ) {
            if( Helper.global.shipment_methods == null ) return string.Empty;
            if( _source == Constants.MAGENTO2 )
                return Helper.global.shipment_methods.Where( x => x.magento2_shipment_code == _code ).First().shipment_code;
            else
                return Helper.global.shipment_methods.Where( x => x.magento2_shipment_code == _code ).First().magento2_shipment_code;
        }

        public static string GetShipmentName(  string _code ) {
            return Helper.global.shipment_methods.Where( x => x.shipment_code == _code ).First().shipment_name;
        }
    }
}
