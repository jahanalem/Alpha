using System;
using System.Threading.Tasks;
using Alpha.Models;
using Alpha.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<AboutUs> AboutUs { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleLike> ArticleLikes { get; set; }
        public virtual DbSet<ArticleTag> ArticleTags { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentLike> CommentLikes { get; set; }
        //public virtual DbSet<CommentReply> CommentReplies { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectState> ProjectStates { get; set; }
        public virtual DbSet<ProjectTag> ProjectTags { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleClaim>(builder =>
            {
                builder.HasOne(roleClaim => roleClaim.Role).WithMany(role => role.Claims).HasForeignKey(roleClaim => roleClaim.RoleId);
                builder.ToTable("RoleClaim");
            });
            modelBuilder.Entity<Role>(builder =>
            {
                builder.ToTable("Role");
            });
            modelBuilder.Entity<UserClaim>(builder =>
            {
                builder.HasOne(userClaim => userClaim.User).WithMany(user => user.Claims).HasForeignKey(userClaim => userClaim.UserId);
                builder.ToTable("UserClaim");
            });
            modelBuilder.Entity<UserLogin>(builder =>
            {
                builder.HasOne(userLogin => userLogin.User).WithMany(user => user.Logins).HasForeignKey(userLogin => userLogin.UserId);
                builder.ToTable("UserLogin");
            });
            
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("User"); //.HasMany(e => e.Comments).WithOne().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<UserRole>(builder =>
            {
                builder.HasOne(userRole => userRole.Role).WithMany(role => role.Users).HasForeignKey(userRole => userRole.RoleId);
                builder.HasOne(userRole => userRole.User).WithMany(user => user.Roles).HasForeignKey(userRole => userRole.UserId);
                builder.ToTable("UserRole");
            });
            modelBuilder.Entity<UserToken>(builder =>
            {
                builder.HasOne(userToken => userToken.User).WithMany(user => user.UserTokens).HasForeignKey(userToken => userToken.UserId);
                builder.ToTable("UserToken");
            });

            modelBuilder.Entity<AboutUs>().ToTable("AboutUs");

            modelBuilder.Entity<Article>().ToTable("Article")
                .HasMany(a=>a.Comments)
                .WithOne(a=>a.Article)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArticleLike>().ToTable("ArticleLike");
            modelBuilder.Entity<ArticleTag>().ToTable("ArticleTag");
            modelBuilder.Entity<AttachmentFile>().ToTable("AttachmentFile");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<CommentLike>().ToTable("CommentLike");
            //modelBuilder.Entity<CommentReply>().ToTable("CommentReply");
            modelBuilder.Entity<ContactUs>().ToTable("ContactUs");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<ProjectState>().ToTable("ProjectState");
            modelBuilder.Entity<ProjectTag>().ToTable("ProjectTag");
            modelBuilder.Entity<Rating>().ToTable("Rating");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<ArticleCategory>().ToTable("ArticleCategory").HasMany(e => e.Articles).WithOne().OnDelete(DeleteBehavior.Cascade);
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<Role> roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            string userName = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(userName) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new Role(role));
                }

                User user = new User
                {
                    Email = email,
                    UserName = userName,
                    IsActive = true,
                    FirstName = "Said Roohullah",
                    LastName = "Allem",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, password);
                if (result.IsCompletedSuccessfully)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
