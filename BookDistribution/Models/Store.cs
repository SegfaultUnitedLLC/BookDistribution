using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookDistribution.Models
{
    public class Store
    {
        public string Id { get; set; }
        public List<Book> Books { get; set; }

        public Store(string id, List<Book> books)
        {
            this.Id = id;
            this.Books = books;
        }
        
        public Store() {}
    }
}
