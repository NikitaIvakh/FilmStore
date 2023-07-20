using FilmStore.Presentation.Application;
using Microsoft.AspNetCore.Mvc;

namespace FilmStore.Presentation.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmService _filmService;

        public FilmController(FilmService filmService)
        {
            _filmService = filmService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var model = await _filmService.GetByIdAsync(id);
            return View(model);
        }
    }
}