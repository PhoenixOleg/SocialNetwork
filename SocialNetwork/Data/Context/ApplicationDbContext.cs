using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Configs;
using SocialNetwork.Models.Users;

namespace SocialNetwork.Models.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Так как у меня логин именно Email, а не UserName, то делаем его уникальным
            builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            //builder
                //.ApplyConfiguration<Friend>(new FriendConfiguration())
                //.ApplyConfiguration<Message>(new MessageConfiguration());

            builder
                .ApplyConfiguration(new FriendConfiguration())
                .ApplyConfiguration(new MessageConfiguration());
        }
    }
}
