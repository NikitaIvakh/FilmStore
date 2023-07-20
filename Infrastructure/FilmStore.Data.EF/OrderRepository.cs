using Microsoft.EntityFrameworkCore;

namespace FilmStore.Data.EF
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        private readonly DbContextFactory _dbContextFactory;

        public async Task<Order> CreateAsync()
        {
            var dbContext = _dbContextFactory.Create(typeof(OrderRepository));

            var dbContextFactory = Order.DtoFactory.Create();
            dbContext.Orders.Add(dbContextFactory);
            await dbContext.SaveChangesAsync();

            return Order.Mapper.Map(dbContextFactory);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var dbContext = _dbContextFactory.Create(typeof(OrderRepository));

            var dbContextFactory = await dbContext.Orders.Include(order => order.Items).SingleAsync(order => order.Id == id);
            return Order.Mapper.Map(dbContextFactory);
        }

        public async Task UpdateAsync(Order order)
        {
            var dbContext = _dbContextFactory.Create(typeof(OrderRepository));
            await dbContext.SaveChangesAsync();
        }
    }
}