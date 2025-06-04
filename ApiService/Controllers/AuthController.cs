using ApiService.Models;
using ApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers {

    /// <summary>
    /// Authentication endpoint for handling user login operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller {
        readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service used to handle login operations.</param>
        public AuthController(IAuthService authService) {
            this.authService = authService;
        }

        /// <summary>
        /// Handles user login requests.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>
        /// A <see cref="UserLoginResponse"/> object containing authentication results, tokens, and additional settings.
        /// </returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> LoginUserAsync([FromBody] UserLoginRequest request) {
            var result = await authService.LoginUserAsync(request);
            return result;
        }
    }
}
