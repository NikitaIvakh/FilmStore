using Microsoft.EntityFrameworkCore;

namespace FilmStore.Data.EF
{
    public class FilmRepository : IFilmRepository
    {
        public FilmRepository(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        private readonly DbContextFactory _dbContextFactory;

        public async Task<Film> GetByIdAsync(int id)
        {
            var dbContext = _dbContextFactory.Create(typeof(FilmRepository));
            var dbContextFactory = await dbContext.Films.SingleAsync(film => film.Id == id);

            return Film.Mapper.Map(dbContextFactory);
        }

        public async Task<Film[]> GetAllByIdsAsync(IEnumerable<int> filmIds)
        {
            var dbContext = _dbContextFactory.Create(typeof(FilmRepository));
            var films = await dbContext.Films.Where(film => filmIds.Contains(film.Id)).ToArrayAsync();

            return films.Select(Film.Mapper.Map).ToArray();
        }

        public async Task<Film[]> GetAllByIMDbAsync(string imdb)
        {
            var dbContext = _dbContextFactory.Create(typeof(FilmRepository));

            if (Film.TryFormatIMDb(imdb, out string formattedIMDb))
            {
                var filmsDtos = await dbContext.Films.Where(film => film.IMDb == formattedIMDb).ToArrayAsync();
                return filmsDtos.Select(Film.Mapper.Map).ToArray();
            }

            return Array.Empty<Film>();
        }

        public async Task<Film[]> GetAllByTitleOrAuthorAsync(string titleOrAuthor)
        {
            var dbContext = _dbContextFactory.Create(typeof(FilmRepository));

            var parameter = "'" + titleOrAuthor.Replace("'", "''") + ":*'";
            var filmsDtos = await dbContext.Films.FromSqlInterpolated($@"SELECT * FROM ""Films"" WHERE ""SearchVector"" @@ to_tsquery('russian', {parameter})").ToArrayAsync();

            return filmsDtos.Select(Film.Mapper.Map).ToArray();
        }
    }
}