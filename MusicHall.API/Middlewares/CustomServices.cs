using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicHall.Core;
using MusicHall.Data;
using MusicHall.Services;
using MusicHall.Services.Authentication;
using MusicHall.Services.Authentication.JWT;
using MusicHall.Services.Publications;
using MusicHall.Services.Security;
using MusicHall.Services.Users;

namespace MusicHall.Web.Middlewares
{
    public static class CustomServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            /////DBCONTEXT
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Context")));
            services.AddTransient<IDbContext, ApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            //REPOSITORY
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IJwtFactory, JwtFactory>();
            //HTTP CONTEXT ACCESSOR
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //AUTHENTICATION SERVICES
            //Scoped car récupère le current Admin et le met en cache pour éviter d'interroger trop souvent le HttpContext
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
            services.AddTransient<IUserRegistrationService, UserRegistrationService>();

            //WORK CONTEXT
            services.AddScoped<IWorkContext, WorkContext>();

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IPublicationService, PublicationService>();

            return services;
        }
    }
}
