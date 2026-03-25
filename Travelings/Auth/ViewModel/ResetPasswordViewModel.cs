namespace Travelings.Auth.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string email { get; set; } = string.Empty;
        public string tempPassword { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;
    }
}
