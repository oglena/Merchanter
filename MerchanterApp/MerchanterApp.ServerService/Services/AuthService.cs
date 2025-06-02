using MerchanterApp.ServerService.Classes;
using MerchanterApp.ServerService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ServerService.Services {

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

            if( string.IsNullOrEmpty( request.UserName ) || string.IsNullOrEmpty( request.Password ) ) {
                throw new ArgumentNullException( nameof( request ) );
            }

            var admin = merchanterService.helper.GetAdmin( request.UserName, request.Password );
            if( admin != null ) {
                var generatedTokenInformation = await tokenService.GenerateToken( new GenerateTokenRequest { AdminID = admin.id, Name = admin.name } );

                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
                response.AdminInformation = admin;
            }

            return response;
        }
    }
}
