using FilmStore.Presentation.Application;
using Microsoft.AspNetCore.Mvc;

namespace FilmStore.Presentation.Controllers
{
    public class SearchController : Controller
    {
        private readonly FilmService filmService;

        public SearchController(FilmService filmService)
        {
            this.filmService = filmService;
        }

        public async Task<IActionResult> Index(string query)
        {
            var films = await filmService.GetAllByQueryAsync(query);
            return View("Index", films);
        }
    }
}