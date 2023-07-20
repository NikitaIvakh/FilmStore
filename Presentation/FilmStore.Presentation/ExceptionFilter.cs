using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilmStore.Presentation
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionFilter(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            // подробный список ошибок
            if (_webHostEnvironment.IsDevelopment())
                return;

            if (context.Exception.TargetSite.Name == "ThrowNoElementsException")
            {
                //context.ExceptionHandled = true;
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/NotFound.cshtml",
                    StatusCode = 404,
                };
            }
        }
    }
}
