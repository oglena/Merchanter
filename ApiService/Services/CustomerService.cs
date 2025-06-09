using ApiService.Repositories;
using Merchanter.Classes;

namespace ApiService.Services {
    /// <summary>
    /// Defines methods for managing customer-related operations, including retrieving customer data, logs,
    /// notifications,  and saving customer information.
    /// </summary>
    /// <remarks>This interface provides asynchronous methods for interacting with customer data. It includes
    /// functionality for  retrieving individual customers, customer logs, and notifications, as well as saving customer
    /// information and  retrieving a list of all customers. Methods typically require a customer identifier and may
    /// accept additional  filtering parameters.</remarks>
    public interface ICustomerService {
        /// <summary>
        /// Retrieves the customer associated with the specified customer ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch customer details. Ensure that
        /// the provided customer ID is valid and exists in the system.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer to retrieve. Must be a positive integer.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.  The task result contains the <see
        /// cref="Customer"/> object corresponding to the specified customer ID. If no customer is found, the result may
        /// be <see langword="null"/>.</returns>
        Task<Customer> GetCustomer(int _customer_id);

        /// <summary>
        /// Retrieves a list of logs associated with a specific customer, filtered by the provided criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch logs from the underlying data
        /// source.  Ensure that the provided customer ID and filters are valid to avoid exceptions.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer whose logs are to be retrieved. Must be a positive integer.</param>
        /// <param name="_filters">The filtering criteria to apply when retrieving the logs. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Log"/>
        /// objects matching the specified customer and filter criteria. Returns an empty list if no logs are found.</returns>
        Task<List<Log>?> GetCustomerLogs(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Retrieves a list of notifications for a specific customer, filtered by the provided criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch notifications for the
        /// specified customer. Use the <paramref name="_filters"/> parameter to refine the results, such as by date
        /// range or notification type.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer whose notifications are being retrieved. Must be a positive integer.</param>
        /// <param name="_filters">An object containing filtering criteria to narrow down the notifications. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
        /// cref="Notification"/> objects matching the specified customer and filter criteria. Returns an empty list if
        /// no notifications are found.</returns>
        Task<List<Notification>?> GetCustomerNotifications(int _customer_id, ApiFilter _filters);

        /// <summary>
        /// Saves the specified customer data to the database.
        /// </summary>
        /// <param name="_customer_id">The unique identifier of the customer to be saved.</param>
        /// <param name="_customer">The customer object containing the data to be saved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the saved  Customer object if
        /// the operation is successful; otherwise, null.</returns>
        Task<Customer?> SaveCustomer(int _customer_id, Customer _customer);

        /// <summary>
        /// Retrieves a list of customers asynchronously.
        /// </summary>
        /// <remarks>This method performs the operation asynchronously and should be awaited to ensure 
        /// proper completion. The returned list may include all customers or a filtered subset  depending on the
        /// implementation.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
        /// cref="Customer"/> objects representing the customers. If no customers are found,  the list will be empty.</returns>
        Task<List<Customer>?> GetCustomers();

        /// <summary>
        /// Retrieves the total number of logs associated with a specific customer, filtered by the provided criteria.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to query logs for a specific customer.
        /// Ensure that the provided filters are valid and  appropriately configured to avoid unexpected
        /// results.</remarks>
        /// <param name="_customer_id">The unique identifier of the customer whose logs are being queried. Must be a positive integer.</param>
        /// <param name="_filters">An <see cref="ApiFilter"/> object specifying the filtering criteria for the logs, such as date range or log
        /// type.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total count of logs matching
        /// the specified criteria.</returns>
        Task<int> GetCustomerLogCount(int _customer_id, ApiFilter _filters);
    }

    /// <summary>
    /// Provides methods for managing customer data and related operations.
    /// </summary>
    /// <remarks>This service acts as a layer for accessing and manipulating customer information, logs,
    /// notifications,  and other related data. It relies on an underlying repository implementation to perform data
    /// operations.</remarks>
    /// <param name="customerRepository"></param>
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService {
        /// <inheritdoc />
        public Task<Customer> GetCustomer(int _customer_id) {
            return customerRepository.GetCustomerById(_customer_id);
        }

        /// <inheritdoc />
        public Task<List<Log>?> GetCustomerLogs(int _customer_id, ApiFilter _filters) {
            return customerRepository.GetCustomerLogsById(_customer_id, _filters);
        }

        /// <inheritdoc />
        public Task<int> GetCustomerLogCount(int _customer_id, ApiFilter _filters) {
            return customerRepository.GetCustomerLogCount(_customer_id, _filters);
        }

        /// <inheritdoc />
        public Task<List<Notification>?> GetCustomerNotifications(int _customer_id, ApiFilter _filters) {
            return customerRepository.GetCustomerNotificationsById(_customer_id, _filters);
        }

        /// <inheritdoc />
        public Task<Customer?> SaveCustomer(int _customer_id, Customer _customer) {
            return customerRepository.SaveCustomer(_customer_id, _customer);
        }

        /// <inheritdoc />
        public Task<List<Customer>?> GetCustomers() {
            return customerRepository.GetAllCustomers();
        }
    }
}
