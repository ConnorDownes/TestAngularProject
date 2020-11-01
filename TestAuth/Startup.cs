using AngularShop.Models.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TestAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AngularShop.Services.Interfaces;
using AngularShop.Services;
using AngularShop.ViewModels.ConfigurationOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System;

namespace TestAuth
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
            services.AddDbContext<ShopDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<User>(m =>
            {
                m.Password.RequiredLength = 8;
                m.Password.RequireDigit = true;
                m.Password.RequireUppercase = true;
                m.Password.RequireLowercase = true;
            })
                .AddEntityFrameworkStores<ShopDbContext>()
                .AddDefaultTokenProviders();

            // DI

            // Classes

            services.AddTransient<SecurityTokenHandler, JwtSecurityTokenHandler>();
            services.AddTransient<SecurityTokenDescriptor>();
            services.AddTransient<ITokenService, JwtTokenService>();
            services.AddTransient<ClaimsIdentityService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IMfaService, MfaService>();
            services.AddHttpContextAccessor();

            // Options

            services.Configure<AppSettingsOptions>(Configuration.GetSection(AppSettingsOptions.AppSettings));
            services.Configure<JwtConfigOptions>(Configuration.GetSection(JwtConfigOptions.JwtConfig));

            //

            services.AddControllersWithViews().AddNewtonsoftJson();

            var AppSettings = Configuration.GetSection(nameof(AppSettingsOptions.AppSettings));
            var JwtConfig = Configuration.GetSection(nameof(JwtConfigOptions.JwtConfig));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings[nameof(AppSettingsOptions.Secret)])),
                        ValidateIssuer = true,
                        ValidIssuer = JwtConfig[nameof(JwtConfigOptions.Issuer)],
                        ValidateAudience = true,
                        ValidAudience = JwtConfig[nameof(JwtConfigOptions.Audience)],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Explore role-based auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                options.AddPolicy("Member", policy => policy.RequireClaim("Role", "Member"));
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // spa.UseAngularCliServer(npmScript: "start"); // Serve angular via cli
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // Serve angular manually
                }
            });
        }
    }
}
