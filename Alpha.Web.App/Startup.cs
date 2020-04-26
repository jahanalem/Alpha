using System;
using System.IO;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Infrastructure;
using Alpha.Models.Identity;
using Alpha.Services;
using Alpha.Infrastructure.Email;
using Alpha.LoggerService;
using Alpha.Web.App.CustomTokenProviders;
using Alpha.Web.App.GlobalErrorHandling.Extensions;
using Alpha.Web.App.Resources.Constants;
using Alpha.Web.App.Services;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Web.App.Resources.AppSettingsFileModel.EmailTemplates;
using Alpha.Web.App.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;
using Alpha.Web.App.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Alpha.ViewModels;
using Alpha.ViewModels.Interfaces;
using Newtonsoft.Json;

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

            //services.ConfigureLoggerService();
            services.AddControllersWithViews();
            services.AddRazorPages();//.AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

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
                opts.ClientId = "56074703213-u4qak3nim2ejjvdd23euf68e724qn4a7.apps.googleusercontent.com";
                opts.ClientSecret = "NDEmt2-XKC30D09lvll4XrW6";

                opts.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                opts.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");

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
                opts.AppId = "520361115549717";
                opts.AppSecret = "32fb5d66afb9042b900c69a5c65d5474";

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

            #region appsettings.json file

            services.Configure<AppSettingsModel>(Configuration.GetSection("appSettings"));
            services.Configure<DomainAndUrlSettingsModel>(Configuration.GetSection("DomainAndUrlSettings"));
            services.Configure<EmailConfigurationSettingsModel>(Configuration.GetSection("EmailConfigurationSettings"));
            //EmailTemplatesSettingsModel
            services.Configure<EmailTemplatesSettingsModel>(Configuration.GetSection("EmailTemplatesSettings"));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            ILoggerManager logger)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseAuthentication();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".less"] = "plain/text";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            ApplicationDbContext.CreateAdminAccount(serviceProvider, Configuration).Wait();//app.ApplicationServices

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.ConfigureExceptionHandler(logger);
            app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areasDefault", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(name: "articleformat", pattern: "{controller=Article}/{action}/tagId/{tagId?}/pageNumber/{pageNumber}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "normal", pattern: "{controller}/{action}/{id?}");
                //endpoints.MapAreaControllerRoute("areasDefault","Admin", "{controller=Home}/{action=Index}/{id?}");
            });
            //SeedData.EnsurePopulated(app);
        }
    }
}