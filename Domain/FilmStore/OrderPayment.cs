﻿namespace FilmStore
{
    public class OrderPayment
    {
        public OrderPayment(string uniqueCode, string description, IReadOnlyDictionary<string, string> parameters)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentException(nameof(uniqueCode));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(nameof(description));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            UniqueCode = uniqueCode;
            Description = description;
            Parameters = parameters;
        }

        public string UniqueCode { get; }

        public string Description { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }
    }
}