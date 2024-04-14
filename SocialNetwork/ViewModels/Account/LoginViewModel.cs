namespace SocialNetwork.ViewModels.Account
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string? ReturnUrl { get; internal set; }
        public string Password { get; internal set; }
        public bool RememberMe { get; internal set; }
    }
}
