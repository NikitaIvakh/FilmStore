using FilmStore.Messages;
using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using System.Reflection;

namespace FilmStore.Presentation.Application
{
    public class OrderService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected ISession Session => _httpContextAccessor.HttpContext.Session;

        public OrderService(IFilmRepository filmRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            _filmRepository = filmRepository;
            _orderRepository = orderRepository;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool hasValue, OrderModel model)> TryGetModelAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();

            if (hasValue)
                return (true, await MapAsync(order));

            return (false, null);
        }

        internal async Task<(bool hasValue, Order order)> TryGetOrderAsync()
        {
            if (Session.TryGetCart(out Cart cart))
            {
                var order = await _orderRepository.GetByIdAsync(cart.OrderId);
                return (true, order);
            }

            return (false, null);
        }

        internal async Task<OrderModel> MapAsync(Order order)
        {
            var films = await GetFilmsAsync(order);
            var items = from item in order.Items
                        join film in films on item.FilmId equals film.Id
                        select new OrderItemModel
                        {
                            FilmId = film.Id,
                            Title = film.Title,
                            Author = film.Author,
                            Price = item.Price,
                            Count = item.Count,
                        };

            return new OrderModel
            {
                Id = order.Id,
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        internal async Task<IEnumerable<Film>> GetFilmsAsync(Order order)
        {
            var filmsIds = order.Items.Select(item => item.FilmId);
            return await _filmRepository.GetAllByIdsAsync(filmsIds);
        }

        public async Task<OrderModel> AddFilmAsync(int filmId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few films to add");

            var (hasValue, order) = await TryGetOrderAsync();

            if (!hasValue)
                order = await _orderRepository.CreateAsync();

            await AddOrUpdateFilmAsync(order, filmId, count);
            UpdateSession(order);

            return await MapAsync(order);
        }

        internal async Task AddOrUpdateFilmAsync(Order order, int filmId, int count)
        {
            var film = await _filmRepository.GetByIdAsync(filmId);

            if (order.Items.TryGet(filmId, out OrderItem orderItem))
                orderItem.Count += count;

            else
                order.Items.Add(film.Id, film.Price, count);

            await _orderRepository.UpdateAsync(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public async Task<OrderModel> UpdateFilmAsync(int filmId, int count)
        {
            var order = await GetOrderAsync();
            order.Items.Get(filmId).Count = count;

            await _orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> RemoveFilmAsync(int filmId)
        {
            var order = await GetOrderAsync();
            order.Items.Remove(filmId);

            await _orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<Order> GetOrderAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();

            if (hasValue)
                return order;

            throw new InvalidOperationException("Empty session.");
        }

        public async Task<OrderModel> SendConfirmationAsync(string cellPhone)
        {
            var order = await GetOrderAsync();
            var model = await MapAsync(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; // todo: random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                await _notificationService.SendConfirmationCodeAsync(formattedPhone, confirmationCode);
            }

            else
                model.Errors["cellPhone"] = "Номер телефона не соответствует формату +375(29)508-43-81";

            return model;
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "BY");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                return true;
            }

            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }

        public async Task<OrderModel> ConfirmCellPhoneAsync(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Что-то случилось. Попробуйте получить код ещё раз.";
                return model;
            }

            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Неверный код. Проверьте и попробуйте ещё раз.";
                return model;
            }

            var order = await GetOrderAsync();
            order.CellPhone = cellPhone;
            await _orderRepository.UpdateAsync(order);

            Session.Remove(cellPhone);
            return await MapAsync(order);
        }

        public async Task<OrderModel> SetDeliveryAsync(OrderDelivery delivery)
        {
            var order = await GetOrderAsync();
            order.Delivery = delivery;

            await _orderRepository.UpdateAsync(order);
            return await MapAsync(order);
        }

        public async Task<OrderModel> SetPaymentAsync(OrderPayment payment)
        {
            var order = await GetOrderAsync();
            order.Payment = payment;

            await _orderRepository.UpdateAsync(order);
            Session.RemoveCart();

            await _notificationService.StartProcessAsync(order);
            return await MapAsync(order);
        }
    }
}