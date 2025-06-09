using Exam.Repository;
using Exam.Repository.Implementation.User;
using Exam.Repository.Interfaces.User;
using Exam.Services.Implementation.Authentication;
using Exam.Services.Implementation.User;
using Exam.Services.Interfaces.Authentication;
using Exam.Services.Interfaces.User;

namespace Exam.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            // Initialize connection factory
            ConnectionFactory.Initialize(builder.Configuration.GetConnectionString("DefaultConnection"));

            // Add session support
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
