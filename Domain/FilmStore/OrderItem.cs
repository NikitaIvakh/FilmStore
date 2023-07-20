using FilmStore.Data;

namespace FilmStore
{
    public class OrderItem
    {
        internal OrderItem(OrderItemDTO orderItemDTO)
        {
            _orderItemDTO = orderItemDTO;
        }

        private readonly OrderItemDTO _orderItemDTO;

        public int FilmId => _orderItemDTO.FilmId;

        public int Count
        {
            get { return _orderItemDTO.Count; }
            set
            {
                ThrowIfInvalidCount(value);
                _orderItemDTO.Count = value;
            }
        }

        public decimal Price
        {
            get => _orderItemDTO.Price;
            set => _orderItemDTO.Price = value;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greater than zero.");
        }

        public static class DtoFactory
        {
            public static OrderItemDTO Create(OrderDTO order, int filmId, decimal price, int count)
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                ThrowIfInvalidCount(count);

                return new OrderItemDTO
                {
                    FilmId = filmId,
                    Price = price,
                    Count = count,
                    Order = order,
                };
            }
        }

        public static class Mapper
        {
            public static OrderItem Map(OrderItemDTO dto) => new OrderItem(dto);

            public static OrderItemDTO Map(OrderItem domain) => domain._orderItemDTO;
        }
    }
}