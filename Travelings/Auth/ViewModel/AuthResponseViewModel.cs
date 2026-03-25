namespace Travelings.Auth.ViewModel
{
    public class AuthResponseViewModel
    {
        public int userId { get; set; }
        public string email { get; set; } = string.Empty;
        public string nome { get; set; } = string.Empty;
        public string accessToken { get; set; } = string.Empty;
        public DateTime accessTokenExpiresAtUtc { get; set; }
        public string refreshToken { get; set; } = string.Empty;
    }
}
