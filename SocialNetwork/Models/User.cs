using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleName { get; set; } //Допускает Null - Отчества может не быть и оно не предусмотрено на форме регистрации

        public DateTime BirthDate { get; set; }

    //@@@ Тут должен быть город - его ID
    }
}
