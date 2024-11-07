using Merchanter.Classes;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Models;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.Json;

namespace Merchanter.ServerService.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class CustomerController( ICustomerService customerService ) :ControllerBase {

        [HttpGet( "{CID}/GetCustomer" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer>>> GetCustomer( string CID ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                Customer customer = await customerService.GetCustomer( customer_id );
                if( customer != null ) {
                    return Ok( new BaseResponseModel<Customer>() { Success = customer != null, Data = customer ?? new(), ErrorMessage = customer != null ? "" : "Error -1" } );
                }
                else {
                    return NotFound();
                }
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpPost( "{CID}/GetCustomerLogs" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Log>>>> GetCustomerLogs( string CID, [FromBody] ApiFilter _api_filter ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( _api_filter.Pager != null ) {
                    List<Log> customer_logs = await customerService.GetCustomerLogs( customer_id, _api_filter );
                    _api_filter.TotalCount = await customerService.GetCustomerLogCount( customer_id, _api_filter );

                    if( customer_logs != null ) {
                        return Ok( new BaseResponseModel<List<Log>>() { ApiFilter = _api_filter, Success = customer_logs != null, Data = customer_logs ?? [], ErrorMessage = customer_logs != null ? "" : "Error -1" } );
                    }
                    else {
                        return BadRequest( "Filter response empty." );
                    }
                }
                return StatusCode( 500 );
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpPost( "{CID}/GetCustomerNotifications" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Notification>>>> GetCustomerNotifications( string CID, [FromBody] ApiFilter _api_filter ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( _api_filter.Pager != null && _api_filter.Filters != null ) {
                    List<Notification> customer_notifications = await customerService.GetCustomerNotifications( customer_id, _api_filter );
                    if( customer_notifications != null ) {
                        return Ok( new BaseResponseModel<List<Notification>>() { Success = customer_notifications != null, ErrorMessage = customer_notifications != null ? "" : "Error -1", Data = customer_notifications ?? [] } );
                    }
                    else {
                        return BadRequest( "Filter response empty." );
                    }
                }
                return StatusCode( 500 );
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpGet( "GetCustomers" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Customer>>>> GetCustomers() {
            List<Customer> customers = await customerService.GetCustomers();
            if( customers != null ) {
                return Ok( new BaseResponseModel<List<Customer>>() { Success = customers != null, Data = customers ?? [], ErrorMessage = customers != null ? "" : "Error -1" } );
            }
            else {
                return BadRequest();
            }
        }

        [HttpPut( "{CID}/SaveCustomer" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Customer?>>> SaveCustomer( string CID, [FromBody] Customer _customer ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                Customer? customer = await customerService.SaveCustomer( customer_id, _customer );
                if( customer != null ) {
                    return Ok( new BaseResponseModel<Customer>() { Success = customer != null, Data = customer ?? new(), ErrorMessage = customer != null ? "" : "Error -1" } );
                }
                else {
                    return BadRequest( "Insert failed." );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }
    }
}
