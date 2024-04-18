using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите email")]
        public string Email { get; set; }

        public string? ReturnUrl { get; internal set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string Password { get; internal set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; internal set; }
    }
}
