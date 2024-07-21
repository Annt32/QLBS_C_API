using CSharp5Nhom2.Iservers;
using CSharp5Nhom2.Servers;
using API.Services; // Thêm dòng này để nhập TokenService
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CSharp5Nhom2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IUserServers, UserServers>();

            // Đăng ký TokenService
            builder.Services.AddSingleton<TokenService>();

            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromSeconds(600);
            });
            builder.Services.AddHttpContextAccessor();
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
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Login}");

            app.Run();
        }
    }
}
