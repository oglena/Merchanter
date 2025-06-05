using Microsoft.AspNetCore.Mvc;
using Merchanter.Classes;
using Microsoft.AspNetCore.Authorization;
using ApiService.Models;
using ApiService.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiService.Controllers {
    /// <summary>
    /// Brand Endpoint for managing brands in Merchanter.
    /// </summary>
    [ApiController]
    [SwaggerTag("Brand endpoint for Merchanter.")]
    [Route("api/[controller]")]
    public class BrandController(IBrandService brandService, ICatalogService catalogService) : ControllerBase {
        /// <summary>
        /// Returns a filtered and paginated list of brands for the authenticated customer.
        /// </summary>
        /// <param name="_filters">Filtering, sorting, and paging options. Returns with extended query properties.</param>
        /// <returns>List of brands wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetBrands")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Brand>>>> GetBrands([FromBody] ApiFilter? _filters) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                _filters ??= new ApiFilter() { Filters = null, Sort = null, Pager = null };
                _filters.ExtendedQueryResponses = await catalogService.GetExtendedQuery(customer_id, _filters, typeof(Brand));
                var brands = await brandService.GetBrands(customer_id, _filters);
                return Ok(new BaseResponseModel<List<Brand>>() { Success = brands != null, Data = brands ?? [], ApiFilter = _filters, ErrorMessage = brands != null ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Returns the total count of brands matching the given filters for the authenticated customer.
        /// </summary>
        /// <param name="_filters">Filtering options.</param>
        /// <returns>Total brand count wrapped in a BaseResponseModel.</returns>
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

        /// <summary>
        /// Updates or creates a brand for the authenticated customer.
        /// </summary>
        /// <param name="_brand">Brand object to save.</param>
        /// <returns>Saved brand wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveBrand")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Brand?>>> SaveBrand([FromBody] Brand _brand) {
            int customer_id;
            if (int.TryParse(_brand.customer_id.ToString(), out customer_id) && customer_id > 0) {
                Brand? brand = await brandService.SaveBrand(customer_id, _brand);
                if (brand != null) {
                    return Ok(new BaseResponseModel<Brand?>() { Success = true, Data = brand, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Brand?>() { Success = false, Data = null, ErrorMessage = "Error save brand" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Deletes a brand for the authenticated customer.
        /// </summary>
        /// <param name="_brand">Brand object to delete.</param>
        /// <returns>Result of the delete operation wrapped in a BaseResponseModel.</returns>
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

        /// <summary>
        /// Retrieves default brand for authenticated customer.
        /// </summary>
        /// <returns>Default brand wrapped in a BaseResponseModel.</returns>
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
