using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace JWT
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => {
                    options.SaveToken                 = true;
                    options.RequireHttpsMetadata      = true; // использовать SSL при отправке токена
                    options.TokenValidationParameters = new TokenValidationParameters {
                        // установка издателя токена
                        ValidIssuer              = "MyAuthServer",
                        // установка потребителя токена
                        ValidAudience            = "MyAuthClient",
                        // установка ключа безопасности
                        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey_texqtgwxknlho")),
                        // сдвиг часов при проверке времени
                        ClockSkew                = TimeSpan.Zero,
                        // валидировать издателя токена
                        ValidateIssuer           = true,
                        // валидировать потребителя токена
                        ValidateAudience         = true,
                        // валидировать ключ безопасности
                        ValidateIssuerSigningKey = true,
                        // валидировать время существования
                        ValidateLifetime         = true,
                    };
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new {
                        controller = "Home",
                        action = "Index"
                    });
            });
        }
    }
}
