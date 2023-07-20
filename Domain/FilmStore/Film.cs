using FilmStore.Data;
using System.Text.RegularExpressions;

namespace FilmStore
{
    public class Film
    {
        internal Film(FilmDTO filmDTO)
        {
            _filmDTO = filmDTO;
        }

        private readonly FilmDTO _filmDTO;

        public int Id => _filmDTO.Id;

        public string IMDb
        {
            get => _filmDTO.IMDb;
            set
            {
                if (TryFormatIMDb(value, out string formattedIMDb))
                    _filmDTO.IMDb = formattedIMDb;

                throw new ArgumentException(nameof(IMDb));
            }
        }

        public string Author
        {
            get => _filmDTO.Author;
            set => _filmDTO.Author = value?.Trim();
        }

        public string Title
        {
            get => _filmDTO.Title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Title));

                _filmDTO.Title = value.Trim();
            }
        }

        public string Description
        {
            get => _filmDTO.Description;
            set => _filmDTO.Description = value;
        }

        public decimal Price
        {
            get => _filmDTO.Price;
            set => _filmDTO.Price = value;
        }

        public static bool TryFormatIMDb(string imdb, out string formattedIMDb)
        {
            if (imdb == null)
            {
                formattedIMDb = null;
                return false;
            }

            formattedIMDb = imdb.Replace("-", "").Replace(" ", "").ToUpper();
            return Regex.IsMatch(formattedIMDb, @"^ID\d{7}(\d{3})?$");
        }

        public static bool IsIMDb(string imdb) => TryFormatIMDb(imdb, out _);

        public static class DtoFactory
        {
            public static FilmDTO Create(string imdb, string author, string title, string description, decimal price)
            {
                if (TryFormatIMDb(imdb, out string formattedIMDb))
                    imdb = formattedIMDb;

                else
                    throw new ArgumentException(nameof(imdb));

                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentException(nameof(title));

                return new FilmDTO
                {
                    IMDb = imdb,
                    Author = author?.Trim(),
                    Title = title.Trim(),
                    Description = description?.Trim(),
                    Price = price,
                };
            }
        }

        public static class Mapper
        {
            public static Film Map(FilmDTO dto) => new Film(dto);

            public static FilmDTO Map(Film domain) => domain._filmDTO;
        }
    }
}