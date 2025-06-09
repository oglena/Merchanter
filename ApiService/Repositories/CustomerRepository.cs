using Merchanter.Classes;
using ApiService.Services;

namespace ApiService.Repositories {
    /// <summary>
    /// Defines a contract for managing customer-related data and operations.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving, saving, and interacting with customer data, 
    /// including logs, notifications, and general customer information. It also includes support for  filtering data
    /// using <see cref="ApiFilter"/> and accessing related services through  <see cref="Services.MerchanterService"/>.</remarks>
    public interface ICustomerRepository {
        /// <summary>
        /// Retrieves a customer by their unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch customer details.  Ensure
        /// that the provided identifier is valid and corresponds to an existing customer.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer to retrieve. Must be a positive integer.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.  The task result contains the <see
        /// cref="Customer"/> object corresponding to the specified identifier,  or <see langword="null"/> if no
        /// customer with the given identifier exists.</returns>
        Task<Customer> GetCustomerById(int _customer_id);

        /// <summary>
        /// Retrieves a list of logs associated with a specific customer, filtered by the provided criteria.
        /// </summary>
        /// <param name="_customer_id">The unique identifier of the customer whose logs are to be retrieved. Must be a positive integer.</param>
        /// <param name="_filters">The filter criteria to apply when retrieving logs, such as date ranges or log types. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Log"/>
        /// objects matching the specified customer and filter criteria. Returns an empty list if no logs are found.</returns>
        Task<List<Log>?> GetCustomerLogsById(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Retrieves a list of notifications for a specific customer based on the provided filters.
        /// </summary>
        /// <param name="_customer_id">The unique identifier of the customer whose notifications are to be retrieved. Must be a positive integer.</param>
        /// <param name="_filters">An <see cref="ApiFilter"/> object specifying the criteria to filter the notifications. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
        /// cref="Notification"/> objects matching the specified filters. Returns an empty list if no notifications are
        /// found.</returns>
        Task<List<Notification>?> GetCustomerNotificationsById(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <remarks>This method returns all customers currently stored in the system.  The returned list
        /// will be empty if no customers are available.</remarks>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of  <see cref="Customer"/>
        /// objects representing all customers. If no customers exist, the list will be empty.</returns>
        Task<List<Customer>?> GetAllCustomers();

        /// <summary>
        /// Retrieves the total number of logs associated with a specific customer, filtered by the provided criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to query logs for a specific customer.
        /// The filtering criteria provided in  <paramref name="_filters"/> can be used to narrow down the results based
        /// on specific conditions.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer whose logs are being queried. Must be a positive integer.</param>
        /// <param name="_filters">An <see cref="ApiFilter"/> object specifying the filtering criteria for the logs, such as date range or log
        /// type.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total count of logs matching
        /// the specified criteria.</returns>
        Task<int> GetCustomerLogCount(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Saves the specified customer data to the database.
        /// </summary>
        /// <param name="_customer_id">The unique identifier of the customer to be saved.</param>
        /// <param name="_customer">The customer object containing the data to be saved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the saved  Customer object if
        /// the operation is successful; otherwise, null.</returns>
        Task<Customer?> SaveCustomer(int _customer_id, Customer _customer);

        /// <summary>
        /// Gets or sets the instance of <see cref="Services.MerchanterService"/> used to manage merchant-related operations.
        /// </summary>
        MerchanterService MerchanterService { get; set; }
    }

    /// <summary>
    /// Provides methods for managing customer data, including retrieving, saving, and querying customer information.
    /// </summary>
    /// <remarks>This repository serves as an abstraction for customer-related operations, such as fetching
    /// customer details, logs, notifications,  and managing customer persistence. It interacts with the underlying <see
    /// cref="MerchanterService"/> to perform these operations.</remarks>
    /// <param name="merchanterService"></param>
    public class CustomerRepository(MerchanterService merchanterService) : ICustomerRepository {
        /// <inheritdoc />
        MerchanterService ICustomerRepository.MerchanterService { get; set; } = new MerchanterService();

        /// <inheritdoc />
        public async Task<Customer> GetCustomerById(int _customer_id) {
            return await GetCustomer(_customer_id);
        }

        /// <inheritdoc />
        public async Task<List<Log>?> GetCustomerLogsById(int _customer_id, ApiFilter _filters) {
            return await GetCustomerLogs(_customer_id, _filters);
        }

        /// <inheritdoc />
        public async Task<List<Notification>?> GetCustomerNotificationsById(int _customer_id, ApiFilter _filters) {
            return await GetCustomerNotifications(_customer_id, _filters);
        }

        /// <inheritdoc />
        public async Task<Customer?> SaveCustomer(int _customer_id, Customer _customer) {
            return await Task.Run(() => merchanterService.Helper.SaveCustomer(_customer_id, _customer, false));
        }

        /// <inheritdoc />
        public async Task<List<Customer>?> GetAllCustomers() {
            return await GetCustomers();
        }

        /// <inheritdoc />
        public async Task<int> GetCustomerLogCount(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.Helper.GetLogsCount(_customer_id, _filters));
        }

        /// <inheritdoc />
        private async Task<Customer> GetCustomer(int _customer_id) {
            return await merchanterService.Helper.GetCustomer(_customer_id) ?? new Customer() { customer_id = _customer_id };
        }

        /// <inheritdoc />
        private async Task<List<Log>?> GetCustomerLogs(int _customer_id, ApiFilter _filters) {
            return await merchanterService.Helper.GetLastLogs(_customer_id, _filters);
        }

        /// <inheritdoc />
        private async Task<List<Notification>?> GetCustomerNotifications(int _customer_id, ApiFilter _filters) {
            return await merchanterService.Helper.GetNotifications(_customer_id, null, _filters);
        }

        /// <inheritdoc />
        private async Task<List<Customer>?> GetCustomers() {
            return await merchanterService.Helper.GetCustomers();
        }
    }
}
