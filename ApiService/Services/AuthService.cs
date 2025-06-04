using ApiService.Classes;
using ApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Services {

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
                if (int.TryParse(customer.customer_id.ToString(), out int customer_id)) {
                    var settings = merchanterService.helper.LoadSettings(customer_id);
                    var generatedTokenInformation = await tokenService.GenerateToken(
                        new GenerateTokenRequest {
                            CustomerID = customer_id,
                            Email = customer.email
                        });
                    customer.password = null;
                    response.AuthenticateResult = true;
                    response.AuthToken = generatedTokenInformation.Token;
                    response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
                    //response.CustomerInformation = customer;
                    response.Settings = settings;
                    response.Settings.customer.password = null;
                }
            }

            return response;
        }
    }
}
