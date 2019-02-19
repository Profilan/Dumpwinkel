using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Dumpwinkel.Web
{
    public static class MyAuthentication
    {
        public const String ApplicationCookie = "DumpwinkelAuthenticationType";
    }

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // need to add UserManager into owin, because this is used in cookie invalidation
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = MyAuthentication.ApplicationCookie,
                LoginPath = new PathString("/cmseek/Account/Login"),
                Provider = new CookieAuthenticationProvider(),
                CookieName = "Dumpwinkel",
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromHours(12), // adjust to your needs
            });
        }
    }
}