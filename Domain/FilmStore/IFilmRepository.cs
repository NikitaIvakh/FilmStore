namespace FilmStore
{
    public interface IFilmRepository
    {
        public Task<Film> GetByIdAsync(int id);

        public Task<Film[]> GetAllByIdsAsync(IEnumerable<int> filmIds);

        public Task<Film[]> GetAllByIMDbAsync(string IMDb);

        public Task<Film[]> GetAllByTitleOrAuthorAsync(string titleOrAuthor);
    }
}