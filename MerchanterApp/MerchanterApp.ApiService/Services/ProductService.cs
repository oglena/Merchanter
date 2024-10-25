using Merchanter.Classes;
using MerchanterApp.ApiService.Repositories;

namespace MerchanterApp.ApiService.Services {
    public interface IProductService {
        Task<List<Product>?> GetProducts( int _customer_id );
    }

    public class ProductService( IProductRepository productRepository ) :IProductService {
        public Task<List<Product>?> GetProducts( int _customer_id ) {
            return productRepository.GetProducts( _customer_id );
        }
    }
}
