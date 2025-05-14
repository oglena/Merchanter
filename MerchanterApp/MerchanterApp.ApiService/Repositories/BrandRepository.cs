using Merchanter.Classes;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Repositories {
    public interface IBrandRepository {
        Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters);
        Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters);
    }

    public class BrandRepository(MerchanterService merchanterService) : IBrandRepository {
        public async Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters) {
            return await GetBrandsAsync(_customer_id, _filters);
        }
        public async Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters) {
            return await GetBrandsCountAsync(_customer_id, _filters);
        }

        private async Task<List<Brand>> GetBrandsAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetBrands(_customer_id, _filters));
        }

        private async Task<int> GetBrandsCountAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetBrandsCount(_customer_id, _filters));
        }
    }
}