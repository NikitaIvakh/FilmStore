using FilmStore.Data;

namespace FilmStore
{
    public class Order
    {
        public Order(OrderDTO dto)
        {
            _orderDTO = dto;
            Items = new OrderItemCollection(dto);
        }

        private readonly OrderDTO _orderDTO;

        public int Id => _orderDTO.Id;

        public string CellPhone
        {
            get => _orderDTO.CellPhone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CellPhone));

                _orderDTO.CellPhone = value;
            }
        }

        public OrderDelivery Delivery
        {
            get
            {
                if (_orderDTO.DeliveryUniqueCode == null)
                    return null;

                return new OrderDelivery(_orderDTO.DeliveryUniqueCode, _orderDTO.DeliveryDescription, _orderDTO.DeliveryPrice, _orderDTO.DeliveryParameters);
            }

            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Delivery));

                _orderDTO.DeliveryUniqueCode = value.UniqueCode;
                _orderDTO.DeliveryDescription = value.Description;
                _orderDTO.DeliveryPrice = value.Price;
                _orderDTO.DeliveryParameters = value.Parameters.ToDictionary(key => key.Key, key => key.Value);
            }
        }

        public OrderPayment Payment
        {
            get
            {
                if (_orderDTO.PaymentServiceName == null)
                    return null;

                return new OrderPayment(_orderDTO.PaymentServiceName, _orderDTO.PaymentDescription, _orderDTO.PaymentParameters);
            }

            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Payment));

                _orderDTO.PaymentServiceName = value.UniqueCode;
                _orderDTO.PaymentDescription = value.Description;
                _orderDTO.PaymentParameters = value.Parameters.ToDictionary(key => key.Key, key => key.Value);
            }
        }

        public OrderItemCollection Items { get; }

        public int TotalCount => Items.Sum(item => item.Count);

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Count) + (Delivery?.Price ?? 0m);

        public static class DtoFactory
        {
            public static OrderDTO Create() => new OrderDTO();
        }

        public static class Mapper
        {
            public static Order Map(OrderDTO dto) => new Order(dto);

            public static OrderDTO Map(Order domain) => domain._orderDTO;
        }
    }
}