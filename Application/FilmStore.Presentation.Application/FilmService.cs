namespace FilmStore.Presentation.Application
{
    public class FilmService
    {
        private readonly IFilmRepository _filmRepository;

        public FilmService(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<FilmModel> GetByIdAsync(int id)
        {
            var film = await _filmRepository.GetByIdAsync(id);
            return Map(film);
        }

        public async Task<IReadOnlyCollection<FilmModel>> GetAllByQueryAsync(string query)
        {
            var films = Film.IsIMDb(query) ? await _filmRepository.GetAllByIMDbAsync(query) : await _filmRepository.GetAllByTitleOrAuthorAsync(query);
            return films.Select(Map).ToArray();
        }

        private FilmModel Map(Film film)
        {
            return new FilmModel
            {
                Id = film.Id,
                IMDb = film.IMDb,
                Title = film.Title,
                Author = film.Author,
                Description = film.Description,
                Price = film.Price,
            };
        }
    }
}