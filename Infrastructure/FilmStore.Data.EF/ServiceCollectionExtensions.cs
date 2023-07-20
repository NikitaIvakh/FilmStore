using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FilmStore.Data.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FilmStoreDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            }, ServiceLifetime.Transient);

            services.AddScoped<Dictionary<Type, FilmStoreDbContext>>();
            services.AddScoped<DbContextFactory>();

            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}