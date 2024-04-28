using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Models.Users;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace SocialNetwork.Data
{
    public class GenetateUsers
    {
        private readonly string[] maleNames = new string[] { "Иван", "Петр", "Сидор", "Александр", "Алексей", "Дмитрий", "Евгений", "Кирилл", "Сергей", "Василий", "Андрей", "Валерий", "Олег", "Борис" };
        private readonly string[] femaleNames = new string[] { "Мария", "Ирина", "Наталья", "Наталия", "Александра", "Вероника", "Евгения", "Вера", "Юлия", "Василиса", "Анастасия", "Валерия", "Галина", "Марина" };
        private readonly string[] lastNames = new string[] { "Иванов", "Петров", "Сидоров", "Анисимов", "Попов", "Титов", "Королев", "Григорьев" };

        private enum Gender : int
        {
            female = 1,
            male
        }

        public List<User> Populate(int count)
        {
            //string gender;
            string firstName;
            string lastName;
            string? middleName;

            var users = new List<User>();


            for (int i = 1; i < count; i++)
            {
                var rnd = new Random((int)(DateTime.Now.Ticks % 10000));

                var gender = (int)Gender.male == (rnd.Next(1, 1000) % 2) + 1 ? Gender.male : Gender.female;

                lastName = lastNames[rnd.Next(0, lastNames.Length - 1)];
                middleName = GetBaseMiddleName(rnd);


                if (gender == Gender.male)
                {
                    firstName = maleNames[rnd.Next(0, maleNames.Length - 1)];

                    if (!middleName.IsNullOrEmpty())
                    {

                        middleName = string.Concat(middleName, "вич");
                    }
                }
                else
                {
                    firstName = femaleNames[rnd.Next(0, femaleNames.Length - 1)];
                    lastName = string.Concat(lastName, "а");

                    if (!middleName.IsNullOrEmpty())
                    {
                        middleName = string.Concat(middleName, "вна");
                    }
                }

                users.Add(new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    BirthDate = DateTime.Now.AddDays(-rnd.Next(1, (DateTime.Now - DateTime.Now.AddYears(-50)).Days)),
                    Email = string.Concat("test", rnd.Next(0, 1204), "@test.com"),
                    UserName = string.Concat("test", rnd.Next(0, 1204)),
                    Image = "https://thispersondoesnotexist.com",
                });
            }

            return users;
        }

        private string? GetBaseMiddleName(System.Random rnd)
        {
            //var rnd = new Random(DateTime.Now.Millisecond);
            string? middleName = null;

            if ((rnd.Next(1, 10) % 2) == 0)
            {
                return null;
            }
            else
            {
                middleName = maleNames[rnd.Next(0, maleNames.Length - 1)];


                if (middleName.Substring(middleName.Length - 2) == "ий")
                {
                    if (middleName == "Дмитрий")
                    {
                        middleName = middleName.Replace("ий", "ие");
                    }
                    else
                    {
                        middleName = middleName.Replace("ий", "ье");
                    }                    
                }
                else
                {
                    if (middleName.Substring(middleName.Length - 1) == "й")
                    {
                        middleName = middleName.Replace("ей", "eе");
                    }
                    else
                    {
                        middleName = string.Concat(middleName, "о");
                    }
                }

                return middleName;
            }
        }
    }
}
