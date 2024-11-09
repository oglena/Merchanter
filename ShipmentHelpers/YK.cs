using yurticikargo_service;

namespace ShipmentHelpers {
    public class YK {
        private string user_name { get; set; }
        private string password { get; set; }
        public static string lang = "TR";

        public YK( string _user_name, string _password ) {
            user_name = _user_name;
            password = _password;
        }

        /// <summary>
        /// Yurtiçi Kargo insert shipment
        /// </summary>
        /// <param name="_order_source">Order Source</param>
        /// <param name="_order_label">Order Label</param>
        /// <param name="_customer_name">Customer Name</param>
        /// <param name="_receiver_address">Receiver Address</param>
        /// <param name="_receiver_phone">Receiver Phone</param>
        /// <param name="_city_name">City Name</param>
        /// <param name="_town_name">Town Name</param>
        /// <returns>Shipment Barcode</returns>
        public string? InsertShipment( string _order_source, string _order_label, string _customer_name, string _receiver_address, string _receiver_phone, string _city_name, string? _town_name ) {
            try {
                string barcode = (_order_source == "MAGENTO2" ? "7" : "5") + DateTime.Now.ToString( "yy" ) + DateTime.Now.ToString( "ddMM" ) + _order_label;
                createShipment yk_shipment = new createShipment();
                yk_shipment.wsUserName = user_name;
                yk_shipment.wsPassword = password;
                yk_shipment.userLanguage = lang;
                yk_shipment.ShippingOrderVO = [ new ShippingOrderVO() {
                    cargoKey = barcode,
                    invoiceKey = _order_label,
                    receiverCustName = _customer_name,
                    receiverAddress = _receiver_address,
                    receiverPhone1 = _receiver_phone,
                    cityName = _city_name,
                    townName = !string.IsNullOrWhiteSpace(_town_name) ? _town_name : _city_name,
                    specialField1 = "1$" + _order_source + " Barkod:#2$" + barcode + "#"
               }
                ];
                yurticikargo_service.ShippingOrderDispatcherServicesClient client = new ShippingOrderDispatcherServicesClient();
                createShipmentResponse response = client.createShipment( yk_shipment );
                if( response.ShippingOrderResultVO.errCode == null ) {
                    return barcode;
                }
                else {
                    return null;
                }
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Yurtiçi Kargo get shipment
        /// </summary>
        /// <param name="_shipment_barcode">Shipment Barcode</param>
        /// <returns>Tracking Numbers</returns>
        public List<string>? GetShipment( string _shipment_barcode ) {
            try {
                queryShipment yk_query_shipment = new queryShipment();
                yk_query_shipment.wsUserName = user_name;
                yk_query_shipment.wsPassword = password;
                yk_query_shipment.wsLanguage = lang;
                yk_query_shipment.keyType = 0;
                yk_query_shipment.keys = [ _shipment_barcode ];
                List<string> tracking_numers = [];
                yurticikargo_service.ShippingOrderDispatcherServicesClient client = new ShippingOrderDispatcherServicesClient();
                queryShipmentResponse response = client.queryShipment( yk_query_shipment );
                if( response.ShippingDeliveryVO.errCode == null ) {
                    foreach( var shipment_item in response.ShippingDeliveryVO.shippingDeliveryDetailVO ) {
                        if( shipment_item.operationStatus != "NOP" && shipment_item.operationStatus == "IND" && shipment_item.errCode == 0 ) {
                            tracking_numers.Add( shipment_item.shippingDeliveryItemDetailVO.docId );
                        }
                    }
                }

                return tracking_numers;
            }
            catch( Exception _ex ) {
                return null;
            }
        }
    }
}
