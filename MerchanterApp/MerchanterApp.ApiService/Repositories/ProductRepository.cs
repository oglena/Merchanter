using Merchanter.Classes;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Repositories {
    public interface IProductRepository {
        Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters);
        Task<Product?> GetProduct(int _customer_id, int _product_id);
    }

    public class ProductRepository(MerchanterService merchanterService) : IProductRepository {
        public async Task<List<Product>?> GetProducts(int _customer_id, ApiFilter _filters) {
            var merts = await GetProductsAsync(_customer_id, _filters);
            return merts;
        }

        public async Task<Product?> GetProduct(int _customer_id, int _product_id) {
            var mert = await GetProductAsync(_customer_id, _product_id);
            return mert;
        }

        private async Task<Product?> GetProductAsync(int _customer_id, int _product_id) {
            await Task.Run(() => merchanterService.global = merchanterService.helper.LoadSettings(_customer_id));
            return await Task.Run(() => merchanterService.helper.GetProduct(_customer_id, _product_id));
        }

        private async Task<List<Product>> GetProductsAsync(int _customer_id, ApiFilter _filters) {
            await Task.Run(() => merchanterService.global = merchanterService.helper.LoadSettings(_customer_id));
            return await Task.Run(() => merchanterService.helper.GetProducts(_customer_id, 20, 1, _filters));
        }
    }
}