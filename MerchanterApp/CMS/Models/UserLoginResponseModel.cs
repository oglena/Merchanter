﻿using Merchanter.Classes;

namespace CMS.Models {
    public class UserLoginResponseModel {
        public bool AuthenticateResult { get; set; }
        public string AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public Admin AdminInformation { get; set; }
    }
}
