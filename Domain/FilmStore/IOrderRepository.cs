namespace FilmStore
{
    public interface IOrderRepository
    {
        public Task<Order> CreateAsync();

        public Task<Order> GetByIdAsync(int id);

        public Task UpdateAsync(Order order);
    }
}