using ApiService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ApiService.Services {
    /// <summary>
    /// Defines a contract for generating tokens based on the provided request.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for creating tokens  according to the
    /// specifications in the <see cref="GenerateTokenRequest"/>.</remarks>
    public interface ITokenService {
        /// <summary>
        /// Generates a token based on the provided request parameters.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="request"/> object is properly populated with  all
        /// required fields before calling this method. The specific requirements for  the request object depend on the
        /// implementation of the token generation process.</remarks>
        /// <param name="request">The request object containing the parameters required to generate the token.  This cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains  a <see
        /// cref="GenerateTokenResponse"/> object with the generated token and related metadata.</returns>
        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request);
    }

    /// <summary>
    /// Provides functionality for generating JSON Web Tokens (JWTs) with user-specific claims.
    /// </summary>
    /// <remarks>This service generates JWTs that include user-specific claims such as email and customer ID. 
    /// The tokens are signed using the HMAC-SHA256 algorithm and are configured with expiration and  audience/issuer
    /// details based on application settings.</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </remarks>
    /// <param name="configuration">The application configuration settings used to initialize the service.  This parameter cannot be <see
    /// langword="null"/>.</param>
    public class TokenService(IConfiguration configuration) : ITokenService {
        readonly IConfiguration configuration = configuration;

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user, containing their email and customer ID as claims.
        /// </summary>
        /// <remarks>The generated token is signed using the HMAC-SHA256 algorithm and includes the email
        /// and customer ID as claims. The token is valid for 660 minutes from the time of generation.</remarks>
        /// <param name="request">The request object containing the user's email and customer ID. Both values must be provided.</param>
        /// <returns>A <see cref="GenerateTokenResponse"/> containing the generated JWT and its expiration date.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="request"/> is null, or if the <see cref="GenerateTokenRequest.Email"/> is null or
        /// empty, or if <see cref="GenerateTokenRequest.CustomerID"/> is less than or equal to 0.</exception>
        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request) {
            string? secret = configuration["AppSettings:Secret"];
            if (string.IsNullOrEmpty(secret)) {
                throw new InvalidOperationException("AppSettings:Secret is not configured.");
            }

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.ASCII.GetBytes(secret));

            var dateTimeNow = DateTime.UtcNow;
            if (string.IsNullOrEmpty(request.Email) || request.CustomerID <= 0) {
                throw new ArgumentException("Email and CustomerID must be provided to generate a token.");
            }
            JwtSecurityToken jwt = new(
                    issuer: configuration["AppSettings:ValidIssuer"],
                    audience: configuration["AppSettings:ValidAudience"],
                    claims: [
                        new("email", request.Email),
                        new("customerId", request.CustomerID.ToString())
                    ],
                    notBefore: dateTimeNow,
                    expires: dateTimeNow.Add(TimeSpan.FromMinutes(660)),
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new GenerateTokenResponse {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpireDate = dateTimeNow.Add(TimeSpan.FromMinutes(660))
            });
        }
    }
}
