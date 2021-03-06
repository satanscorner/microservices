using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly ItemDB _db;

        public CatalogController(ILogger<CatalogController> logger, ItemDB db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IResult> CreateItem(Item item)
        {
            
            _db.Items.Add(item);
            await _db.SaveChangesAsync();
            return Results.Created($"/{item.Id}", item);
        }

        [HttpGet]
        [Route("allitems")]
        public async Task<ActionResult<List<Item>>> GetAllItems()
        {
            return await _db.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            var item = await _db.Items.FindAsync(id);

            if(item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<List<Item>>> SearchItem(string title)
        {
            var items = await _db.Items.Where(i => i.Title.Contains(title)).ToListAsync();

            if(items == null)
                return NotFound();

            return Ok(items);
        }

        [HttpGet]
        public string GetCatalogItems()
        {
            var items =  _db.Items.ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(items);
        }

        [HttpPut]
        public async Task<IResult> PutTodo(Item item1)
        {
            var item2 = await _db.Items.FindAsync(item1.Id);

            if(item2 == null)
                return Results.NotFound();
            
            item2.Title = item1.Title;
            item2.Description = item1.Description;
            item2.UnitPrice = item1.UnitPrice;

            await _db.SaveChangesAsync();

            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteItem(long id)
        {
            if(await _db.Items.FindAsync(id) is Item item)
            {
                _db.Items.Remove(item);
                await _db.SaveChangesAsync();
                return Results.Ok(item);
            }

            return Results.NotFound();
        }

    }
}