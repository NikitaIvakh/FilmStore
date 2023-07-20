namespace FilmStore.Presentation.Application
{
    public class OrderItemModel
    {
        public int FilmId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}