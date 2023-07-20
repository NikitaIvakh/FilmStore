namespace FilmStore.Presentation.Application
{
    public class Cart
    {
        public Cart(int orderId, int totalCount, decimal totalPrice)
        {
            OrderId = orderId;
            TotalCount = totalCount;
            TotalPrice = totalPrice;
        }

        public int OrderId { get; }

        public int TotalCount { get; }

        public decimal TotalPrice { get; }
    }
}