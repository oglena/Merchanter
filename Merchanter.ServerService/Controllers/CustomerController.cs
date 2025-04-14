using Merchanter.Classes;
using Merchanter.ServerService.Models;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Merchanter.ServerService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase {
        [HttpGet("{CID}/GetCustomer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer>>> GetCustomer(string CID) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
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

        [HttpPost("{CID}/GetCustomerLogs")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Log>>>> GetCustomerLogs(string CID, [FromBody] ApiFilter _api_filter) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (_api_filter.Pager != null) {
                    List<Log> customer_logs = await customerService.GetCustomerLogs(customer_id, _api_filter);
                    _api_filter.TotalCount = await customerService.GetCustomerLogCount(customer_id, _api_filter);

                    if (customer_logs != null) {
                        return Ok(new BaseResponseModel<List<Log>>() { ApiFilter = _api_filter, Success = true, Data = customer_logs, ErrorMessage = "" });
                    }
                    else {
                        return Ok(new BaseResponseModel<List<Log>>() { Success = false, Data = null, ErrorMessage = "Error getting customer logs." });
                    }
                }
                else {
                    return BadRequest("Pager information is invalid.");
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPost("{CID}/GetCustomerNotifications")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Notification>>>> GetCustomerNotifications(string CID, [FromBody] ApiFilter _api_filter) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
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

        [HttpPut("{CID}/SaveCustomer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer?>>> SaveCustomer(string CID, [FromBody] Customer _customer) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                Customer? customer = await customerService.SaveCustomer(customer_id, _customer);
                if (customer != null) {
                    return Ok(new BaseResponseModel<Customer>() { Success = true, Data = customer, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Customer>() { Success = false, Data = null, ErrorMessage = "Error save customer" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }
    }
}
