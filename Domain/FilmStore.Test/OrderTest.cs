using FilmStore.Data;

namespace FilmStore.Test
{
    public class OrderTest
    {
        private static Order CreateEmptyTestOrder()
        {
            return new Order(new OrderDTO
            {
                Id = 1,
                Items = Array.Empty<OrderItemDTO>(),
            });
        }

        private static Order CreateTestOrder()
        {
            return new Order(new OrderDTO
            {
                Id = 1,
                Items = new OrderItemDTO[]
                {
                    new OrderItemDTO { FilmId = 1, Price = 10m, Count = 3 },
                    new OrderItemDTO { FilmId = 2, Price = 100m, Count = 5 },
                }
            });
        }

        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {
            var order = CreateEmptyTestOrder();
            Assert.Equal(0, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = CreateEmptyTestOrder();
            Assert.Equal(0m, order.TotalCount);
        }

        [Fact]
        public void TotalCount_WithNonEmptyItems_CalcualtesTotalCount()
        {
            var order = CreateTestOrder();
            Assert.Equal(3 + 5, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithNonEmptyItems_CalcualtesTotalPrice()
        {
            var order = CreateTestOrder();
            Assert.Equal(3 * 10m + 5 * 100m, order.TotalPrice);
        }
    }
}