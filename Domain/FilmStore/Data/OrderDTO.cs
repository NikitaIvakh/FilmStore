namespace FilmStore.Data
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string CellPhone { get; set; }

        public string DeliveryUniqueCode { get; set; }

        public string DeliveryDescription { get; set; }

        public decimal DeliveryPrice { get; set; }

        public Dictionary<string, string> DeliveryParameters { get; set; } = new Dictionary<string, string>();

        public string PaymentServiceName { get; set; }

        public string PaymentDescription { get; set; }

        public Dictionary<string, string> PaymentParameters { get; set; } = new Dictionary<string, string>();

        public IList<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }
}