using ApiService.Repositories;
using Merchanter.Classes;

namespace ApiService.Services {
    public interface ICategoryService {
        Task<List<Category>> GetCategories(int _customer_id, ApiFilter _filters);
        Task<int> GetCategoriesCount(int _customer_id, ApiFilter _filters);
        Task<Category?> SaveCategory(int _customer_id, Category _category);
        Task<bool> DeleteCategory(int _customer_id, Category _category);
        Task<Category> GetDefaultCategory(int _customer_id);
        Task<Category?> GetCategory(int _customer_id, int _category_id);
        Task<List<CategoryTarget>> GetCategoryTargets(int _customer_id, int _category_id);
    }

    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService {
        public Task<List<Category>> GetCategories(int _customer_id, ApiFilter _filters) {
            return categoryRepository.GetCategories(_customer_id, _filters);
        }

        public Task<int> GetCategoriesCount(int _customer_id, ApiFilter _filters) {
            return categoryRepository.GetCategoriesCount(_customer_id, _filters);
        }

        public async Task<Category?> SaveCategory(int _customer_id, Category _category) {
            if (_category.id > 0) {
                return await categoryRepository.UpdateCategory(_customer_id, _category);
            }
            else {
                return await categoryRepository.CreateCategory(_customer_id, _category);
            }
        }

        public async Task<bool> DeleteCategory(int _customer_id, Category _category) {
            if (_category.id > 0) {
                return await categoryRepository.DeleteCategory(_customer_id, _category);
            }
            return false;
        }

        public async Task<Category> GetDefaultCategory(int _customer_id) {
            return await categoryRepository.GetDefaultCategory(_customer_id);
        }

        public async Task<Category?> GetCategory(int _customer_id, int _category_id) {
            return await categoryRepository.GetCategory(_customer_id, _category_id);
        }

        public async Task<List<CategoryTarget>> GetCategoryTargets(int _customer_id, int _category_id) {
            return await categoryRepository.GetCategoryTargets(_customer_id, _category_id);
        }
    }
}
