using ApiService.Repositories;
using Merchanter.Classes;

namespace ApiService.Services {
    /// <summary>
    /// Interface for product-related operations.
    /// </summary>
    public interface IProductService {
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
        /// Saves a product (insert or update).
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="product">The product to save.</param>
        /// <returns>The saved product.</returns>
        Task<Product?> SaveProduct(int _customer_id, Product product);

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

    /// <summary>
    /// Service implementation for product-related operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </remarks>
    /// <param name="productRepository">The product repository.</param>
    public class ProductService(IProductRepository productRepository) : IProductService {
        /// <summary>
        /// The product repository used for data access operations.
        /// </summary>
        private readonly IProductRepository productRepository = productRepository;

        /// <summary>
        /// Retrieves a list of products based on customer ID and filters.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_filters">Filter criteria for the products.</param>
        /// <returns>A list of products.</returns>
        public Task<List<Product>> GetProducts(int _customer_id, ApiFilter _filters) {
            return productRepository.GetProducts(_customer_id, _filters);
        }

        /// <summary>
        /// Retrieves the count of products based on customer ID and filters.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_filters">Filter criteria for the products.</param>
        /// <returns>The count of products.</returns>
        public Task<int> GetProductsCount(int _customer_id, ApiFilter _filters) {
            return productRepository.GetProductsCount(_customer_id, _filters);
        }

        /// <summary>
        /// Retrieves a specific product by customer ID and product ID.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_product_id">The ID of the product.</param>
        /// <returns>The product, or null if not found.</returns>
        public Task<Product?> GetProduct(int _customer_id, int _product_id) {
            return productRepository.GetProduct(_customer_id, _product_id);
        }

        /// <summary>
        /// Saves a product (insert or update).
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="product">The product to save.</param>
        /// <returns>The saved product.</returns>
        public Task<Product?> SaveProduct(int _customer_id, Product product) {
            if (product.id > 0) {
                return productRepository.UpdateProduct(_customer_id, product);
            }
            else {
                return productRepository.InsertProduct(_customer_id, product);
            }
        }

        /// <summary>
        /// Deletes a product image.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_product_image">The product image to delete.</param>
        /// <returns>True if the image was deleted, false otherwise.</returns>
        public async Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image) {
            if (_product_image.id > 0) {
                return await productRepository.DeleteProductImage(_customer_id, _product_image);
            }
            return false;
        }

        /// <summary>
        /// Retrieves product targets based on customer ID and product ID.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_product_id">The ID of the product.</param>
        /// <returns>A list of product targets.</returns>
        public Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id) {
            return productRepository.GetProductTargets(_customer_id, _product_id);
        }
    }
}