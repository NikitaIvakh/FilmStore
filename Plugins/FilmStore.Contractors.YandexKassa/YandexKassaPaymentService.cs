using FilmStore.Presentation.Contractors;
using Microsoft.AspNetCore.Http;

namespace FilmStore.Contractors.YandexKassa
{
    public class YandexKassaPaymentService : IPaymentService, IWebContractorService
    {
        public YandexKassaPaymentService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        private HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        public string Name => "YandexKassa";

        public string Title => "Оплата банковской картой";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name).AddParameter("orderId", order.Id.ToString());
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid Yandex.Kassa payment step.");

            return Form.CreateLast(Name, step + 1, values);
        }

        public OrderPayment GetPayment(Form form)
        {
            if (form.ServiceName != Name || !form.IsFinal)
                throw new InvalidOperationException("Invalid payment form.");

            return new OrderPayment(Name, "Оплатой картой", form.Parameters);
        }

        public Task<Uri> StartSessionAsync(IReadOnlyDictionary<string, string> parameters, Uri returnUri)
        {
            var uri = StartSession(parameters, returnUri);
            return Task.FromResult(uri);
        }

        private Uri StartSession(IReadOnlyDictionary<string, string> parameters, Uri returnUri)
        {
            var queryString = QueryString.Create(parameters);
            queryString += QueryString.Create("returnUri", returnUri.ToString());

            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = "YandexKassa/",
                Query = queryString.ToString(),
            };

            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;

            return builder.Uri;
        }
    }
}