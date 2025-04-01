using MerchanterApp.ApiService.Classes;
using MerchanterApp.ApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ApiService.Services {

    public interface IAuthService {

        [ServiceFilter( typeof( ClientIpCheckActionFilter ) )]
        public Task<UserLoginResponse> LoginUserAsync( UserLoginRequest request );
    }

    public class AuthService :IAuthService {
        readonly ITokenService tokenService;
        readonly MerchanterService merchanterService;

        public AuthService( MerchanterService merchanterService, ITokenService tokenService ) {
            this.tokenService = tokenService;
            this.merchanterService = merchanterService;
        }

        public async Task<UserLoginResponse> LoginUserAsync( UserLoginRequest request ) {
            UserLoginResponse response = new();

            if( string.IsNullOrEmpty( request.Username ) || string.IsNullOrEmpty( request.Password ) ) {
                throw new ArgumentNullException( nameof( request ) );
            }

            var customer = merchanterService.helper.GetCustomer( request.Username, request.Password );
            if( customer != null ) {
                var generatedTokenInformation = await tokenService.GenerateToken( new GenerateTokenRequest { Username = request.Username, Password = request.Password } );

                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
            }

            return response;
        }
    }
}
