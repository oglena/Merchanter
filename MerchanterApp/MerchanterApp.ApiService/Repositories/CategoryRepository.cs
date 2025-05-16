using Merchanter.Classes;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Repositories {
    public interface ICategoryRepository {
        Task<List<Category>> GetCategories(int _customer_id, ApiFilter _filters);
        Task<int> GetCategoriesCount(int _customer_id, ApiFilter _filters);
        Task<Category?> CreateCategory(int _customer_id, Category _category);
        Task<Category?> UpdateCategory(int _customer_id, Category _category);
        Task<bool> DeleteCategory(int _customer_id, Category _category);
        Task<Category> GetDefaultCategory(int _customer_id);
        Task<Category?> GetCategory(int _customer_id, int _category_id);
    }

    public class CategoryRepository(MerchanterService merchanterService) : ICategoryRepository {
        public async Task<List<Category>> GetCategories(int _customer_id, ApiFilter _filters) {
            return await GetCategoriesAsync(_customer_id, _filters);
        }

        public async Task<int> GetCategoriesCount(int _customer_id, ApiFilter _filters) {
            return await GetCategoriesCountAsync(_customer_id, _filters);
        }

        private async Task<List<Category>> GetCategoriesAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetCategories(_customer_id, _filters));
        }

        private async Task<int> GetCategoriesCountAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetCategoryCount(_customer_id, _filters));
        }

        public async Task<Category?> CreateCategory(int _customer_id, Category _category) {
            return await Task.Run(() => merchanterService.helper.InsertCategory(_customer_id, _category));
        }

        public async Task<Category?> UpdateCategory(int _customer_id, Category _category) {
            return await Task.Run(() => merchanterService.helper.UpdateCategory(_customer_id, _category));
        }

        public async Task<bool> DeleteCategory(int _customer_id, Category _category) {
            return await Task.Run(() => merchanterService.helper.DeleteCategory(_customer_id, _category.id));
        }

        public async Task<Category> GetDefaultCategory(int _customer_id) {
            return await Task.Run(() => merchanterService.helper.GetRootCategory(_customer_id));
        }

        public async Task<Category?> GetCategory(int _customer_id, int _category_id) {
            return await Task.Run(() => merchanterService.helper.GetCategory(_customer_id, _category_id));
        }
    }
}