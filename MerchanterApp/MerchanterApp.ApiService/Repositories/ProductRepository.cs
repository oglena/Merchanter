using Merchanter.Classes;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Repositories {
    public interface IProductRepository {
        Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters);
        Task<int> GetProductsCount(int _customer_id, ApiFilter _filters);
        Task<Product?> GetProduct(int _customer_id, int _product_id);
        Task<Product?> UpdateProduct(int _customer_id, Product product);
        Task<Product?> InsertProduct(int _customer_id, Product product);
        Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image);
        Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id);
        Task<ApiFilter?> GetProductsFilterProperties(int _customer_id, ApiFilter _filters);
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

        public async Task<ApiFilter?> GetProductsFilterProperties(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetProductsFilterProperties(_customer_id, _filters));
        }

        private async Task<int> GetProductsCountAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetProductsCount(_customer_id, _filters));
        }

        public async Task<Product?> UpdateProduct(int _customer_id, Product product) {
            return await Task.Run(() => merchanterService.helper.UpdateProduct(_customer_id, product));
        }

        public async Task<Product?> InsertProduct(int _customer_id, Product product) {
            return await Task.Run(() => merchanterService.helper.InsertProduct(_customer_id, product));
        }

        public async Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image) {
            return await Task.Run(() => merchanterService.helper.DeleteProductImage(_customer_id, _product_image));
        }

        public async Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id) {
            return await Task.Run(() => merchanterService.helper.GetProductTargets(_customer_id, _product_id));
        }
    }
}