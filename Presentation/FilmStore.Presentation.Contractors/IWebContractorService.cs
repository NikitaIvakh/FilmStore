namespace FilmStore.Presentation.Contractors
{
    public interface IWebContractorService
    {
        public string Name { get; }

        public Task<Uri> StartSessionAsync(IReadOnlyDictionary<string, string> parameters, Uri returnUri);
    }
}