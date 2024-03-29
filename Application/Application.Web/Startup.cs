using Application.Data;
using Application.Data.Repositories;
using Application.Domain;
using Application.Domain.ApplicationEntities;
using Application.Services.Authentication;
using Application.Services.Forum;
using Application.Services.Forum.Filters;
using Application.Services.UserProfile;
using Application.Web.Models;
using Application.Web.RealTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sonya.AspNetCore.Common.Documents;
using Sonya.AspNetCore.Common.Email;
using Sonya.AspNetCore.Common.Files;
using Sonya.AspNetCore.Common.Filtering;
using Sonya.AspNetCore.Common.Shared;
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

            services.AddDbContext<ApplicationDbContext>();

            services.AddSignalR();

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
                endpoints.MapHub<ReactionsHub>("/reactionshub");
                endpoints.MapHub<PostsHub>("/postshub");
            });
        }

        private void EnforceDependancyInjection(IServiceCollection services)
        {
            services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false).AddRazorRuntimeCompilation();

            //Authentication
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IAccountRecoveryService, AccountRecoveryService>();
            services.AddScoped<IPasswordAssignmentService, PasswordSettingService>();
            services.AddScoped<ILogoutService, LogoutService>();

            //Email
            services.AddScoped<IEmailBuilder, EmailBuilder>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            //Documents
            services.AddScoped<IPdfBuilder<string>, HtmlToPdfBuilder>();

            //Files
            services.AddScoped<IFileDownloadService, FileDownloadService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IFileSizeRestrictionService, FileSizeRestrictionService>();
            services.AddScoped<IFileValidationService, FileValidationService>();
            services.AddScoped<IImageFileValidationService, ImageFileValidationService>();
            services.AddScoped<IImageUploadService, ImageUploadService>();
            services.AddScoped<IFileFilterBuilder, FileFilterBuilder>();

            //Filtering
            services.AddScoped<IFilterService<IFormFile>, FilterService<IFormFile>>();

            //Profile
            services.AddScoped<IUserProfileService, UserProfileService>();

            //Shared
            services.AddScoped<IRandomStringGeneratorService, RandomStringGeneratorService>();

            //Forum
            services.AddScoped<IThreadService, ThreadService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IThreadFilterBuilder, ThreadFilterBuilder>();
            services.AddScoped<IFilterService<Thread>, FilterService<Thread>>();

            //Repositories & Uit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IThreadRepository, ThreadRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
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
