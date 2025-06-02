using MerchanterApp.ServerService.Models;
using MerchanterApp.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ServerService.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController :Controller {
        readonly IAuthService authService;

        public AuthController( IAuthService authService ) {
            this.authService = authService;
        }

        [HttpPost( "Login" )]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> LoginUserAsync( [FromBody] UserLoginRequest request ) {
            var result = await authService.LoginUserAsync( request );

            return result;
        }
    }
}
