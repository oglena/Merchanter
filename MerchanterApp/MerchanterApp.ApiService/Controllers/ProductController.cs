using Merchanter.Classes;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MerchanterApp.ApiService.Controllers {
    /// <summary>
    /// Provides product-related API endpoints for Merchanter.
    /// </summary>
    [Route("api/[controller]")]
    [SwaggerTag("Provides product-related API endpoints for Merchanter.")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase {

        /// <summary>
        /// Returns a filtered and paginated list of products for the authenticated customer.
        /// </summary>
        /// <param name="_filters">Filtering, sorting, and paging options.</param>
        /// <returns>List of products wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetProducts")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<Product>>>> GetProducts([FromBody] ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var products = await productService.GetProducts(customer_id, _filters);
                _filters.TotalCount = await productService.GetProductsCount(customer_id, _filters);
                return Ok(new BaseResponseModel<List<Product>>() { Success = products != null, Data = products ?? [], ApiFilter = _filters, ErrorMessage = products != null ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Returns the total count of products matching the given filters for the authenticated customer.
        /// </summary>
        /// <param name="_filters">Filtering options.</param>
        /// <returns>Total product count wrapped in a BaseResponseModel.</returns>
        [HttpPost("GetProductsCount")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<int>>> GetProductsCount([FromBody] ApiFilter _filters) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                var count = await productService.GetProductsCount(customer_id, _filters);
                return Ok(new BaseResponseModel<int>() { Success = count >= 0, Data = count, ApiFilter = _filters, ErrorMessage = count >= 0 ? "" : "Error -1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Retrieves a single product by its ID for the authenticated customer.
        /// </summary>
        /// <param name="PID">Product ID as string.</param>
        /// <returns>Product details wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetProduct/{PID}")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Product?>>> GetProduct(string PID) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                if (int.TryParse(PID, out int product_id) && product_id > 0) {
                    var product = await productService.GetProduct(customer_id, product_id);

                    return Ok(new BaseResponseModel<Product?>() { Success = product != null, Data = product ?? null, ErrorMessage = product != null ? "" : "Error -1", ApiFilter = null });
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates an existing product for the authenticated customer.
        /// </summary>
        /// <param name="product">Product object to update.</param>
        /// <returns>Updated product wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveProduct")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<Product?>>> SaveProduct([FromBody] Product product) {
            int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id);
            if (customer_id > 0) {
                if (product != null && product.id > 0) {
                    var updatedProduct = await productService.SaveProduct(customer_id, product);
                    return Ok(new BaseResponseModel<Product?>() { Success = updatedProduct != null, Data = updatedProduct ?? null, ErrorMessage = updatedProduct != null ? "" : "Error -1", ApiFilter = null });
                }
            }
            return BadRequest();
        }
    }
}
