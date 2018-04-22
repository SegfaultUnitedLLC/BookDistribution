namespace BookDistribution.Models
{
    public class Book
    {
        public Book(string id, string title, string author, string publisher)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Publisher = publisher;
        }

        public Book() {}

        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
    }
}
