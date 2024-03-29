﻿using Fusion.Web.Services;
using Fusion.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

namespace Fusion.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IProductService, ProductService>();
            services.AddHttpClient<ICartService, CartService>();
            services.AddHttpClient<ICouponService, CouponService>();

            SD.ProductAPIBase = Configuration["ServiceUrls:ProductAPI"];
            SD.ShoppingCartAPIBase = Configuration["ServiceUrls:ShoppingCartAPI"];
            SD.CouponAPIBase = Configuration["ServiceUrls:CouponAPI"];

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICouponService, CouponService>();

            services.AddControllersWithViews();

            services.AddAuthentication(options => {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", c=>c.ExpireTimeSpan=TimeSpan.FromMinutes(10))
                .AddOpenIdConnect("oidc", options => { 
                    options.Authority = Configuration["ServiceUrls:IdentityAPI"];
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClientId = "fusion";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ClaimActions.MapJsonKey("role", "role", "role");
                    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.Scope.Add("fusion");
                    options.SaveTokens = true;

                })
                ;

            services.AddRazorPages();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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
