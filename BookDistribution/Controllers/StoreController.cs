using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookDistribution.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookDistribution.Controllers
{
    [Route("store/[controller]")]
    public class StoreController : Controller
    {
        private readonly StoreContext _context;

        public StoreController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("{storeId}")]
        public string Get(string storeId)
        {
            var db = this.SelectStoreContext();
            var store = db.Store.SingleOrDefault(b => b.Id == storeId);
            return JsonConvert.SerializeObject(store);
        }

        [HttpPost]
        public string Post([FromBody]string value)
        {
            var db = this.SelectStoreContext();
            JObject o = JObject.Parse(value);
            var guid = Guid.NewGuid();
            var storeId = $"storeid-{guid}";
            Store store = new Store($"{storeId}", o["Body"].ToObject<List<Book>>());
            db.Store.Add(store);
            db.SaveChanges();
            return storeId;
        }

        [HttpPut]
        public void Put(string storeId, [FromBody]string value)
        {
            var db = this.SelectStoreContext();
            JObject o = JObject.Parse(value);
            var store = db.Store.Single(s => s.Id == storeId);
            store.Books = o["Body"].ToObject<List<Book>>();
            db.Store.Update(store);
            db.SaveChanges();
        }

        [HttpDelete]
        public void Delete(string storeId)
        {
            var db = this.SelectStoreContext();
            var store = db.Store.SingleOrDefault(s => s.Id == storeId);
            db.Store.Remove(store);
            db.SaveChanges();
        }

        private StoreContext SelectStoreContext()
        {
            if (this._context != null)
            {
                return this._context;
            }
            else
            {
                return new StoreContext(new DbContextOptions<StoreContext>());
            }
        }
    }
}
