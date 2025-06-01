using Merchanter.Classes;
using MerchanterApp.ApiService.Repositories;

namespace MerchanterApp.ApiService.Services {
	public interface IProductService {
		Task<List<Product>> GetProducts( int _customer_id, ApiFilter _filters);
		Task<int> GetProductsCount( int _customer_id, ApiFilter _filters);
		Task<Product?> GetProduct( int _customer_id, int _product_id );
        Task<Product?> SaveProduct(int _customer_id, Product product);
        Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image);
        Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id);
        Task<ApiFilter?> GetProductsFilterProperties(int _customer_id, ApiFilter _filters);
    }

    public class ProductService( IProductRepository productRepository ) : IProductService {
		public Task<List<Product>> GetProducts( int _customer_id, ApiFilter _filters) {
			return productRepository.GetProducts( _customer_id, _filters );
		}

        public Task<ApiFilter?> GetProductsFilterProperties(int _customer_id, ApiFilter _filters) {
            return productRepository.GetProductsFilterProperties(_customer_id, _filters);
        }

        public Task<int> GetProductsCount( int _customer_id, ApiFilter _filters) {
			return productRepository.GetProductsCount( _customer_id, _filters );
		}

		public Task<Product?> GetProduct( int _customer_id, int _product_id ) {
			return productRepository.GetProduct( _customer_id, _product_id );
		}

        public Task<Product?> SaveProduct(int _customer_id, Product product) {
            if (product.id > 0) {
                return productRepository.UpdateProduct(_customer_id, product);
            }
            else {
                return productRepository.InsertProduct(_customer_id, product);
            }
            
        }

        public async Task<bool> DeleteProductImage(int _customer_id, ProductImage _product_image) {
            if (_product_image.id > 0) {
                return await productRepository.DeleteProductImage(_customer_id, _product_image);
            }
            return false;
        }

        public Task<List<ProductTarget>> GetProductTargets(int _customer_id, int _product_id) {
            return productRepository.GetProductTargets(_customer_id, _product_id);
        }
    }
}
