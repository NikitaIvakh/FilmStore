using FilmStore.Data;

namespace FilmStore.Test
{
    public class OrderItemTest
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                OrderItem.DtoFactory.Create(new OrderDTO(), 1, 10m, count);
            });
        }

        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                OrderItem.DtoFactory.Create(new OrderDTO(), 1, 10m, count);
            });
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetsCount()
        {
            var orderItem = OrderItem.DtoFactory.Create(new OrderDTO(), 1, 10m, 2);
            Assert.Equal(1, orderItem.FilmId);
            Assert.Equal(10m, orderItem.Price);
            Assert.Equal(2, orderItem.Count);
        }

        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOfRangeException()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDTO(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = -1;
            });
        }

        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOfRangeException()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDTO(), 1, 10m, 10);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = 0;
            });
        }

        [Fact]
        public void Count_WithPositiveValue_SetsValue()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDTO(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);

            orderItem.Count = 10;
            Assert.Equal(10, orderItem.Count);
        }
    }
}