using Microsoft.AspNetCore.Mvc;
using Merchanter.Classes;
using Microsoft.AspNetCore.Authorization;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MerchanterApp.ApiService.Controllers {
    /// <summary>
    /// Provides category-related API endpoints for Merchanter.
    /// </summary>
    [ApiController]
    [SwaggerTag("Provides category-related API endpoints for Merchanter.")]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase {

        /// <summary>
        /// Returns a filtered and paginated list of categories for the authenticated customer.
        /// </summary>
        /// <param name="_filters">Filtering, sorting, and paging options.</param>
        /// <returns>List of categories wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetCategories")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Category>>>> GetCategories([FromBody] ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var categories = await categoryService.GetCategories(customer_id, _filters);
                _filters.TotalCount = await categoryService.GetCategoriesCount(customer_id, _filters);
                return Ok(new BaseResponseModel<List<Category>>() { Success = categories != null, Data = categories ?? [], ApiFilter = _filters, ErrorMessage = categories != null ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Returns the total count of categories matching the given filters for the authenticated customer.
        /// </summary>
        /// <param name="_filters">Filtering options.</param>
        /// <returns>Total category count wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetCategoriesCount")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<int>>> GetCategoriesCount([FromBody] ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var count = await categoryService.GetCategoriesCount(customer_id, _filters);
                return Ok(new BaseResponseModel<int>() { Success = count >= 0, Data = count, ApiFilter = _filters, ErrorMessage = count >= 0 ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates or creates a category for the authenticated customer.
        /// </summary>
        /// <param name="_category">Category object to save.</param>
        /// <returns>Saved category wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveCategory")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Category?>>> SaveCategory([FromBody] Category _category) {
            int customer_id;
            if (int.TryParse(_category.customer_id.ToString(), out customer_id) && customer_id > 0) {
                Category? category = await categoryService.SaveCategory(customer_id, _category);
                if (category != null) {
                    return Ok(new BaseResponseModel<Category?>() { Success = true, Data = category, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Category?>() { Success = false, Data = null, ErrorMessage = "Error save category" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Deletes a category for the authenticated customer.
        /// </summary>
        /// <param name="_category">Category object to delete.</param>
        /// <returns>Result of the delete operation wrapped in a BaseResponseModel.</returns>
        [HttpDelete("DeleteCategory")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<bool>>> DeleteCategory([FromBody] Category _category) {
            int customer_id;
            if (int.TryParse(_category.customer_id.ToString(), out customer_id) && customer_id > 0) {
                bool result = await categoryService.DeleteCategory(customer_id, _category);
                return Ok(new BaseResponseModel<bool>() { Success = result, Data = result, ErrorMessage = result ? "" : "Error delete category" });
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Retrieves a single category by its ID for the authenticated customer.
        /// </summary>
        /// <param name="CID">Category ID.</param>
        /// <returns>Category details wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetCategory/{CID}")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Category?>>> GetCategory(int CID) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var category = await categoryService.GetCategory(customer_id, CID);
                return Ok(new BaseResponseModel<Category>() { Success = category != null, Data = category, ErrorMessage = category != null ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Retrieves the default category for the authenticated customer.
        /// </summary>
        /// <returns>Default category wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetDefaultCategory")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Category>>> GetDefaultCategory() {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var category = await categoryService.GetDefaultCategory(customer_id);
                return Ok(new BaseResponseModel<Category>() { Success = category != null, Data = category, ErrorMessage = category != null ? "" : "Error -1" });
            }
            return BadRequest();
        }
    }
}
