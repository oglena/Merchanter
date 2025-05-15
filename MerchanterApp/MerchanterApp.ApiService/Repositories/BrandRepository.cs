using Merchanter.Classes;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Repositories {
    public interface IBrandRepository {
        Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters);
        Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters);
        Task<Brand?> CreateBrand(int _customer_id, Brand _brand);
        Task<Brand?> UpdateBrand(int _customer_id, Brand _brand);
        Task<bool> DeleteBrand(int _customer_id, Brand _brand);
        Task<Brand> GetDefaultBrand(int _customer_id);
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
        public async Task<Brand?> CreateBrand(int _customer_id, Brand _brand) {
            return await Task.Run(() => merchanterService.helper.InsertBrand(_customer_id, _brand));
        }
        public async Task<Brand?> UpdateBrand(int _customer_id, Brand _brand) {
            return await Task.Run(() => merchanterService.helper.UpdateBrand(_customer_id, _brand));
        }
        public async Task<bool> DeleteBrand(int _customer_id, Brand _brand) {
            return await Task.Run(() => merchanterService.helper.DeleteBrand(_customer_id, _brand.id));
        }

        public async Task<Brand> GetDefaultBrand(int _customer_id) {
            return await Task.Run(() => merchanterService.helper.GetDefaultBrand(_customer_id));
        }
    }
}