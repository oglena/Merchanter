using Merchanter.Classes;
using Merchanter.Classes.Settings;

namespace ApiService.Models {
    public class UserLoginResponse {
        public bool AuthenticateResult { get; set; } = false;
        public string AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public SettingsMerchanter Settings { get; set; }
    }
}
