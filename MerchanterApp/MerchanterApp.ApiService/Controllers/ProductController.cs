using Merchanter.Classes;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ApiService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase {

        [HttpPost("GetProducts")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> GetProducts(ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var products = await productService.GetProducts(customer_id, _filters);

                return Ok(new BaseResponseModel() { Success = products != null, Data = products ?? [], ErrorMessage = products != null ? "" : "Error -1" });
                //return Ok( products );
            }
            return BadRequest();
        }

        [HttpGet("GetProduct/{PID}")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> GetProduct(string PID) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                if (int.TryParse(PID, out int product_id) && product_id > 0) {
                    var product = await productService.GetProduct(customer_id, product_id);

                    return Ok(new BaseResponseModel() { Success = product != null, Data = product ?? new(), ErrorMessage = product != null ? "" : "Error -1" });
                }
                //return Ok( products );
            }
            return BadRequest();
        }
    }
}
