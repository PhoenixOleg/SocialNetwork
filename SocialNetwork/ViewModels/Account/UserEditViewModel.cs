using SocialNetwork.Models.Users;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.ViewModels.Account
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        [ReadOnly(true)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Отчество", Prompt = "Введите отчество")]
        public string? MiddleName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "email@example.com")]
        public string Email { get; set; }

        //Так как начал писать, что Никнейм - поле не дублирующее емэйл, то пока так и оставляю. Ну пока не обломаюсь на реализации
        //public string UserName => Email;
        [Required(ErrorMessage = "Поле \"{0}\" обязательно для заполнения")]
        [Display(Name = "Никнейм", Prompt = "Никнейм")]
        public string Login { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Фото", Prompt = "Укажите ссылку на картинку")]
        public string Image { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Статус", Prompt = "Введите статус")]
        public string Status { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "О себе", Prompt = "Введите данные о себе")]
        public string About { get; set; }
    }
}
