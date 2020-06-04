using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JNGH_IDENDITY.Data;

[assembly: HostingStartup(typeof(JNGH_IDENDITY.Areas.Identity.IdentityHostingStartup))]
namespace JNGH_IDENDITY.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.ConfigureApplicationCookie(options =>
                 {
                     options.Cookie.Name = "Cookie";
                     options.Cookie.HttpOnly = true;
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                     options.LoginPath = new PathString("/Identity/Account/Login");
                     options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                     options.SlidingExpiration = true;
                 });
            });
        }
    }
}