using Merchanter.Classes;
using Merchanter.Classes.Settings;

namespace MerchanterFrontend.Models {
    public class UserLoginResponseModel {
        public bool AuthenticateResult { get; set; }
        public string AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public SettingsMerchanter Settings { get; set; }
    }
}