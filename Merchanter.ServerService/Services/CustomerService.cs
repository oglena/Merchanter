using Merchanter.Classes;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;

namespace Merchanter.ServerService.Services {
    public interface ICustomerService {
        Task<Customer> GetCustomer( int _customer_id );
        Task<List<Log>> GetCustomerLogs( int _customer_id, int _items_per_page, int _current_page_index, Dictionary<string,string?> _filters );
        Task<List<Notification>> GetCustomerNotifications( int _customer_id, int _items_per_page, int _current_page_index, Dictionary<string, string?> _filters );
        Task<Customer> SaveCustomer( int _customer_id, Customer _customer );
        Task<List<Customer>> GetCustomers();
    }

    public class CustomerService( ICustomerRepository customerRepository ) :ICustomerService {

        public Task<Customer> GetCustomer( int _customer_id ) {
            return customerRepository.GetCustomerById( _customer_id );
        }

        public Task<List<Log>> GetCustomerLogs( int _customer_id, int _items_per_page, int _current_page_index, Dictionary<string, string?> _filters ) {
            return customerRepository.GetCustomerLogsById( _customer_id, _items_per_page, _current_page_index, _filters );
        }

        public Task<List<Notification>> GetCustomerNotifications( int _customer_id, int _items_per_page, int _current_page_index, Dictionary<string, string?> _filters ) {
            return customerRepository.GetCustomerNotificationsById( _customer_id, _items_per_page, _current_page_index, _filters );
        }

        public Task<Customer> SaveCustomer( int _customer_id, Customer _customer ) {
            return customerRepository.SaveCustomer( _customer_id, _customer );
        }

        public Task<List<Customer>> GetCustomers() {
            return customerRepository.GetAllCustomers();
        }
    }
}
