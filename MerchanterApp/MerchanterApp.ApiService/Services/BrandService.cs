using Merchanter.Classes;
using MerchanterApp.ApiService.Repositories;

namespace MerchanterApp.ApiService.Services {
    public interface IBrandService {
        Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters);
        Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters);
    }

    public class BrandService(IBrandRepository brandRepository) : IBrandService {
        public Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters) {
            return brandRepository.GetBrands(_customer_id, _filters);
        }

        public Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters) {
            return brandRepository.GetBrandsCount(_customer_id, _filters);
        }
    }
}
