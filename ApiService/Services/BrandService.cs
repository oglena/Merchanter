using ApiService.Repositories;
using Merchanter.Classes;

namespace ApiService.Services {
    public interface IBrandService {
        Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters);
        Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters);
        Task<Brand?> SaveBrand(int _customer_id, Brand _brand);
        Task<bool> DeleteBrand(int _customer_id, Brand _brand);
        Task<Brand> GetDefaultBrand(int _customer_id);
    }

    public class BrandService(IBrandRepository brandRepository) : IBrandService {
        public Task<List<Brand>> GetBrands(int _customer_id, ApiFilter _filters) {
            return brandRepository.GetBrands(_customer_id, _filters);
        }

        public Task<int> GetBrandsCount(int _customer_id, ApiFilter _filters) {
            return brandRepository.GetBrandsCount(_customer_id, _filters);
        }

        public async Task<Brand?> SaveBrand(int _customer_id, Brand _brand) {
            if (_brand.id > 0) {
                return await brandRepository.UpdateBrand(_customer_id, _brand);
            }
            else {
                return await brandRepository.CreateBrand(_customer_id, _brand);
            }
        }

        public async Task<bool> DeleteBrand(int _customer_id, Brand _brand) {
            if (_brand.id > 0) {
                return await brandRepository.DeleteBrand(_customer_id, _brand);
            }
            return false;
        }

        public async Task<Brand> GetDefaultBrand(int _customer_id) {
            return await brandRepository.GetDefaultBrand(_customer_id);
        }
    }
}
