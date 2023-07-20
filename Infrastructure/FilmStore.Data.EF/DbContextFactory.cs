using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FilmStore.Data.EF
{
    public class DbContextFactory
    {
        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public FilmStoreDbContext Create(Type repositoryType)
        {
            var services = _httpContextAccessor.HttpContext.RequestServices;

            var dbContexts = services.GetService<Dictionary<Type, FilmStoreDbContext>>();
            if (!dbContexts.ContainsKey(repositoryType))
                dbContexts[repositoryType] = services.GetService<FilmStoreDbContext>();

            return dbContexts[repositoryType];
        }
    }
}