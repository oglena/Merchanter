using Merchanter.Classes;
using MerchanterApp.ServerService.Repositories;

namespace MerchanterApp.ServerService.Services {
    public interface ICustomerService {
        Task<Customer> GetCustomer( int _customer_id );
        Task<List<Log>> GetCustomerLogs( int _customer_id, ApiFilter _filters );
        Task<List<Notification>> GetCustomerNotifications( int _customer_id, ApiFilter _filters );
        Task<Customer?> SaveCustomer( int _customer_id, Customer _customer );
        Task<List<Customer>> GetCustomers();
        Task<int> GetCustomerLogCount( int _customer_id );
        Task<int> GetCustomerLogCount( int _customer_id, ApiFilter _filters );
    }

    public class CustomerService( ICustomerRepository customerRepository ) :ICustomerService {

        public Task<Customer> GetCustomer( int _customer_id ) {
            return customerRepository.GetCustomerById( _customer_id );
        }

        public Task<List<Log>> GetCustomerLogs( int _customer_id, ApiFilter _filters ) {
            return customerRepository.GetCustomerLogsById( _customer_id, _filters );
        }

        public Task<int> GetCustomerLogCount( int _customer_id ) {
            return customerRepository.GetCustomerLogCount( _customer_id );
        }

        public Task<int> GetCustomerLogCount( int _customer_id, ApiFilter _filters ) {
            return customerRepository.GetCustomerLogCount( _customer_id, _filters );
        }

        public Task<List<Notification>> GetCustomerNotifications( int _customer_id, ApiFilter _filters ) {
            return customerRepository.GetCustomerNotificationsById( _customer_id, _filters );
        }

        public Task<Customer?> SaveCustomer( int _customer_id, Customer _customer ) {
            return customerRepository.SaveCustomer( _customer_id, _customer );
        }

        public Task<List<Customer>> GetCustomers() {
            return customerRepository.GetAllCustomers();
        }
    }
}
