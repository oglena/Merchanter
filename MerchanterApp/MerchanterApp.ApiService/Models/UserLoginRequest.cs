namespace MerchanterApp.ApiService.Models {
    public class UserLoginRequest {
        public int CustomerID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
