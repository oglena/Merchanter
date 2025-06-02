using MerchanterHelper.Requests;
using MerchanterHelper.Responses;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MerchanterHelper {
    public partial class N11 {
        private string AppKey { get; set; }
        private string AppSecret { get; set; }

        public N11(string _appkey, string _appsecret) {
            AppKey = _appkey;
            AppSecret = _appsecret;
        }

        #region Enums

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum N11_TaskStatus {
            IN_QUEUE,
            REJECT
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum N11TaskType {
            SKU_UPDATE,
            PRODUCT_UPDATE
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum N11_ProductStatus {
            Active,
            InCatalogApproval,
            Suspended,
            CatalogRejected,
            Unlisted,
            Prohibited,
            InApproval
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum N11_SaleStatus {
            Before_Sale,
            On_Sale,
            Out_Of_Stock,
            Sale_Closed
        }
        #endregion

        public N11_TaskDetail? GetTaskDetail(int taskId) {
            Executioner executioner = new Executioner();
            var request = new { taskId, pageable = new { page = 0, size = 1000 } };
            var task_detail = executioner.ExecuteN11("https://api.n11.com/ms/product/task-details/page-query", RestSharp.Method.Post, request, AppKey, AppSecret);
            if (task_detail == null) {
                Debug.WriteLine("N11 Task Detail is null");
                return null;
            }
            return JsonSerializer.Deserialize<N11_TaskDetail>(task_detail);
        }

        public N11_Products? GetProducts(long? _id = null, string? _productMainId = null, string? _stockCode = null, N11_SaleStatus? _saleStatus = null, N11_ProductStatus? _productStatus = null, string? _brandName = null, long[]? _categoryIds = null, int _page = 0, int _size = 20) {
            Executioner executioner = new Executioner();
            var n11_products = executioner.ExecuteN11($"https://api.n11.com/ms/product-query" +
                $"?id={_id}&productMainId={_productMainId}&stockCode={_stockCode}&saleStatus={_saleStatus}" +
                $"&productStatus={_productStatus}&brandName={_brandName}" +
                (_categoryIds?.Length > 0 ? $"&categoryIds={string.Join(",", _categoryIds)}" : string.Empty),
                RestSharp.Method.Get, null, AppKey, AppSecret);
            if (n11_products == null) {
                Debug.WriteLine("N11 Products is null");
                return null;
            }
            return JsonSerializer.Deserialize<N11_Products>(n11_products);
        }

        public N11_Categories? GetN11Categories() {
            Executioner executioner = new Executioner();
            var n11_categories = executioner.ExecuteN11("https://api.n11.com/cdn/categories", RestSharp.Method.Get, null, AppKey, null);
            if (n11_categories == null) {
                Debug.WriteLine("N11 Categories is null");
                return null;
            }
            return JsonSerializer.Deserialize<N11_Categories>(n11_categories);
        }

        public N11_ProductUpdate? UpdateProduct(N11_Update _request) {
            Executioner executioner = new Executioner();
            var task_detail = executioner.ExecuteN11("https://api.n11.com/ms/product/tasks/product-update", RestSharp.Method.Post, _request, AppKey, AppSecret);
            if (task_detail == null) {
                Debug.WriteLine("N11 Task Detail is null for UpdateProduct");
                return null;
            }
            return JsonSerializer.Deserialize<N11_ProductUpdate>(task_detail);
        }

        public N11_ProductUpdate? UpdateProductPriceAndStock(N11_UpdatePriceAndStock _request) {
            Executioner executioner = new Executioner();
            var task_detail = executioner.ExecuteN11("https://api.n11.com/ms/product/tasks/price-stock-update", RestSharp.Method.Post, _request, AppKey, AppSecret);
            if (task_detail == null) {
                Debug.WriteLine("N11 Task Detail is null for UpdateProductPriceAndStock");
                return null;
            }
            return JsonSerializer.Deserialize<N11_ProductUpdate>(task_detail);
        }
    }
}
