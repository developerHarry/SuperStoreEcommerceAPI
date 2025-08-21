namespace SuperStoreEcommerceAPI.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
