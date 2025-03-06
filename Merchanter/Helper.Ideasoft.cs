using Merchanter.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter {
    public static partial class Helper {
        public static bool UpdateIdeaProduct(IDEA_Product _product) {
            var pro_json = new {
                price1 = _product.price1,
                _product.marketPriceDetail,
                stockAmount = _product.stockAmount
            };

            using (Executioner executioner = new Executioner()) {
                var json_qty = executioner.Execute(Helper.global.ideasoft.store_url + "/admin-api/products/" + _product.id, RestSharp.Method.Put, pro_json, Helper.global.ideasoft.access_token);
                if (json_qty != null) {
                    return true;
                }
                return false;
            }
        }
    }
}
