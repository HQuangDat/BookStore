using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using BookStore.Repositories;
using Serilog;
using Hangfire;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting Web Application");
                var builder = WebApplication.CreateBuilder(args);
                // Configure Serilog
                builder.Services.AddSerilog();

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                builder.Services.AddHangfire((options) =>
                    options.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
                builder.Services.AddHangfireServer();

                builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<IBookRepository, BookRepository>();
                builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
                builder.Services.AddScoped<ICartRepository, CartRepository>();

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                    .AddGoogle(options =>
                    {
                        options.ClientId = builder.Configuration.GetSection("GoogleKey:ClientId").Value;
                        options.ClientSecret = builder.Configuration.GetSection("GoogleKey:ClientSecret").Value;
                    })
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                        options.AccessDeniedPath = "/Account/AccessDenied";
                    });

                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                    options.AddPolicy("User", policy => policy.RequireRole("User"));
                    options.AddPolicy("Saler", policy => policy.RequireRole("Saler"));
                });


                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
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

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed!!!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
