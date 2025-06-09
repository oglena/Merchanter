using ApiService.Classes;
using ApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Services {
    /// <summary>
    /// Defines methods for handling user authentication operations.
    /// </summary>
    /// <remarks>This interface provides functionality for authenticating users, including login operations.
    /// Implementations may include additional security measures, such as IP filtering or rate limiting.</remarks>
    public interface IAuthService {
        /// <summary>
        /// Authenticates a user based on the provided login request.
        /// </summary>
        /// <remarks>This method applies the <see cref="ClientIpCheckActionFilter"/> to validate the
        /// client's IP address before processing the login request. Ensure that the <paramref name="request"/> contains
        /// valid credentials and any required fields.</remarks>
        /// <param name="request">The login request containing the user's credentials and other required information. Must not be <see
        /// langword="null"/>.</param>
        /// <returns>A <see cref="UserLoginResponse"/> object containing the result of the authentication process, including user
        /// details and authentication status.</returns>
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
    }

    /// <summary>
    /// Provides authentication services, including user login functionality.
    /// </summary>
    /// <remarks>This service is responsible for handling user authentication by validating login credentials
    /// and generating authentication tokens. It interacts with the <see cref="MerchanterService"/> to retrieve customer
    /// information and settings, and with the <see cref="ITokenService"/> to generate tokens for authenticated
    /// users.</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </remarks>
    /// <param name="merchanterService">The service responsible for managing merchant-related operations.  This is required to perform
    /// authentication tasks related to merchants.</param>
    /// <param name="tokenService">The service responsible for generating and validating authentication tokens. This is required to handle
    /// token-based authentication.</param>
    public class AuthService(MerchanterService merchanterService, ITokenService tokenService) : IAuthService {
        readonly ITokenService tokenService = tokenService;
        readonly MerchanterService merchanterService = merchanterService;

        /// <summary>
        /// Authenticates a user based on the provided login request and returns authentication details.
        /// </summary>
        /// <remarks>This method validates the user's credentials and generates an authentication token
        /// upon successful login. The returned response includes user-specific settings and authentication details. The
        /// password field in the response is cleared for security.</remarks>
        /// <param name="request">The login request containing the user's email and password. Both fields must be non-null and non-empty.</param>
        /// <returns>A <see cref="UserLoginResponse"/> object containing authentication results, including an authentication
        /// token, token expiration date, and user-specific settings.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null or if the <c>Email</c> or <c>Password</c> properties of the
        /// request are null or empty.</exception>
        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request) {
            UserLoginResponse response = new();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) {
                throw new ArgumentNullException(nameof(request));
            }

            var customer = await merchanterService.Helper.GetCustomerByMail(request.Email, request.Password);

            if (customer != null) {
                if (int.TryParse(customer.customer_id.ToString(), out int customer_id)) {
                    var settings = await merchanterService.Helper.LoadSettings(customer_id);
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
