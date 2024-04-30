using AutoMapper;
using AwesomeNetwork.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialNetwork.Data.Repository;
using SocialNetwork.Models.Context;
using SocialNetwork.Models.Users;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;

namespace SocialNetwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Создаем WebApplicationBuilder
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Получаем строку подключения из файла конфигурации appsettings.json
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            #region Добавление сервисов в контейнер

            // Добавляем контекст ApplicationDbContext в качестве сервиса в приложение
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            builder.Services
                .AddUnitOfWork()
                .AddCustomRepository<Friend, FriendsRepository>()
                .AddCustomRepository<Message, MessageRepository>();

            // Добавление модели работы с пользователями (установка требований к политике паролей)
            builder.Services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })               
                .AddEntityFrameworkStores<ApplicationDbContext>();              

            #region Подключаем автомаппинг (заменен на более понятный из модуля 33)
            //Закоменченный код из модуля 34
            //var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            //builder.Services.AddAutoMapper(assembly);

            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new MappingProfile()); //Добавляем в профиль конфигурации класс с описанием маппинга объектов
            });

            IMapper mapper = mapperConfig.CreateMapper();

            builder.Services.AddSingleton(mapper);
            #endregion Подключаем автомаппинг

            // Add services to the container. ???
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            #endregion

            // Создаем WebApplication
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            var cachePeriod = "0";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            app.UseRouting();

            app.UseAuthentication(); //Добавление аутентификации
            app.UseAuthorization(); //Добавление авторизации


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"); //Обычный маршрут по умолчанию -> контроллер Home, действие Index

            app.Run();
        }
    }
}
