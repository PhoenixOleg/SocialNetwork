using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialNetwork.Controllers.Context;
using SocialNetwork.Models;

namespace SocialNetwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ������� WebApplicationBuilder
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // �������� ������ ����������� �� ����� ������������ appsettings.json
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            #region ���������� �������� � ���������

            // ��������� �������� ApplicationDbContext � �������� ������� � ����������
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            // ���������� ������ ������ � �������������� (��������� ���������� � �������� �������)
            builder.Services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services to the container. ???
            builder.Services.AddControllersWithViews();
            #endregion

            // ������� WebApplication
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //���������� ��������������
            app.UseAuthorization(); //���������� �����������


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
