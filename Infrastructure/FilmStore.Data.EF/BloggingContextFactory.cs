using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FilmStore.Data.EF
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<FilmStoreDbContext>
    {
        public FilmStoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FilmStoreDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FilmStore;Username=postgres;Password=postgres");

            return new FilmStoreDbContext(optionsBuilder.Options);
        }
    }
}