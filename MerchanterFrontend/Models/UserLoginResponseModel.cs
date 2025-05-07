using Merchanter.Classes;

namespace MerchanterFrontend.Models {
    public class UserLoginResponseModel {
        public bool AuthenticateResult { get; set; }
        public string AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public Customer CustomerInformation { get; set; }
    }
}
