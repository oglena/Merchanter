using Merchanter.Classes;
using ApiService.Services;

namespace ApiService.Repositories {
    /// <summary>
    /// Interface for category-related data access operations.
    /// </summary>
    public interface ICategoryRepository {
        /// <summary>
        /// Retrieves a list of categories based on customer ID and filters.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filter</param>
        /// <returns>A list of categories.</returns>
        Task<List<Category>> GetCategories(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Retrieves the count of categories based on customer ID and filters.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_filters">Api Filter</param>
        /// <returns>The count of categories.</returns>
        Task<int> GetCategoriesCount(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Creates or updates a category for the specified customer.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category object to create or update</param>
        /// <returns>The created or updated category.</returns>
        Task<Category?> CreateCategory(int _customer_id, Category _category);

        /// <summary>
        /// Updates an existing category for the specified customer.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category object to update</param>
        /// <returns>The updated category.</returns>
        Task<Category?> UpdateCategory(int _customer_id, Category _category);

        /// <summary>
        /// Deletes a category for the specified customer.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category">Category object to delete</param>
        /// <returns>True if the category was deleted successfully, otherwise false.</returns>
        Task<bool> DeleteCategory(int _customer_id, Category _category);

        /// <summary>
        /// Retrieves the default category for the specified customer.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <returns>The default category.</returns>
        Task<Category> GetDefaultCategory(int _customer_id);

        /// <summary>
        /// Retrieves a specific category by category ID.
        /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category_id">Category ID</param>
        /// <returns>The category if found, otherwise null.</returns>
        Task<Category?> GetCategory(int _customer_id, int _category_id);

        /// <summary>
        /// Retrieves a list of category targets for a specific category.
        /// /// </summary>
        /// <param name="_customer_id">Customer ID</param>
        /// <param name="_category_id">Category ID</param>
        /// <returns>A list of category targets.</returns>
        Task<List<CategoryTarget>> GetCategoryTargets(int _customer_id, int _category_id);
    }

    /// <inheritdoc />
    public class CategoryRepository(MerchanterService merchanterService) : ICategoryRepository {
        /// <inheritdoc />
        public async Task<List<Category>> GetCategories(int _customer_id, ApiFilter _filters) {
            return await GetCategoriesAsync(_customer_id, _filters);
        }

        /// <inheritdoc />
        public async Task<int> GetCategoriesCount(int _customer_id, ApiFilter _filters) {
            return await GetCategoriesCountAsync(_customer_id, _filters);
        }

        /// <inheritdoc />
        private async Task<List<Category>> GetCategoriesAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetCategories(_customer_id, _filters));
        }

        /// <inheritdoc />
        private async Task<int> GetCategoriesCountAsync(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetCategoryCount(_customer_id, _filters));
        }

        /// <inheritdoc />
        public async Task<Category?> CreateCategory(int _customer_id, Category _category) {
            return await Task.Run(() => merchanterService.helper.InsertCategory(_customer_id, _category));
        }

        /// <inheritdoc />
        public async Task<Category?> UpdateCategory(int _customer_id, Category _category) {
            return await Task.Run(() => merchanterService.helper.UpdateCategory(_customer_id, _category));
        }

        /// <inheritdoc />
        public async Task<bool> DeleteCategory(int _customer_id, Category _category) {
            return await Task.Run(() => merchanterService.helper.DeleteCategory(_customer_id, _category.id));
        }

        /// <inheritdoc />
        public async Task<Category> GetDefaultCategory(int _customer_id) {
            return await Task.Run(() => merchanterService.helper.GetRootCategory(_customer_id));
        }

        /// <inheritdoc />
        public async Task<Category?> GetCategory(int _customer_id, int _category_id) {
            return await Task.Run(() => merchanterService.helper.GetCategory(_customer_id, _category_id));
        }

        /// <inheritdoc />
        public async Task<List<CategoryTarget>> GetCategoryTargets(int _customer_id, int _category_id) {
            return await Task.Run(() => merchanterService.helper.GetCategoryTargets(_customer_id, _category_id));
        }
    }
}