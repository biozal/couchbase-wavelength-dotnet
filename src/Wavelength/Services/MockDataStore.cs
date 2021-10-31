using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Wavelength.Models;

namespace Wavelength.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = "Item: 100001", Title = "Verizon Hoodie", ImageUrl = "https://m.media-amazon.com/images/I/71h+PYDfj2L._AC_UX679_.jpg", StartTime = DateTime.Now.AddMinutes(15), StopTime = DateTime.Now.AddMinutes(20)},
                new Item { Id = "Item: 100010", Title = "Verizon Hat", ImageUrl="https://m.media-amazon.com/images/I/717O-j9rK6L._AC_UX679_.jpg", StartTime = DateTime.Now.AddMinutes(25), StopTime = DateTime.Now.AddMinutes(30) },
                new Item { Id = "Item: 100011", Title = "Verizon Shirt", ImageUrl="https://m.media-amazon.com/images/I/61PBN5WFtVL._AC_UX679_.jpg", StartTime = DateTime.Now.AddMinutes(35), StopTime = DateTime.Now.AddMinutes(40)},
                new Item { Id = "Item: 100100", Title = "Verizon Socks", ImageUrl="https://m.media-amazon.com/images/I/91j7WZeH0ZL._AC_UY879_.jpg", StartTime = DateTime.Now.AddMinutes(45), StopTime = DateTime.Now.AddMinutes(50)},
            };
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
	        var currentItems = items.Where(x=>x.StopTime >= DateTime.Now).ToList();
            return await Task.FromResult(currentItems);
        }
    }
}
