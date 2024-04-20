using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Email", Prompt = "Введите адрес электронной почты")]
        public string EmailReg { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "День", Prompt = "День")]
        public int? Day { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Месяц", Prompt = "Месяц")]
        public int? Month { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Год", Prompt = "Год")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)] //@@@
        public string PasswordReg { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "Повторите пароль")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Никнейм", Prompt = "Никнейм")]
        public string Login { get; set; }
    }
}
