using Application.Domain;
using Application.Services.Authentication;
using Application.Services.Documents;
using Application.Services.Email;
using Application.Services.Shared;
using Application.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace Application.Web
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
            services.AddControllersWithViews();

            EnforceDependancyInjection(services);

            ConfigureIdentity(services);

            //services.AddAuthentication("cookies").AddCookie("cookies", options => options.LoginPath = "/Home/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void EnforceDependancyInjection(IServiceCollection services)
        {
            //Authentication
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IAccountRecoveryService, AccountRecoveryService>();
            services.AddScoped<IPasswordChangeService, PasswordChangeService>();
            services.AddScoped<ILogoutService, LogoutService>();

            //Email
            services.AddScoped<IEmailGeneratorService, EmailGeneratorService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            //Documents
            services.AddScoped<IPdfGeneratorService<string>, PdfGeneratorFromHtmlService>();

            //Shared
            services.AddScoped<IRandomStringGeneratorService, RandomStringGeneratorService>();
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("authentication");
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationAuthenticationDbContext>(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";

                options.Lockout = new LockoutOptions()
                {
                    MaxFailedAccessAttempts = 5
                };
            });

            services.AddScoped<IUserStore<ApplicationUser>, UserOnlyStore<ApplicationUser, ApplicationAuthenticationDbContext>>();
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationAuthenticationDbContext>();
        }
    }
}
