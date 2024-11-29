using Merchanter.Classes;
using MerchanterApp.ApiService.Repositories;

namespace MerchanterApp.ApiService.Services {
	public interface IProductService {
		Task<List<Product>> GetProducts( int _customer_id );
		Task<Product?> GetProduct( int _customer_id, int _product_id );
	}

	public class ProductService( IProductRepository productRepository ) : IProductService {
		public Task<List<Product>> GetProducts( int _customer_id ) {
			return productRepository.GetProducts( _customer_id );
		}
		public Task<Product?> GetProduct( int _customer_id, int _product_id ) {
			return productRepository.GetProduct( _customer_id, _product_id );
		}
	}
}
