using System;
using System.Globalization;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

//for Azure AD
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using AspCoreFrame.Data;
using AspCoreFrame.Configuration;
using AspCoreFrame.Services;

using AutoMapper;

namespace AspCoreFrame.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            long appId = Configuration.GetValue<long>("AppId");
            string tenantId = Configuration.GetValue<string>("TenantId"); 
            int loginAttemptCount = Configuration.GetValue<int>("LoginAttemptCount");

            UsualConfig config = new UsualConfig()
            {
                AppId = appId,
                TenantId = tenantId
            };

            services.AddDbContext<DataContext>();            

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options =>
                {
                    Configuration.Bind("AzureAd", options);
                    //options.CookieSchemeName = IdentityConstants.ApplicationScheme;
                });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Lockout.MaxFailedAccessAttempts = loginAttemptCount;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            //Comment if using query string
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {new CultureInfo("en-MY"),new CultureInfo("zh-CN")};
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-MY");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpContextAccessor();

            //services.AddLogging();
            
            services.AddSingleton<IUsualConfig>(config);
            services.AddScoped<ICommonDataService, CommonDataService>();

            //limit the file size that the user can upload to 201MB.
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 210763776;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //Uncomment for query string testing.
            //var cultures = new List<CultureInfo>() {new CultureInfo("en-MY"),new CultureInfo("zh-CN")};
            //app.UseRequestLocalization(options => {
            //    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-MY");
            //    options.SupportedCultures = cultures;
            //    options.SupportedUICultures = cultures;
            //});

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            loggerFactory.AddLog4Net();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
