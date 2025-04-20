using Merchanter.Classes;
using MerchanterApp.ApiService.Repositories;

namespace MerchanterApp.ApiService.Services {
	public interface IProductService {
		Task<List<Product>> GetProducts( int _customer_id, ApiFilter _filters);
		Task<int> GetProductsCount( int _customer_id, ApiFilter _filters);
		Task<Product?> GetProduct( int _customer_id, int _product_id );
	}

	public class ProductService( IProductRepository productRepository ) : IProductService {
		public Task<List<Product>> GetProducts( int _customer_id, ApiFilter _filters) {
			return productRepository.GetProducts( _customer_id, _filters );
		}
		public Task<int> GetProductsCount( int _customer_id, ApiFilter _filters) {
			return productRepository.GetProductsCount( _customer_id, _filters );
		}

		public Task<Product?> GetProduct( int _customer_id, int _product_id ) {
			return productRepository.GetProduct( _customer_id, _product_id );
		}
	}
}
