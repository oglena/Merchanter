using Merchanter.Classes;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Repositories {
    public interface IProductRepository {
        Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters);
        Task<int> GetProductsCount(int _customer_id, ApiFilter _filters);
        Task<Product?> GetProduct(int _customer_id, int _product_id);
    }

    public class ProductRepository(MerchanterService merchanterService) : IProductRepository {
        public async Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters) {
            return await GetProductsAsync(_customer_id, _filters);
        }
        public async Task<int> GetProductsCount(int _customer_id, ApiFilter _filters) {
            return await GetProductsCountAsync(_customer_id, _filters);
        }

        public async Task<Product?> GetProduct(int _customer_id, int _product_id) {
            return await GetProductAsync(_customer_id, _product_id);
        }

        private async Task<Product?> GetProductAsync(int _customer_id, int _product_id) {
            return await Task.Run(() => merchanterService.helper.GetProduct(_customer_id, _product_id));
        }

        private async Task<List<Product>> GetProductsAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetProducts(_customer_id, _filters));
        }

        private async Task<int> GetProductsCountAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetProductsCount(_customer_id, _filters));
        }
    }
}