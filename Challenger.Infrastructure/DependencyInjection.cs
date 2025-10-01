using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Challenger.Domain.Interfaces;
using Challenger.Infrastructure.Context;
using Challenger.Infrastructure.Percistence.Repositoryes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Challenger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<CGContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MotoGridDB"));
            });
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<IPatioRepository, PatioRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
