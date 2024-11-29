using Merchanter;
using Merchanter.Classes;
using Merchanter.Classes.Settings;
using MerchanterApp.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MerchanterApp.ApiService.Repositories {
    public interface IProductRepository {
        Task<List<Product>> GetProducts( int _customer_id );
        Task<Product?> GetProduct( int _customer_id, int _product_id );
	}
    public class ProductRepository( MerchanterService merchanterService ) :IProductRepository {
        public async Task<List<Product>?> GetProducts( int _customer_id ) {
            var mert = await GetProductsAsync( _customer_id );
            return mert;
		}
		public async Task<Product?> GetProduct( int _customer_id,  int _product_id ) {
			var mert = await GetProductAsync( _customer_id, _product_id );
			return mert;
		}

		private async Task<Product?> GetProductAsync( int _customer_id, int _product_id ) {
			merchanterService.global = merchanterService.helper.LoadSettings( _customer_id );
			return await Task.Run( () => merchanterService.helper.GetProduct(_customer_id, _product_id ) );
		}

		private Task<List<Product>> GetProductsAsync( int _customer_id ) {
            merchanterService.global = merchanterService.helper.LoadSettings( _customer_id );
            return Task.Run( () => merchanterService.helper.GetProducts( _customer_id ) );
		}
    }
}
