﻿using Merchanter.Classes;

namespace MerchanterApp.ApiService.Models {
    public class UserLoginResponse {
        public bool AuthenticateResult { get; set; }
        public string AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public Customer CustomerInformation { get; set; }
    }
}
