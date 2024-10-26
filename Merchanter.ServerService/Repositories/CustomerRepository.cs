using Merchanter.Classes;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Services;
using System.Diagnostics;
using System.Management;

namespace Merchanter.CustomerService.Repositories {
    public interface ICustomerRepository {
        Task<Customer> GetCustomerById( int _customer_id );
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> SaveCustomer( int _customer_id, Customer _customer );
        MerchanterService merchanterService { get; set; }
    }
    public class CustomerRepository( MerchanterService merchanterService ) :ICustomerRepository {
        public Customer customer = new Customer();
        public List<Customer> customers = [];

        MerchanterService ICustomerRepository.merchanterService { get; set; } = new MerchanterService();

        public async Task<Customer> GetCustomerById( int _customer_id ) {
            merchanterService.ChangeCustomer( _customer_id );
            return await GetCustomer( _customer_id );
        }

        public async Task<Customer> SaveCustomer( int _customer_id, Customer _customer ) {
            merchanterService.ChangeCustomer( _customer_id );
            return await Task.Run( () => merchanterService.helper.SaveCustomer( _customer_id, _customer ) );
        }

        public async Task<List<Customer>> GetAllCustomers() {
            merchanterService.global = null;
            return await GetCustomers();
        }

        private async Task<Customer> GetCustomer( int _customer_id ) {
            return await Task.Run( () => customer = merchanterService.helper.GetCustomer( _customer_id ) );
        }

        private async Task<List<Customer>> GetCustomers() {
            return await Task.Run( () => customers = merchanterService.helper.GetCustomers() );
        }
    }
}
