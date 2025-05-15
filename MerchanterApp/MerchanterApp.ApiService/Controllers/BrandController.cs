using Microsoft.AspNetCore.Mvc;
using Merchanter.Classes;
using Microsoft.AspNetCore.Authorization;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController(IBrandService brandService) : ControllerBase {
        [HttpPost("GetBrands")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Brand>>>> GetBrands([FromBody] ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var brands = await brandService.GetBrands(customer_id, _filters);
                _filters.TotalCount = await brandService.GetBrandsCount(customer_id, _filters);
                return Ok(new BaseResponseModel<List<Brand>>() { Success = brands != null, Data = brands ?? [], ApiFilter = _filters, ErrorMessage = brands != null ? "" : "Error -1" });
            }
            return BadRequest();
        }

        [HttpPost("GetBrandsCount")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<int>>> GetBrandsCount([FromBody] ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var count = await brandService.GetBrandsCount(customer_id, _filters);
                return Ok(new BaseResponseModel<int>() { Success = count >= 0, Data = count, ApiFilter = _filters, ErrorMessage = count >= 0 ? "" : "Error -1" });
            }
            return BadRequest();
        }

        [HttpPut("SaveBrand")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Brand?>>> SaveBrand([FromBody] Brand _brand) {
            int customer_id;
            if (int.TryParse(_brand.customer_id.ToString(), out customer_id) && customer_id > 0) {
                Brand? brand = await brandService.SaveBrand(customer_id, _brand);
                if (brand != null) {
                    return Ok(new BaseResponseModel<Brand>() { Success = true, Data = brand, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Brand>() { Success = false, Data = null, ErrorMessage = "Error save brand" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpDelete("DeleteBrand")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<bool>>> DeleteBrand([FromBody] Brand _brand) {
            int customer_id;
            if (int.TryParse(_brand.customer_id.ToString(), out customer_id) && customer_id > 0) {
                bool result = await brandService.DeleteBrand(customer_id, _brand);
                return Ok(new BaseResponseModel<bool>() { Success = result, Data = result, ErrorMessage = result ? "" : "Error delete brand" });
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpGet("GetDefaultBrand")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Brand>>> GetDefaultBrand() {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var brand = await brandService.GetDefaultBrand(customer_id);
                return Ok(new BaseResponseModel<Brand>() { Success = brand != null, Data = brand, ErrorMessage = brand != null ? "" : "Error -1" });
            }
            return BadRequest();
        }
    }
}
