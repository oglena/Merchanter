using ApiService.Models;
using ApiService.Services;
using Merchanter.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiService.Controllers {
    /// <summary>
    /// Provides customer-related API endpoints for Merchanter.
    /// </summary>
    [Route("api/[controller]")]
    [SwaggerTag("Customer endpoint for Merchanter.")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, ICatalogService catalogService) : ControllerBase {
        /// <summary>
        /// Retrieves a single customer.
        /// </summary>
        /// <returns>Customer details wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetCustomer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer>>> GetCustomer() {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                Customer customer = await customerService.GetCustomer(customer_id);
                if (customer != null) {
                    return Ok(new BaseResponseModel<Customer>() { Success = true, Data = customer, ErrorMessage = "" });
                }
                else { //this should never happen, but just in case
                    return Ok(new BaseResponseModel<Customer>() { Success = false, Data = null, ErrorMessage = "Customer not found." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Retrieves a list of customer logs.
        /// </summary>
        /// <param name="_filters">Filtering options.</param>
        /// <returns>List of logs wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetCustomerLogs")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Log>?>>> GetCustomerLogs([FromBody] ApiFilter? _filters) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                _filters ??= new ApiFilter() { Filters = null, Sort = null, Pager = null };
                _filters.ExtendedQueryResponses = await catalogService.GetExtendedQuery(customer_id, _filters, typeof(Log));
                var logs = await customerService.GetCustomerLogs(customer_id, _filters);
                return Ok(new BaseResponseModel<List<Log>?>() { Success = logs != null, Data = logs ?? [], ApiFilter = _filters, ErrorMessage = logs != null ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Retrieves a list of customer notifications.
        /// </summary>
        /// <param name="_filters">Filtering options.</param>
        /// <returns>List of notifications wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetCustomerNotifications")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Notification>?>>> GetCustomerNotifications([FromBody] ApiFilter? _filters) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                _filters ??= new ApiFilter() { Filters = null, Sort = null, Pager = null };
                _filters.ExtendedQueryResponses = await catalogService.GetExtendedQuery(customer_id, _filters, typeof(Notification));
                var customer_notifications = await customerService.GetCustomerNotifications(customer_id, _filters);
                return Ok(new BaseResponseModel<List<Notification>?>() { Success = customer_notifications != null, Data = customer_notifications ?? [], ApiFilter = _filters, ErrorMessage = customer_notifications != null ? "" : "Error -1" });
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Returns a list of customers.
        /// </summary>
        /// <returns>List of customers wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetCustomers")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Customer>?>>> GetCustomers() {
            var customers = await customerService.GetCustomers();
            if (customers != null) {
                return Ok(new BaseResponseModel<List<Customer>?>() { Success = true, Data = customers, ErrorMessage = "" });
            }
            else {
                return Ok(new BaseResponseModel<List<Customer>?>() { Success = false, Data = null, ErrorMessage = "Error getting customers" });
            }
        }

        /// <summary>
        /// Updates or creates a customer.
        /// </summary>
        /// <param name="customer">Customer object to save.</param>
        /// <returns>Saved customer wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveCustomer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer?>>> SaveCustomer([FromBody] Customer customer) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                Customer? saved_customer = await customerService.SaveCustomer(customer_id, customer);
                if (saved_customer != null) {
                    return Ok(new BaseResponseModel<Customer?>() { Success = true, Data = saved_customer, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Customer?>() { Success = false, Data = null, ErrorMessage = "Error save customer" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }
    }
}
