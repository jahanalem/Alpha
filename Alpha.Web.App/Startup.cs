using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Infrastructure;
using Alpha.Infrastructure.Email;
using Alpha.LoggerService;
using Alpha.Models.Identity;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.Web.App.CustomTokenProviders;
using Alpha.Web.App.Extensions;
using Alpha.Web.App.GlobalErrorHandling.Extensions;
using Alpha.Web.App.Models;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.AppSettingsFileModel.EmailTemplates;
using Alpha.Web.App.Resources.Constants;
using Alpha.Web.App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.IO;

namespace Alpha.Web.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogManager.LoadConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // DbContext with pooling
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Alpha.DataAccess")));

            #region Repositories and Services

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ApplicationDbContext>();

            // Register repositories and services
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IArticleCategoryService, ArticleCategoryService>();
            services.AddTransient<IArticleTagService, ArticleTagService>();
            services.AddTransient<IContactUsService, ContactUsService>();
            services.AddTransient<IAboutUsService, AboutUsService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ITagService, TagService>();

            #endregion

            #region Configuration

            services.Configure<AppSettingsModel>(Configuration.GetSection("appSettings"));
            services.Configure<DomainAndUrlSettingsModel>(Configuration.GetSection("DomainAndUrlSettings"));
            services.Configure<EmailConfigurationSettingsModel>(Configuration.GetSection("EmailConfigurationSettings"));
            services.Configure<EmailTemplatesSettingsModel>(Configuration.GetSection("EmailTemplatesSettings"));

            #endregion

            #region Security, Authorization, Authentication

            services.AddIdentity<User, Role>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.RequireUniqueEmail = true;
                opts.SignIn.RequireConfirmedAccount = true; // Set to false to debug authentication issues
                opts.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

            services.Configure<EmailConfirmationTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(3));

            services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<User>, CustomUserValidator>();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(PolicyTypes.OrdinaryUsers, policy => policy.RequireRole("Users"));
                opts.AddPolicy(PolicyTypes.SuperAdmin, policy => policy.RequireRole("Admins"));
            });

            services.AddAuthentication().AddGoogle(opts =>
            {
                var googleAuthNSection = Configuration.GetSection("Authentication:Google");
                opts.ClientId = googleAuthNSection["ClientId"];
                opts.ClientSecret = googleAuthNSection["ClientSecret"];
                opts.SaveTokens = true;
            }).AddFacebook(opts =>
            {
                var facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                opts.AppId = facebookAuthNSection["AppId"];
                opts.AppSecret = facebookAuthNSection["AppSecret"];
                opts.SaveTokens = true;
            });

            #endregion

            #region Miscellaneous Services

            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddTransient<CurrentUserInformation>();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            logger.LogInfo("Starting Configure method in Startup.");

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".less"] = "text/plain" }
                }
            });

            app.UseRouting();

            logger.LogInfo("Before Forwarded Headers");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            logger.LogInfo("Before CORS Policy");
            app.UseCors("CorsPolicy");

            logger.LogInfo("Before Authentication");
            app.UseAuthentication(); // Authentication must be before Authorization
            app.UseAuthorization();

            logger.LogInfo("Before Custom Exception Middleware");
            app.ConfigureCustomExceptionMiddleware();

            ApplicationDbContext.CreateAdminAccount(serviceProvider, Configuration).Wait();
            SeedData.EnsurePopulated(app);

            logger.LogInfo("Adding Endpoints");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultRoute", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("fallbackRoute", "{controller}/{action}/{id?}");
            });

            logger.LogInfo("Configure method in Startup completed.");
        }
    }
}
