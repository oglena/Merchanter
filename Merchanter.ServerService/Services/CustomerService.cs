using Merchanter.Classes;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;

namespace Merchanter.ServerService.Services {
    public interface ICustomerService {
        Task<Customer> GetCustomer( int _customer_id );
        Task<Customer> SaveCustomer( int _customer_id, Customer _customer );
        Task<List<Customer>> GetCustomers();
    }

    public class CustomerService( ICustomerRepository customerRepository ) :ICustomerService {

        public Task<Customer> GetCustomer( int _customer_id ) {
            return customerRepository.GetCustomerById( _customer_id );
        }

        public Task<Customer> SaveCustomer( int _customer_id, Customer _customer ) {
            return customerRepository.SaveCustomer( _customer_id, _customer );
        }

        public Task<List<Customer>> GetCustomers() {
            return customerRepository.GetAllCustomers();
        }
    }
}
