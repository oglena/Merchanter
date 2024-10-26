using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Merchanter.ServerService.Services {

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

            if( string.IsNullOrEmpty( request.Name ) || string.IsNullOrEmpty( request.Password ) ) {
                throw new ArgumentNullException( nameof( request ) );
            }

            var admin = merchanterService.helper.GetAdmin( request.AdminID, request.Name, request.Password );
            if( admin != null ) {
                var generatedTokenInformation = await tokenService.GenerateToken( new GenerateTokenRequest { AdminID = admin.id, Name = admin.name } );

                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
            }

            return response;
        }
    }
}
