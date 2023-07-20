using FilmStore.Data;

namespace FilmStore.Test
{
    public class OrderItemCollectionTest
    {
        private static Order CreateTesstOrder()
        {
            return new Order(new OrderDTO
            {
                Id = 1,
                Items = new List<OrderItemDTO>
                {
                    new OrderItemDTO { FilmId = 1, Price = 10m, Count = 3 },
                    new OrderItemDTO { FilmId = 2, Price = 100m, Count = 5 },
                }
            });
        }

        [Fact]
        public void Get_WithExistingItem_ReturnsItem()
        {
            var order = CreateTesstOrder();
            var orderItem = order.Items.Get(1);
            Assert.Equal(3, orderItem.Count);
        }

        [Fact]
        public void Get_WithNonExistingItem_ThrowsInvalidOperationException()
        {
            var order = CreateTesstOrder();
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Get(100);
            });
        }

        [Fact]
        public void Add_WithExistingItem_ThrowInvalidOperationException()
        {
            var order = CreateTesstOrder();
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(1, 10m, 10);
            });
        }

        [Fact]
        public void Add_WithNewItem_SetsCount()
        {
            var order = CreateTesstOrder();
            order.Items.Add(4, 30m, 10);
            Assert.Equal(10, order.Items.Get(4).Count);
        }

        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = CreateTesstOrder();
            order.Items.Remove(1);
            Assert.Collection(order.Items, item => Assert.Equal(2, item.FilmId));
        }

        [Fact]
        public void Remove_WithNonExistingItem_ThrowsInvalidOperationException()
        {
            var order = CreateTesstOrder();
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Remove(100);
            });
        }
    }
}