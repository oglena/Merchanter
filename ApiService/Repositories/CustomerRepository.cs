using Merchanter.Classes;
using ApiService.Services;

namespace ApiService.Repositories {
    public interface ICustomerRepository {
        Task<Customer> GetCustomerById(int _customer_id);
        Task<List<Log>> GetCustomerLogsById(int _customer_id, ApiFilter _filters);
        Task<List<Notification>> GetCustomerNotificationsById(int _customer_id, ApiFilter _filters);
        Task<List<Customer>> GetAllCustomers();
        Task<int> GetCustomerLogCount(int _customer_id, ApiFilter _filters);
        Task<Customer?> SaveCustomer(int _customer_id, Customer _customer);
        MerchanterService merchanterService { get; set; }
    }
    public class CustomerRepository(MerchanterService merchanterService) : ICustomerRepository {
        public Customer customer = new Customer();
        public List<Log> customer_logs = new List<Log>();
        public List<Notification> customer_notifications = new List<Notification>();
        public List<Customer> customers = [];

        MerchanterService ICustomerRepository.merchanterService { get; set; } = new MerchanterService();

        public async Task<Customer> GetCustomerById(int _customer_id) {
            //merchanterService.ChangeCustomer(_customer_id);
            return await GetCustomer(_customer_id);
        }

        public async Task<List<Log>> GetCustomerLogsById(int _customer_id, ApiFilter _filters) {
            //merchanterService.ChangeCustomer(_customer_id);
            return await GetCustomerLogs(_customer_id, _filters);
        }

        public async Task<List<Notification>> GetCustomerNotificationsById(int _customer_id, ApiFilter _filters) {
            //merchanterService.ChangeCustomer(_customer_id);
            return await GetCustomerNotifications(_customer_id, _filters);
        }

        public async Task<Customer?> SaveCustomer(int _customer_id, Customer _customer) {
            //merchanterService.ChangeCustomer(_customer_id);
            return await Task.Run(() => merchanterService.helper.SaveCustomer(_customer_id, _customer, false));
        }

        public async Task<List<Customer>> GetAllCustomers() {
            //merchanterService.global = null;
            return await GetCustomers();
        }

        public async Task<int> GetCustomerLogCount(int _customer_id, ApiFilter _filters) {
            return await Task.Run(() => merchanterService.helper.GetLogsCount(_customer_id, _filters));
        }

        private async Task<Customer> GetCustomer(int _customer_id) {
            return await merchanterService.helper.GetCustomer(_customer_id) ?? new Customer() { customer_id = _customer_id };
        }

        private async Task<List<Log>> GetCustomerLogs(int _customer_id, ApiFilter _filters) {
            return await merchanterService.helper.GetLastLogs(_customer_id, _filters);
        }

        private async Task<List<Notification>> GetCustomerNotifications(int _customer_id, ApiFilter _filters) {
            return await merchanterService.helper.GetNotifications(_customer_id, null, _filters);
        }

        private async Task<List<Customer>> GetCustomers() {
            return await merchanterService.helper.GetCustomers();
        }
    }
}
