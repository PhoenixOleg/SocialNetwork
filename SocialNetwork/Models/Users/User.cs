﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models.Users
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleName { get; set; } //Допускает Null - Отчества может не быть и оно не предусмотрено на форме регистрации

        public DateTime BirthDate { get; set; }

        public string Image { get; set; } = "https://thispersondoesnotexist.com";

        public string Status { get; set; }

        public string About { get; set; }

        [ProtectedPersonalData]
        [Required]
        public override string Email { get; set; }

        public string GetFullName()
        {
            //Если нет отчества заменяем двйной пробел одним. Иначе поиска не будет
            return (FirstName + " " + MiddleName + " " + LastName).Replace("  ", " ");
        }

        public User()
        {
            //Image = "https://via.placeholder.com/500";
            Status = "Ура! Я в соцсети!";
            About = "Информация обо мне.";
        }
    }
}
