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
using Microsoft.AspNetCore.Authentication;
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Web.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();

            services.AddMvc();//.AddRazorRuntimeCompilation();

            //services.ConfigureLoggerService();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //https://neelbhatt.com/2018/02/27/use-dbcontextpooling-to-improve-the-performance-net-core-2-1-feature/
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"],
                b => b.MigrationsAssembly("Alpha.DataAccess")));

            //services.AddDbContextPool<ApplicationDbContext>(options =>
            //    options.UseMySql(Configuration["ConnectionStrings:DefaultConnection"],
            //    b => b.MigrationsAssembly("Alpha.DataAccess")));

            #region Repositories and Services

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();

            services.AddTransient<IAboutUsRepository, AboutUsRepository>();
            services.AddTransient<IAboutUsService, AboutUsService>();

            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IArticleService, ArticleService>();

            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ITagService, TagService>();

            services.AddTransient<IArticleTagRepository, ArticleTagRepository>();
            services.AddTransient<IArticleTagService, ArticleTagService>();

            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentService, CommentService>();

            services.AddTransient<IContactUsRepository, ContactUsRepository>();
            services.AddTransient<IContactUsService, ContactUsService>();

            services.AddTransient<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddTransient<IArticleCategoryService, ArticleCategoryService>();

            #endregion

            #region appsettings.json file

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
                opts.SignIn.RequireConfirmedAccount = true;
                opts.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

                //opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));
            services.Configure<EmailConfirmationTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(3));

            services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<User>, CustomUserValidator>();

            // https://stackoverflow.com/questions/55666826/where-did-imvcbuilder-addjsonoptions-go-in-net-core-3-0
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(PolicyTypes.OrdinaryUsers, policy => policy.RequireRole("Users"));
                opts.AddPolicy(PolicyTypes.SuperAdmin, policy => policy.RequireRole("Admins"));
            });
            services.AddAuthentication().AddGoogle(opts =>
            {
                IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");

                opts.ClientId = googleAuthNSection["ClientId"];
                opts.ClientSecret = googleAuthNSection["ClientSecret"];

                opts.ClaimActions.MapJsonKey("picture", "picture", "url");
                opts.ClaimActions.MapJsonKey("locale", "locale", "string");

                opts.SaveTokens = true;

                opts.Events.OnCreatingTicket = ctx =>
                {
                    List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                    tokens.Add(new AuthenticationToken()
                    {
                        Name = "TicketCreated",
                        Value = DateTime.UtcNow.ToString()
                    });

                    ctx.Properties.StoreTokens(tokens);

                    return Task.CompletedTask;
                };

            }).AddFacebook(opts =>
            {
                IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                opts.AppId = facebookAuthNSection["AppId"];
                opts.AppSecret = facebookAuthNSection["AppSecret"];

                opts.ClaimActions.MapJsonKey("picture", "picture", "url");
                opts.ClaimActions.MapJsonKey("locale", "locale", "string");

                opts.SaveTokens = true;

                opts.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });
            //    .AddTwitter(opts =>
            //{
            //    opts.ConsumerKey = "";
            //    opts.ConsumerSecret = "";
            //});
            //?? services.AddSingleton<RazorTemplateEngine, CustomTemplateEngine>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            #endregion

            #region UrlHelper, Email, Logger, CurrenUser, 

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                                IWebHostEnvironment env,
                                IServiceProvider serviceProvider,
                                ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                });
            }

            // Core middleware setup
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".less"] = "plain/text" }
                }
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            // Custom exception handling
            app.ConfigureCustomExceptionMiddleware();

            // Create admin account and seed data
            ApplicationDbContext.CreateAdminAccount(serviceProvider, Configuration).Wait();
            SeedData.EnsurePopulated(app);

            // Endpoint routing
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultRoute", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("fallbackRoute", "{controller}/{action}/{id?}");
            });
        }
    }
}