namespace Merchanter.Responses {
    public record class ENT_TokenResponse {
        public bool authenticateResult { get; set; }
        public string authToken { get; set; }
        public DateTime accessTokenExpireDate { get; set; }
    }
}