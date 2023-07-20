using FilmStore.Data;
using System.Collections;

namespace FilmStore
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {
        private readonly OrderDTO _orderDto;
        private readonly List<OrderItem> _items;

        public OrderItemCollection(OrderDTO orderDto)
        {
            if (orderDto == null)
                throw new ArgumentNullException(nameof(orderDto));

            _orderDto = orderDto;
            _items = orderDto.Items.Select(OrderItem.Mapper.Map).ToList();
        }

        public int Count => _items.Count;

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_items as IEnumerable).GetEnumerator();
        }

        public OrderItem Get(int filmId)
        {
            if (TryGet(filmId, out OrderItem orderItem))
                return orderItem;

            throw new InvalidOperationException("Film not found.");
        }

        public bool TryGet(int filmId, out OrderItem orderItem)
        {
            var index = _items.FindIndex(item => item.FilmId == filmId);
            if (index == -1)
            {
                orderItem = null;
                return false;
            }

            orderItem = _items[index];
            return true;
        }

        public OrderItem Add(int filmId, decimal price, int count)
        {
            if (TryGet(filmId, out OrderItem orderItem))
                throw new InvalidOperationException("Film already exists.");

            var orderItemDto = OrderItem.DtoFactory.Create(_orderDto, filmId, price, count);
            _orderDto.Items.Add(orderItemDto);
            orderItem = OrderItem.Mapper.Map(orderItemDto);

            _items.Add(orderItem);
            return orderItem;
        }

        public void Remove(int filmId)
        {
            var index = _items.FindIndex(item => item.FilmId == filmId);
            if (index == -1)
                throw new InvalidOperationException("Can't find film to remove from order.");

            _orderDto.Items.RemoveAt(index);
            _items.RemoveAt(index);
        }
    }
}