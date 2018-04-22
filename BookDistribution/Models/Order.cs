using System.Collections.Generic;

namespace BookDistribution.Models
{
    public class Order
    {
        public Order(string orderid, List<Book> books, string destinationid)
        {
            this.Id = orderid;
            this.Books = books;
            this.DestinationStoreId = destinationid;
        }

        public Order() {}

        public string Id { get; set; }
        public List<Book> Books { get; set; }
        public string DestinationStoreId { get; set; }
    }
}
