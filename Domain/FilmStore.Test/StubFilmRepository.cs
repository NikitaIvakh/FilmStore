//namespace FilmStore.Test
//{
//    public class StubFilmRepository : IFilmRepository
//    {
//        public Film[] ResultOfGetFilmByIsbn { get; set; }

//        public Film[] ResultOfGetFilmByTitleOrAuthor { get; set; }

//        public Film[] GetAllByIsbn(string isbn)
//        {
//            return ResultOfGetFilmByIsbn;
//        }

//        public Film[] GetAllByTitleOrAuthor(string titleOrAuthor)
//        {
//            return ResultOfGetFilmByTitleOrAuthor;
//        }

//        public Film GetById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Film[] GetAllByIds(IEnumerable<int> FilmIds)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Film[]> GetAllByIsbnAsync(string isbn)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Film[]> GetAllByTitleOrAuthorAsync(string titleOrAuthor)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Film> GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}