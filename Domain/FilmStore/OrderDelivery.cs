namespace FilmStore
{
    public class OrderDelivery
    {
        public OrderDelivery(string uniqueCode, string description, decimal price, IReadOnlyDictionary<string, string> parameters)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentException(nameof(uniqueCode));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(nameof(description));

            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            UniqueCode = uniqueCode;
            Description = description;
            Price = price;
            Parameters = parameters;
        }

        public string UniqueCode { get; }

        public string Description { get; }

        public decimal Price { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }
    }
}