using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookDistribution.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookDistribution.Controllers
{
    [Route("order/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }

        [HttpGet("{orderId}")]
        public string Get(string orderId)
        {
            var db = this.SelectOrderContext();
            var order = db.Order.SingleOrDefault(o => o.Id == orderId);
            return JsonConvert.SerializeObject(order);
        }

        [HttpPost]
        public string Post([FromBody]string value)
        {
            var db = this.SelectOrderContext();
            JObject o = JObject.Parse(value);
            var guid = Guid.NewGuid();
            var orderId = $"orderid-{guid}";
            var order = new Order($"{orderId}", o["Body"].ToObject<List<Book>>(), (string)o["Destination"]);
            db.Order.Add(order);
            db.SaveChanges();
            return orderId;
        }

        [HttpPut("{id}")]
        public void Put(string orderId, [FromBody]string value)
        {
            var db = this.SelectOrderContext();
            JObject o = JObject.Parse(value);
            var order = db.Order.Single(b => b.Id == orderId);
            order.Books = o["Body"].ToObject<List<Book>>();
            order.DestinationStoreId = (string)o["Destination"];
            db.Order.Update(order);
            db.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var db = this.SelectOrderContext();
            var order = db.Order.SingleOrDefault(b => b.Id == id);
            db.Order.Remove(order);
            db.SaveChanges();
        }

        private OrderContext SelectOrderContext()
        {
            if (this._context != null)
            {
                return this._context;
            }
            else
            {
                return new OrderContext(new DbContextOptions<OrderContext>());
            }
        }
    }
}
