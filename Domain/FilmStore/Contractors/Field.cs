namespace FilmStore.Contractors
{
    abstract public class Field
    {
        protected Field(string label, string name, string value)
        {
            Label = label;
            Name = name;
            Value = value;
        }

        public string Label { get; }

        public string Name { get; }

        public string Value { get; }
    }

    public class SelectionField : Field
    {
        public IReadOnlyDictionary<string, string> Items { get; }

        public SelectionField(string label, string name, string value, IReadOnlyDictionary<string, string> items)
            : base(label, name, value)
        {
            Items = items;
        }
    }
}