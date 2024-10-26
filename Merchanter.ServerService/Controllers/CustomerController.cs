using Merchanter.Classes;
using Merchanter.ServerService.Models;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Merchanter.ServerService.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class CustomerController( ICustomerService customerService ) :ControllerBase {

        [HttpGet( "{CID}/GetCustomer" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> GetCustomer( string CID ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                Customer customer = await customerService.GetCustomer( customer_id );
                if( customer != null ) {
                    return Ok( new BaseResponseModel() { Success = customer != null, Data = customer != null ? customer : new(), ErrorMessage = customer != null ? "" : "Error -1" } );
                    //return Ok( products );
                }
                else {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet( "GetCustomers" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> GetCustomers() {
            List<Customer> customers = await customerService.GetCustomers();
            if( customers != null ) {
                return Ok( new BaseResponseModel() { Success = customers != null, Data = customers != null ? customers : new(), ErrorMessage = customers != null ? "" : "Error -1" } );
                //return Ok( products );
            }
            else {
                return BadRequest();
            }
        }

        [HttpPut( "{CID}/SaveCustomer" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> SaveCustomer( string CID, [FromBody] Customer _customer ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                Customer customer = await customerService.SaveCustomer( customer_id, _customer );
                if( customer != null ) {
                    return Ok( new BaseResponseModel() { Success = customer != null, Data = customer != null ? customer : new(), ErrorMessage = customer != null ? "" : "Error -1" } );
                    //return Ok( products );
                }
                else {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
