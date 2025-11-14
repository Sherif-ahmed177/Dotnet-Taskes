using Microsoft.AspNetCore.Mvc;
using InventoryHub.Models;

namespace InventoryHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static List<Item> _items = new()
        {
            new Item { Id = 1, Name = "Keyboard", Quantity = 10 },
            new Item { Id = 2, Name = "Mouse", Quantity = 25 }
        };

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(new { success = true, data = _items });
        }

        [HttpPost]
        public IActionResult AddItem(Item item)
        {
            item.Id = _items.Max(i => i.Id) + 1;
            _items.Add(item);

            return Ok(new { success = true, message = "Item added", data = item });
        }
    }
}
