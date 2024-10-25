using n11_orderservice;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MarketplaceHelpers {
    public partial class N11 {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        private OrderServicePortClient N11_client { get; set; } = new OrderServicePortClient();
        private n11_orderservice.Authentication N11_auth { get; set; }
        private OrderServicePort N11_port;
        public int N11_hashcode { get; set; }

        public N11( string _appkey, string _appsecret ) {
            AppKey = _appkey;
            AppSecret = _appsecret;
            N11_auth = new Authentication() {
                appKey = _appkey,
                appSecret = _appsecret
            };

            N11_hashcode = N11_auth.GetHashCode();
            N11_port = N11_client.ChannelFactory.CreateChannel();
        }

        public DetailedOrderData[]? GetDetailedOrders( DateTime _start_date, DateTime _end_date, int _page_size = 100, int _current_page = 0 ) {
            DetailedOrderListRequest1 dol_req = new DetailedOrderListRequest1() {
                DetailedOrderListRequest = new DetailedOrderListRequest() {
                    auth = N11_auth,
                    searchData = new OrderDataListRequest {
                        period = new OrderSearchPeriod() {
                            startDate = _start_date.ToString( "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture ) + " 00:00",
                            endDate = _end_date.ToString( "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture ) + " 23:59"
                        }
                    },
                    pagingData = new PagingData() { pageSize = _page_size, currentPage = _current_page }
                }
            };

            var order_list_response = Task.Run( () => N11_port.DetailedOrderListAsync( dol_req ) ).Result;
            if( order_list_response.DetailedOrderListResponse.result.status == "success" ) {
                Debug.WriteLine( "GetDetailedOrders happened" );
                return order_list_response.DetailedOrderListResponse.orderList;
            }
            else
                return null;
        }

        public OrderData[]? GetOrders( DateTime _start_date, DateTime _end_date, int _page_size = 100, int _current_page = 0 ) {
            OrderListRequest1 dol_req = new OrderListRequest1() {
                OrderListRequest = new OrderListRequest() {
                    auth = N11_auth,
                    searchData = new OrderDataListRequest {
                        period = new OrderSearchPeriod() {
                            startDate = _start_date.ToString( "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture ) + " 00:00",
                            endDate = _end_date.ToString( "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture ) + " 23:59"
                        }
                    },
                    pagingData = new RequestPagingData() { pageSize = _page_size, currentPage = _current_page }
                }
            };

            var order_list_response = Task.Run( () => N11_port.OrderListAsync( dol_req ) ).Result;
            if( order_list_response.OrderListResponse.result.status == "success" ) {
                Debug.WriteLine( "GetOrders happened" );
                return order_list_response.OrderListResponse.orderList;
            }
            else {
                return null;
            }
        }
    }
}
