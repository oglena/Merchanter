using Merchanter.Classes;

namespace Merchanter.ServerService.Models {
    public class UserLoginResponse {
        public bool AuthenticateResult { get; set; }
        public string? AuthToken { get; set; } = null;
        public DateTime AccessTokenExpireDate { get; set; }
        public Admin? AdminInformation { get; set; } = null;
    }
}
