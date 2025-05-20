using Merchanter.Classes;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MerchanterApp.ApiService.Controllers {
    /// <summary>
    /// Provides customer-related API endpoints for Merchanter.
    /// </summary>
    [Route("api/[controller]")]
    [SwaggerTag("Customer endpoint for Merchanter.")]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase {
        /// <summary>
        /// Retrieves a single customer by ID.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <returns>Customer details wrapped in a BaseResponseModel.</returns>
        [HttpGet("{id}/GetCustomer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer>>> GetCustomer(string id) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
                Customer customer = await customerService.GetCustomer(customer_id);
                if (customer != null) {
                    return Ok(new BaseResponseModel<Customer>() { Success = true, Data = customer, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Customer>() { Success = false, Data = null, ErrorMessage = "Customer not found." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Retrieves a list of customer logs.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <param name="_api_filter">Filtering options.</param>
        /// <returns>List of logs wrapped in a BaseResponseModel.</returns>
        [HttpPost("{id}/GetCustomerLogs")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Log>>>> GetCustomerLogs(string id, [FromBody] ApiFilter _api_filter) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
                List<Log> customer_logs = await customerService.GetCustomerLogs(customer_id, _api_filter);
                _api_filter.TotalCount = await customerService.GetCustomerLogCount(customer_id, _api_filter);

                if (customer_logs != null) {
                    return Ok(new BaseResponseModel<List<Log>>() { ApiFilter = _api_filter, Success = true, Data = customer_logs, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<List<Log>>() { Success = false, Data = null, ErrorMessage = "Error getting customer logs." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Retrieves a list of customer notifications.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <param name="_api_filter">Filtering options.</param>
        /// <returns>List of notifications wrapped in a BaseResponseModel.</returns>
        [HttpPost("{id}/GetCustomerNotifications")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Notification>>>> GetCustomerNotifications(string id, [FromBody] ApiFilter _api_filter) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
                if (_api_filter.Pager != null && _api_filter.Filters != null) {
                    List<Notification> customer_notifications = await customerService.GetCustomerNotifications(customer_id, _api_filter);
                    if (customer_notifications != null) {
                        return Ok(new BaseResponseModel<List<Notification>>() { Success = true, ErrorMessage = "", Data = customer_notifications ?? [] });
                    }
                    else {
                        return Ok(new BaseResponseModel<List<Notification>>() { Success = false, Data = null, ErrorMessage = "Error getting customer notifications." });
                    }
                }
                return StatusCode(500);
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Returns a list of customers.
        /// </summary>
        /// <returns>List of customers wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetCustomers")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Customer>>>> GetCustomers() {
            List<Customer> customers = await customerService.GetCustomers();
            if (customers != null) {
                return Ok(new BaseResponseModel<List<Customer>>() { Success = true, Data = customers, ErrorMessage = "" });
            }
            else {
                return Ok(new BaseResponseModel<List<Customer>>() { Success = false, Data = null, ErrorMessage = "Error getting customers" });
            }
        }

        /// <summary>
        /// Updates or creates a customer.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <param name="customer">Customer object to save.</param>
        /// <returns>Saved customer wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveCustomer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer?>>> SaveCustomer(string id, [FromBody] Customer customer) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0 && customer.customer_id == customer_id) {
                Customer? saved_customer = await customerService.SaveCustomer(customer_id, customer);
                if (saved_customer != null) {
                    return Ok(new BaseResponseModel<Customer>() { Success = true, Data = saved_customer, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Customer>() { Success = false, Data = null, ErrorMessage = "Error save customer" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }
    }
}
