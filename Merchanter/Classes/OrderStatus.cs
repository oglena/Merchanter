namespace Merchanter.Classes
{
    public class OrderStatus {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string status_name { get; set; }
        public string status_code { get; set; }
        public string magento2_status_code { get; set; }
        public bool sync_status { get; set; }
        public bool process_status { get; set; }

        public OrderStatus( int _id, int _customer_id, string _status_name, string _status_code, string _magento2_status_code, bool _sync_status, bool _process_status ) {
            id = _id;
            customer_id = _customer_id;
            status_name = _status_name;
            status_code = _status_code;
            magento2_status_code = _magento2_status_code;
            sync_status = _sync_status;
            process_status = _process_status;
        }

        public static string[] GetSyncEnabledCodes( string _source = "" ) {
            if( Helper.global.order_statuses == null ) return [];
            if( _source == Constants.MAGENTO2 )
                return Helper.global.order_statuses.Where( x => x.sync_status == true ).Select( x => x.magento2_status_code ).ToArray();
            else
                return Helper.global.order_statuses.Where( x => x.sync_status == true ).Select( x => x.status_code ).ToArray();
        }

        public static string[] GetProcessEnabledCodes(  string _source = "" ) {
            if( Helper.global.order_statuses == null ) return [];
            if( _source == Constants.MAGENTO2 )
                return Helper.global.order_statuses.Where( x => x.process_status == true ).Select( x => x.magento2_status_code ).ToArray();
            else
                return Helper.global.order_statuses.Where( x => x.process_status == true ).Select( x => x.status_code ).ToArray();
        }

        public static string GetStatusOf(  string _status, string _source = "" ) {
            if( Helper.global.order_statuses == null ) return string.Empty;
            if( _source == Constants.MAGENTO2 )
                return Helper.global.order_statuses.Where( x => x.magento2_status_code == _status ).First().status_code;
            else
                return Helper.global.order_statuses.Where( x => x.magento2_status_code == _status ).First().magento2_status_code;
        }
    }
}
