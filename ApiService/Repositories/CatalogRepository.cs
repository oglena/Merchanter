using ApiService.Services;
using Merchanter.Classes;

namespace ApiService.Repositories {
    /// <summary>
    /// Interface for catalog-related data access operations.
    /// </summary>
    public interface ICatalogRepository {

        /// <summary>
        /// Retrieves filter properties for products.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_filters">Filter criteria for the products.</param>
        /// <param name="_type">Type of the object.</param>
        /// <returns>Filter properties.</returns>
        Task<Dictionary<string, dynamic>> GetExtendedQuery(int _customer_id, ApiFilter _filters, Type _type);
    }

    /// <inheritdoc />
    public class CatalogRepository(MerchanterService merchanterService) : ICatalogRepository {

        /// <inheritdoc />
        public async Task<Dictionary<string, dynamic>> GetExtendedQuery(int _customer_id, ApiFilter _filters, Type _type) {
            return await Task.Run(() => merchanterService.helper.GetExtendedQuery(_customer_id, _filters, _type));
        }
    }
}
