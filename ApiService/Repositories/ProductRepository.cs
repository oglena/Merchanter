using Merchanter.Classes;
using ApiService.Services;

namespace ApiService.Repositories {
    /// <summary>
    /// Interface for product-related data access operations.
    /// </summary>
    public interface IProductRepository {
        /// <summary>
        /// Retrieves a list of products based on customer ID and filters.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_filters">Filter criteria for the products.</param>
        /// <returns>A list of products.</returns>
        Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Retrieves the count of products based on customer ID and filters.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_filters">Filter criteria for the products.</param>
        /// <returns>The count of products.</returns>
        Task<int> GetProductsCount(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Retrieves a specific product by customer ID and product ID.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_product_id">The ID of the product.</param>
        /// <returns>The product, or null if not found.</returns>
        Task<Product?> GetProduct(int _customer_id, int _product_id);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="product">The product to update.</param>
        /// <returns>The updated product.</returns>
        Task<Product?> UpdateProduct(int _customer_id, Product product);

        /// <summary>
        /// Inserts a new product.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="product">The product to insert.</param>
        /// <returns>The inserted product.</returns>
        Task<Product?> InsertProduct(int _customer_id, Product product);

        /// <summary>
        /// Deletes a product image.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_product_image">The product image to delete.</param>
        /// <returns>True if the image was deleted, false otherwise.</returns>
        Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image);

        /// <summary>
        /// Retrieves product targets based on customer ID and product ID.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_product_id">The ID of the product.</param>
        /// <returns>A list of product targets.</returns>
        Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id);
    }

    /// <inheritdoc />
    public class ProductRepository(MerchanterService merchanterService) : IProductRepository {
        /// <inheritdoc />
        public async Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters) {
            return await GetProductsAsync(_customer_id, _filters);
        }

        /// <inheritdoc />
        public async Task<int> GetProductsCount(int _customer_id, ApiFilter _filters) {
            return await GetProductsCountAsync(_customer_id, _filters);
        }

        /// <inheritdoc />
        public async Task<Product?> GetProduct(int _customer_id, int _product_id) {
            return await GetProductAsync(_customer_id, _product_id);
        }

        /// <inheritdoc />
        private async Task<Product?> GetProductAsync(int _customer_id, int _product_id) {
            return await Task.Run(() => merchanterService.helper.GetProduct(_customer_id, _product_id));
        }

        /// <inheritdoc />
        private async Task<List<Product>> GetProductsAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetProducts(_customer_id, _filters));
        }

        /// <inheritdoc />
        private async Task<int> GetProductsCountAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetProductsCount(_customer_id, _filters));
        }

        /// <inheritdoc />
        public async Task<Product?> UpdateProduct(int _customer_id, Product product) {
            return await Task.Run(() => merchanterService.helper.UpdateProduct(_customer_id, product));
        }

        /// <inheritdoc />
        public async Task<Product?> InsertProduct(int _customer_id, Product product) {
            return await Task.Run(() => merchanterService.helper.InsertProduct(_customer_id, product));
        }

        /// <inheritdoc />
        public async Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image) {
            return await Task.Run(() => merchanterService.helper.DeleteProductImage(_customer_id, _product_image));
        }

        /// <inheritdoc />
        public async Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id) {
            return await Task.Run(() => merchanterService.helper.GetProductTargets(_customer_id, _product_id));
        }
    }
}