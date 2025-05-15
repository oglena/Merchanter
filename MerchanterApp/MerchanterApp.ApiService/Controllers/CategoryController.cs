using Microsoft.AspNetCore.Mvc;
using Merchanter.Classes;
using Microsoft.AspNetCore.Authorization;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;

namespace MerchanterApp.ApiService.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase {
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

        [HttpPut("SaveCategory")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Category?>>> SaveCategory([FromBody] Category _category) {
            int customer_id;
            if (int.TryParse(_category.customer_id.ToString(), out customer_id) && customer_id > 0) {
                Category? category = await categoryService.SaveCategory(customer_id, _category);
                if (category != null) {
                    return Ok(new BaseResponseModel<Category>() { Success = true, Data = category, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<Category>() { Success = false, Data = null, ErrorMessage = "Error save category" });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

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
