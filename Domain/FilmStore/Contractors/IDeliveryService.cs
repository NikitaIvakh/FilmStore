namespace FilmStore.Contractors
{
    public interface IDeliveryService
    {
        public string Name { get; }

        public string Title { get; }

        public Form FirstForm(Order order);

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values);

        public OrderDelivery GetDelivery(Form form);
    }
}