using Merchanter.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter {
    public static partial class Helper {
        public static bool UpdateIdeaProduct(int _id, decimal _price1, float _qty) {
            var pro_json = new {
                price1= _price1, //_product.price1,
                marketPriceDetail = _price1.ToString().Replace(".", string.Empty).Replace(",", "."), // _product.marketPriceDetail,
                stockAmount = _qty, //_product.stockAmount,
            };

            using Executioner executioner = new();
            var json_qty = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/products/" + _id.ToString(), RestSharp.Method.Put, pro_json, Helper.global.ideasoft.access_token);
            if (json_qty != null) {
                return true;
            }
            return false;
        }

        public static List<IDEA_Product>? GetIdeaProducts(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Product> products = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url +
                "/admin-api/products?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Product>>(json);
                if (temp_products != null) {
                    if (temp_products.Count == _limit) {
                        products.AddRange(temp_products);
                        page++;
                        goto QUERY;
                    }
                    else {
                        products.AddRange(temp_products);
                        return products;
                    }
                }
            }
            return null;
        }

        public static List<IDEA_Category>? GetIdeaCategories(int _limit = 100) {
            using Executioner executioner = new();
            List<IDEA_Category> categories = []; int page = 1;
        QUERY:
            var json = executioner.Execute(Helper.global.ideasoft.store_url +
                "/admin-api/categories?limit=" + _limit.ToString() + "&page=" + page.ToString(),
                RestSharp.Method.Get, null, Helper.global.ideasoft.access_token);
            if (json != null) {
                var temp_categories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDEA_Category>>(json);
                if (temp_categories != null) {
                    if (temp_categories.Count == _limit) {
                        categories.AddRange(temp_categories);
                        page++;
                        goto QUERY;
                    }
                    else {
                        categories.AddRange(temp_categories);
                        return categories;
                    }
                }
            }
            return null;
        }
    }
}
