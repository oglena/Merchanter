using MerchanterApp.ApiService.Classes;
using MerchanterApp.ApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ApiService.Services {

    public interface IAuthService {

        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
    }

    public class AuthService : IAuthService {
        readonly ITokenService tokenService;
        readonly MerchanterService merchanterService;

        public AuthService(MerchanterService merchanterService, ITokenService tokenService) {
            this.tokenService = tokenService;
            this.merchanterService = merchanterService;
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request) {
            UserLoginResponse response = new();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) {
                throw new ArgumentNullException(nameof(request));
            }

            var customer = merchanterService.helper.GetCustomerByMail(request.Email, request.Password);
            if (customer != null) {
                var generatedTokenInformation = await tokenService.GenerateToken(
                    new GenerateTokenRequest {
                        CustomerID = customer.customer_id,
                        Email = request.Email
                    });

                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
            }

            return response;
        }
    }
}
