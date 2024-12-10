using Bayanihand.App.Configuration;
using Bayanihand.App.Models.Repositories;
using AutoMapper;
using Bayanihand.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bayanihand.App.Models;

namespace Bayanihand.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Almirol"));
            });


            // AutoMapper service
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

            // Repository services
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IHandymanRepository, HandymanRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            // Configure the application cookie
            builder.Services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Account/SignIn";
                options.LogoutPath = "/Account/LogOut";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
                options.SlidingExpiration = true;
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Handyman", "Customer" };

                foreach (var r in roles)
                {
                    if (!await roleManager.RoleExistsAsync(r))
                        await roleManager.CreateAsync(new IdentityRole(r));
                }
            }

            app.Run();
        }
    }
}
