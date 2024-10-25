using Merchanter;
using Merchanter.Classes;
using Merchanter.Classes.Settings;
using MerchanterApp.ApiService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MerchanterApp.ApiService.Repositories {
    public interface IProductRepository {
        Task<List<Product>?> GetProducts( int _customer_id );
    }
    public class ProductRepository( MerchanterService merchanterService ) :IProductRepository {
        public async Task<List<Product>?> GetProducts( int _customer_id ) {
            return await GetProductsAsync( _customer_id );
        }

        private Task<List<Product>?> GetProductsAsync( int _customer_id ) {
            merchanterService.global = merchanterService.helper.LoadSettings( _customer_id );
            return Task.Run( () => merchanterService.helper.GetProducts( _customer_id ) );
        }
    }
}
