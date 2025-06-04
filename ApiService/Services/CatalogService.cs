using ApiService.Repositories;
using Merchanter.Classes;

namespace ApiService.Services {
    /// <summary>
    /// Interface for catalog-related operations.
    /// </summary>
    public interface ICatalogService {

        /// <summary>
        /// Retrieves extended properties for catalog object.
        /// </summary>
        /// <param name="_customer_id">The ID of the customer.</param>
        /// <param name="_filters">Filter criteria for the catalog object.</param>
        /// <param name="_type">Type of the catalog object.</param>
        /// <returns>Extended properties.</returns>
        Task<Dictionary<string, dynamic>> GetExtendedQuery(int _customer_id, ApiFilter _filters, Type _type);
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogService"/> class.
    /// </summary>
    /// <param name="catalogRepository">The catalog repository.</param>
    public class CatalogService(ICatalogRepository catalogRepository) : ICatalogService {
        /// <summary>
        /// The repository for catalog-related data access operations.
        /// </summary>
        private readonly ICatalogRepository catalogRepository = catalogRepository;

        /// <inheritdoc />
        public Task<Dictionary<string, dynamic>> GetExtendedQuery(int _customer_id, ApiFilter _filters, Type _type) {
            return catalogRepository.GetExtendedQuery(_customer_id, _filters, _type);
        }
    }
}
