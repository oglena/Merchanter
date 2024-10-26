namespace Merchanter.ServerService.Models {
    public class UserLoginRequest {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
